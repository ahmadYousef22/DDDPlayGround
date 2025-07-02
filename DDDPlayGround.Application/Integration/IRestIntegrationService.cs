using DDDPlayGround.Domain.Base;

namespace DDDPlayGround.Application.Integration
{
    public interface IRestIntegrationService
    {
        Task<Response<string>> GetAsync(string configKey, Dictionary<string, string>? parameters = null);
        Task<Response<TResponse>> PostAsync<TRequest, TResponse>(string configKey, TRequest payload);
    }
}
