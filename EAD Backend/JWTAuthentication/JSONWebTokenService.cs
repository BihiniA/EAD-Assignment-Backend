using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Threading.Tasks;

namespace EAD_Backend.JWTAuthentication
{
    public class JwtAuthenticationService : ITokenService
    {
        private readonly SymmetricSecurityKey _securityKey;

        public JwtAuthenticationService(SymmetricSecurityKey securityKey)
        {
            _securityKey = securityKey;
        }

        public async Task ValidateToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _securityKey,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5) // Allow a 5 minute clock skew.
            };

            SecurityToken validatedToken;
            var principal = await new JwtSecurityTokenHandler().ValidateTokenAsync(token, tokenValidationParameters, out validatedToken);

            if (principal == null)
            {
                throw new Exception("Invalid token");
            }
        }

        public string GenerateJSONWebToken(Users users) // token generation 
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("eyJhbGciOiJIUzINi"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim("email",users.email), //add email to token
            new Claim("password",users.password) // add password to token
        };
            var token = new JwtSecurityToken("EADBackend",
                "EADBackend",
                claims,
                expires: DateTime.Now.AddMinutes(120), // token expiery time
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
