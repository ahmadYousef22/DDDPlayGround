using DDDPlayGround.Domain.Enums;

namespace DDDPlayGround.Domain.Base
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public HttpStatusCodes StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static Response<T> Failure(HttpStatusCodes statusCode, string message, List<string>? errors = null)
        {
            return new Response<T>
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }

        public static Response<T> SuccessResult(T data, HttpStatusCodes statusCode = HttpStatusCodes.OK)
        {
            return new Response<T>
            {
                Success = true,
                StatusCode = statusCode,
                Data = data
            };
        }
    }
}
