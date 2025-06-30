using DDDPlayGround.Domain.Base;

namespace DDDPlayGround.Application.Integration
{
    public interface IRestIntegrationService
    {
        Task<Response<string>> GetExternalDataAsync(string configKey, Dictionary<string, string>? parameters = null);
    }
}
