using GLCompanyAPI.Enums;

namespace GLCompanyAPI.Models
{
    public class ErrorResponse(ErrorCode errorCode, string message)
    {
        public string Code { get; set; } = $"[MSG{(int)errorCode}]";
        public string Message { get; set; } = message;
    }
}