using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using static ecommerce.Server.Services.KeyLoader;

namespace ecommerce.Server.Services
{
    public class TokenVerifier
    {
        public static bool VerifyJwtToken(string token, RsaKeys rsaKeys)
        {
            var publicKey = RSA.Create();
            publicKey.ImportSubjectPublicKeyInfo(rsaKeys.PublicKeyBytes, out _);

            var securityKey = new RsaSecurityKey(publicKey);
            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = true,
                ValidIssuer = "me",
                ValidateAudience = true,
                ValidAudience = "you"
            };

            try
            {
                var principal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParams, out _);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

/*
 var rsaKeys = KeyLoader.LoadRsaKeysFromPem("private_key.pem", "public_key.pem");
var token = TokenGenerator.GenerateJwtToken(rsaKeys, "your_issuer", "your_audience");
var isValid = TokenVerifier.VerifyJwtToken(token, rsaKeys);
*/