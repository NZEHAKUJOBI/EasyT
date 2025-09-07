namespace backend.Api.DTO.Response

{
    public class EmailOtpDto
    {
        public string Email { get; set; } = string.Empty;
        public string OtpCode { get; set; } = string.Empty;
    }
}