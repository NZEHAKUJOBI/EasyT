namespace API.DTO.Response
{
    public class ServiceResponseDto<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public ServiceResponseDto() { }

        public ServiceResponseDto(T data, bool success, string message)
        {
            Data = data;
            Success = success;
            Message = message;
        }

        // ✅ Factory methods for cleaner service returns:
        public static ServiceResponseDto<T> SuccessResponse(T data, string message = "") =>
            new ServiceResponseDto<T>(data, true, message);

        public static ServiceResponseDto<T> FailResponse(string message) =>
            new ServiceResponseDto<T>(default, false, message);

        public static ServiceResponseDto<T> MessageResponse(string message, bool success = false) =>
            new ServiceResponseDto<T>(default, success, message);
    }
}
