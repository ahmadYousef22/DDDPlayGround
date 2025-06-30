using DDDPlayGround.Application.Integration;
using DDDPlayGround.Domain.Base;
using DDDPlayGround.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DDDPlayGround.Infrastructure.Integration.Rest
{
    public class RestIntegrationService : IRestIntegrationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<RestIntegrationService> _logger;
        private readonly IConfiguration _configuration;


        public RestIntegrationService(IHttpClientFactory httpClientFactory, ILogger<RestIntegrationService> logger, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<Response<string>> GetExternalDataAsync(string configKey, Dictionary<string, string>? parameters = null)
        {
            try
            {
                var endPointUrl = "https://api.adviceslip.com/advice";  //_configuration["AdviceEndpoint"];
                if (string.IsNullOrEmpty(endPointUrl))
                {
                    return Response<string>.Failure(HttpStatusCode.BadRequest, $"Configuration for '{configKey}' not found");
                }

                if (parameters != null)
                {
                    foreach (var key in parameters.Keys)
                    {
                        endPointUrl = endPointUrl.Replace($"{{{key}}}", parameters[key]);
                    }
                }

                var client = _httpClientFactory.CreateClient();

                var response = await client.GetAsync(endPointUrl);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("External API call failed: {StatusCode} {Url}", response.StatusCode, endPointUrl);
                    return Response<string>.Failure((HttpStatusCode)(int)response.StatusCode, "External service call failed");
                }

                var content = await response.Content.ReadAsStringAsync();
                return Response<string>.SuccessResult(content, "External service call succeeded");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetExternalDataAsync");
                return Response<string>.Failure(HttpStatusCode.InternalError, "An unexpected error occurred");
            }
        }
    }
}
