using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Net;

namespace EAD_Backend.JWTAuthentication
{
    public class ValidateTokenMiddleware
    {
        private readonly ITokenService _tokenService;

        public ValidateTokenMiddleware(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Get the token from the request header.
            string token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(' ')[1];

            // Validate the token.
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    ValidateToken(token);
                }
                catch (Exception ex)
                {
                    // If the token is invalid, return a 401 Unauthorized response.
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync(ex.Message);
                    return;
                }
            }

            // If the token is valid, continue processing the request.
            await next(context);
        }

        private void ValidateToken(string token)
        {
            // Validate the token using a JWT library.
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("eyJhbGciOiJIUzINi"));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5) // Allow a 5 minute clock skew.
            };

            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
        }
    }

}
