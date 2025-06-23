
namespace DDDPlayGround.Application.Integration
{
    public interface INumberConversionService
    {
        Task<string> ConvertNumberToWordsAsync(int number);
        Task<string> ConvertNumberToDollarsAsync(int number);
    }
}
