using API.Entities;

namespace API.Interface
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}