using Microsoft.IdentityModel.Tokens;
using SucessPointCore.Domain.Entities;
using SucessPointCore.Domain.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SucessPointCore.Api.Domain.Helpers
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

        public (string accessToken, Guid refreshToken) GenerateTokens(AuthenticatedUser user)
        {
            var symmetricKey = Encoding.UTF8.GetBytes(_secretKey); // Change here
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256);

            var tokenExpiresAfter = DateTime.UtcNow.AddMinutes(AppConfigHelper.TokenExpiryMinute);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim("utype", user.UserType.ToString()),
            new Claim("uid", user.UserID.ToString())
        }),
                NotBefore = DateTime.UtcNow,
                IssuedAt = DateTime.UtcNow,
                Expires = tokenExpiresAfter,
                SigningCredentials = signingCredentials,
                Issuer = AppConfigHelper.Issuer,
                Audience = AppConfigHelper.Audience,
                TokenType = "JWT"
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            var refreshToken = GenerateRefreshToken();

            return (accessToken, refreshToken);
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

        #region Private Functions

        private Guid GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            return new Guid(randomNumber);
        }

        #endregion


    }

}