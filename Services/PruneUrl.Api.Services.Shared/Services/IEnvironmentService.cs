namespace PruneUrl.Api.Services.Shared.Services
{
    public interface IEnvironmentService
    {
        string GetEnvironmentVariable(string key);
    }
}
