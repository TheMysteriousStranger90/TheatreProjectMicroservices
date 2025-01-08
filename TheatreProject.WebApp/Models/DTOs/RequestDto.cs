using TheatreProject.WebApp.Models.Enums;
using ContentType = TheatreProject.WebApp.Models.Enums.ContentType;

namespace TheatreProject.WebApp.Models.DTOs;

public class RequestDto
{
    public ApiType ApiType { get; set; } = ApiType.GET;
    public string Url { get; set; }
    public object Data { get; set; }
    public string AccessToken { get; set; }
    public ContentType ContentType { get; set; } = ContentType.Json;
}