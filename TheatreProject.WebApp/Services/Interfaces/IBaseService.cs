using TheatreProject.WebApp.Models;

namespace TheatreProject.WebApp.Services.Interfaces;

public interface IBaseService
{
    ResponseDto responseModel { get; set; }
    Task<T> SendAsync<T>(RequestDto requestDto);
}