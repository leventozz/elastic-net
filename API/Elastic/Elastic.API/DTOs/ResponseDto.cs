using System.Net;

namespace Elastic.API.DTOs
{
    public record ResponseDto<T>
    {
        public T Data { get; set; }
        public List<string> Errors { get; set; }
        public HttpStatusCode Status { get; set; }


        public static ResponseDto<T> Success(T data, HttpStatusCode statusCode)
        {
            return new ResponseDto<T> { Data = data, Status = statusCode };
        }

        public static ResponseDto<T> Fail(List<string> errors, HttpStatusCode statusCode)
        {
            return new ResponseDto<T> {  Errors = errors, Status = statusCode };
        }
    }
}
