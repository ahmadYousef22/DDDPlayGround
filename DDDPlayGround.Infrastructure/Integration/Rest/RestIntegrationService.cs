using DDDPlayGround.Application.Integration;
using DDDPlayGround.Domain.Base;
using DDDPlayGround.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

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

        public async Task<Response<string>> GetAsync(string configKey, Dictionary<string, string>? parameters = null)
        {
            try
            {
                var endPointUrl =_configuration[configKey];
                if (string.IsNullOrEmpty(endPointUrl))
                {
                    return Response<string>.Failure(HttpStatusCodes.BadRequest, $"Configuration for '{configKey}' not found");
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
                    return Response<string>.Failure((HttpStatusCodes)(int)response.StatusCode, "External service call failed");
                }

                var content = await response.Content.ReadAsStringAsync();
                return Response<string>.SuccessResult(content, HttpStatusCodes.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetAsync)}");
                return Response<string>.Failure(HttpStatusCodes.InternalError, "An unexpected error occurred");
            }
        }
        public async Task<Response<TResponse>> PostAsync<TRequest, TResponse>(string configKey, TRequest payload)
        {
            try
            {
                var endPointUrl = _configuration[configKey];
                if (string.IsNullOrEmpty(endPointUrl))
                {
                    return Response<TResponse>.Failure(HttpStatusCodes.BadRequest, $"Configuration for '{configKey}' not found");
                }

                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsJsonAsync(endPointUrl, payload);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("External API POST call failed: {StatusCode} {Url}", response.StatusCode, endPointUrl);
                    return Response<TResponse>.Failure((HttpStatusCodes)(int)response.StatusCode, "External service call failed");
                }

                var result = await response.Content.ReadFromJsonAsync<TResponse>();
                if (result == null)
                {
                    return Response<TResponse>.Failure(HttpStatusCodes.InternalError, "Empty or invalid response from service");
                }

                return Response<TResponse>.SuccessResult(result, HttpStatusCodes.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(PostAsync)}");
                return Response<TResponse>.Failure(HttpStatusCodes.InternalError, "An unexpected error occurred");
            }
        }
    }
}