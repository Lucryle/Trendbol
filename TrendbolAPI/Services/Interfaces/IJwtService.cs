using TrendbolAPI.Models;

namespace TrendbolAPI.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
