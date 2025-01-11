using System.Net;

namespace TheatreProject.Identity.Services.Interfaces;

public interface IIpBlockingService
{
    bool IsBlocked(IPAddress ipAddress);
}