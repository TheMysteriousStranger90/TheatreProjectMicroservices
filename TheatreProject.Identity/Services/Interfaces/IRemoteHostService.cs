using System.Net;

namespace TheatreProject.Identity.Services.Interfaces;

public interface IRemoteHostService
{
    public IPAddress? GetRemoteHostIpAddressUsingRemoteIpAddress(HttpContext httpContext);

    public IPAddress? GetRemoteHostIpAddressUsingXForwardedFor(HttpContext httpContext);

    public IPAddress? GetRemoteHostIpAddressUsingXRealIp(HttpContext httpContext);
}