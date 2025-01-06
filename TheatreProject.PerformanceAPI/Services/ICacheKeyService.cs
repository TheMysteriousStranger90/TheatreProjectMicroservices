namespace TheatreProject.PerformanceAPI.Services;

public interface ICacheKeyService
{
    void AddKey(string key);
    void RemoveKey(string key);
    IEnumerable<string> GetKeysStartingWith(string prefix);
}