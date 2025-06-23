using DDDPlayGround.Shared.Enums;

namespace DDDPlayGround.Shared.Base
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static Response<T> SuccessResult(T data, string message = "")
        {
            return new Response<T>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = message,
                Data = data
            };
        }

        public static Response<T> Failure(HttpStatusCode statusCode, string message, List<string>? errors = null)
        {
            return new Response<T>
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Errors = errors
            };
        }
    }
}
