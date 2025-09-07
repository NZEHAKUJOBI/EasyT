namespace API.Interface
{
    public interface IOtpService
    {
        string GenerateOtp();
        void StoreOtp(string key, string otp);
        bool ValidateOtp(string key, string otp);
    }
}
