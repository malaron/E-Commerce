using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static ecommerce.Server.Services.KeyLoader;

namespace ecommerce.Server.Services
{
    public class TokenGenerator
    {
        public static string GenerateJwtToken(RsaKeys rsaKeys, string username, string issuer, string audience)
        {
            RSA privateKey = RSA.Create();
            privateKey.ImportPkcs8PrivateKey(rsaKeys.PrivateKeyBytes, out _);

            RsaSecurityKey securityKey = new(privateKey);
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.RsaSha256);

            DateTime now = DateTime.UtcNow;

            Claim[] claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];

            SecurityTokenDescriptor descriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = issuer,
                Audience = audience,
                IssuedAt = now,
                NotBefore = now,
                SigningCredentials = credentials
            };

            SecurityToken token = new JwtSecurityTokenHandler().CreateToken(descriptor);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
