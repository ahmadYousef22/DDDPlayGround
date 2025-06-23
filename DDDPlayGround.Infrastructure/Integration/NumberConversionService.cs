using DDDPlayGround.Application.Integration;
using Microsoft.Extensions.Logging;
using NumberConversionServiceReference;

namespace DDDPlayGround.Infrastructure.Integration
{
    public class NumberConversionService : INumberConversionService
    {
        private readonly ILogger<NumberConversionService> _logger;
        private readonly NumberConversionSoapTypeClient _client;

        public NumberConversionService(ILogger<NumberConversionService> logger)
        {
            _logger = logger;
            _client = new NumberConversionSoapTypeClient(NumberConversionSoapTypeClient.EndpointConfiguration.NumberConversionSoap);
        }

        public async Task<string> ConvertNumberToWordsAsync(int number)
        {
            try
            {
                var result = await _client.NumberToWordsAsync((uint)number);
                return result.Body.NumberToWordsResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error converting number to words");
                throw;
            }
        }

        public async Task<string> ConvertNumberToDollarsAsync(int number)
        {
            try
            {
                var result = await _client.NumberToDollarsAsync(number);
                return result.Body.NumberToDollarsResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error converting number to dollars");
                throw;
            }
        }

    }
}
