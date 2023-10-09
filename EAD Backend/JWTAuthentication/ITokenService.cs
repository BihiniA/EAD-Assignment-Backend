using System.Threading.Tasks;

namespace EAD_Backend.JWTAuthentication
{
    public interface ITokenService
    {
        Task ValidateToken(string token);
    }
}
