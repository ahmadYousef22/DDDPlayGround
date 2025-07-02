using DDDPlayGround.Application.Integration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDDPlayGround.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UtilityController : ControllerBase
    {
        private readonly IRestIntegrationService _restIntegrationService;
        private readonly INumberConversionService _numberConversionService;

        public UtilityController(IRestIntegrationService restIntegrationService, INumberConversionService numberConversionService)
        {
            _restIntegrationService = restIntegrationService;
            _numberConversionService = numberConversionService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAdvice()
        {
            var response = await _restIntegrationService.GetAsync("AdviceEndpoint");

            if (!response.Success)
                return StatusCode((int)response.StatusCode, response);

            return Ok(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetWords(int number)
        {
            var result = await _numberConversionService.ConvertNumberToWordsAsync(number);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetDollars(int number)
        {
            var result = await _numberConversionService.ConvertNumberToDollarsAsync(number);
            return Ok(result);
        }
    }
}
