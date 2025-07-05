using API.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace API.Services
{
    public class OtpService : IOtpService
    {
        private readonly IMemoryCache _cache;

        public OtpService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public void StoreOtp(string key, string otp)
        {
            _cache.Set($"otp_{key}", otp, TimeSpan.FromMinutes(10));
        }

        public bool ValidateOtp(string key, string otp)
        {
            if (_cache.TryGetValue($"otp_{key}", out string storedOtp))
            {
                return storedOtp == otp;
            }
            return false;
        }
    }
}
