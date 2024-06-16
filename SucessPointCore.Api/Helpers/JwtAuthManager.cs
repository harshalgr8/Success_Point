using Microsoft.IdentityModel.Tokens;
using SucessPointCore.Domain.Entities;
using SucessPointCore.Domain.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace SucessPointCore.Api.Helpers
{
    public class JwtAuthManager
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtAuthManager(string secretKey, string issuer, string audience)
        {
            _secretKey = secretKey;
            _issuer = issuer;
            _audience = audience;
        }

        public (string accessToken, Guid refreshToken) GenerateTokens(User user)
        {

            var symmetricKey = Convert.FromBase64String(_secretKey);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                        {
                            new Claim("name", user.UserName),
            new Claim("uid", user.UserID.ToString())
                        }),

                Expires = now.AddMinutes(Convert.ToInt32(AppConfigHelper.TokenExpiryMinute)),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            var refreshToken = GenerateRefreshToken();


            return (accessToken, refreshToken);
        }

        private Guid GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            return new Guid(randomNumber);
        }


       

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(AppConfigHelper.JWTTokenEncryptionKey);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                return principal;
            }

            catch (Exception)
            {
                return null;
            }
        }

    }

}