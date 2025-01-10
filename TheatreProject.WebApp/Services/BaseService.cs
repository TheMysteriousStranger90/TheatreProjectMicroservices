using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using TheatreProject.WebApp.Models;
using TheatreProject.WebApp.Models.DTOs;
using TheatreProject.WebApp.Models.Enums;
using TheatreProject.WebApp.Services.Interfaces;

namespace TheatreProject.WebApp.Services;

public class BaseService : IBaseService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<BaseService> _logger;
    public ResponseDto responseModel { get; set; }

    public BaseService(IHttpClientFactory httpClientFactory, ILogger<BaseService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        this.responseModel = new ResponseDto();
    }

    public async Task<T> SendAsync<T>(RequestDto requestDto)
    {
        try
        {
            using var client = _httpClientFactory.CreateClient("TheatreProjectAPI");
            using var message = new HttpRequestMessage();

            _logger.LogInformation("Sending request to {Url} with type {ApiType}",
                requestDto.Url, requestDto.ApiType);

            if (requestDto.Data != null)
            {
                if (requestDto.ContentType == ContentType.MultipartFormData)
                {
                    message.Headers.Add("Accept", "*/*");
                    var content = new MultipartFormDataContent();

                    foreach (var prop in requestDto.Data.GetType().GetProperties())
                    {
                        var value = prop.GetValue(requestDto.Data);
                        if (value is IFormFile file)
                        {
                            if (file != null)
                            {
                                _logger.LogInformation("Adding file {FileName} to request", file.FileName);
                                content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                            }
                        }
                        else if (value != null)
                        {
                            _logger.LogInformation("Adding field {PropertyName}={Value}", prop.Name, value);
                            content.Add(new StringContent(value.ToString()), prop.Name);
                        }
                    }

                    message.Content = content;
                }
                else
                {
                    var jsonContent = JsonConvert.SerializeObject(requestDto.Data);
                    message.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    _logger.LogInformation("Request JSON content: {Content}", jsonContent);
                }
            }

            message.RequestUri = new Uri(requestDto.Url);
            message.Method = requestDto.ApiType switch
            {
                ApiType.POST => HttpMethod.Post,
                ApiType.PUT => HttpMethod.Put,
                ApiType.DELETE => HttpMethod.Delete,
                _ => HttpMethod.Get
            };

            if (!string.IsNullOrEmpty(requestDto.AccessToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", requestDto.AccessToken);
            }

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.SendAsync(message);
            var contentRes = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("API Response: Status={StatusCode}, Content={Content}",
                response.StatusCode, contentRes);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("API returned error status code: {StatusCode}", response.StatusCode);
                throw new HttpRequestException($"API returned {response.StatusCode}: {contentRes}");
            }

            return JsonConvert.DeserializeObject<T>(contentRes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending request");
            throw;
        }
    }
}