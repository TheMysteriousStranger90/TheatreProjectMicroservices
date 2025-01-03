﻿using TheatreProject.WebApp.Models;

namespace TheatreProject.WebApp.Services.Interfaces;

public interface IBaseService
{
    Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true);
}