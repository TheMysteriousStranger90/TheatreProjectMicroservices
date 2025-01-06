namespace TheatreProject.WebApp.Services.Interfaces;

public interface ITokenProvider
{
    void SetToken(string token);
    Task<string> GetTokenAsync();
    void ClearToken();
}