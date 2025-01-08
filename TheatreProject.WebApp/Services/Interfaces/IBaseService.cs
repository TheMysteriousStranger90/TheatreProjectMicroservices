using TheatreProject.WebApp.Models;
using TheatreProject.WebApp.Models.DTOs;

namespace TheatreProject.WebApp.Services.Interfaces;

public interface IBaseService
{
    ResponseDto responseModel { get; set; }
    Task<T> SendAsync<T>(RequestDto requestDto);
}