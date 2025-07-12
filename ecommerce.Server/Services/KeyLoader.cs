using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ecommerce.Server.Services
{
    public class KeyLoader
    {
        private static string _solutionRoot = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName!;
        private static string _privateKeyPath = Path.Combine(_solutionRoot, "private_key.pem");
        private static string _publicKeyPath = Path.Combine(_solutionRoot, "public_key.pem");

        public async static Task<RsaKeys> LoadRsaKeysFromPem()
        {
            string privateKeyText = await File.ReadAllTextAsync(_privateKeyPath);
            string publicKeyText = await File.ReadAllTextAsync(_publicKeyPath);

            privateKeyText = privateKeyText.Replace("-----BEGIN PRIVATE KEY-----", "").Replace("-----END PRIVATE KEY-----", "").Trim();
            publicKeyText = publicKeyText.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Trim();

            // Decode base64 data
            byte[] privateKeyBytes = Convert.FromBase64String(privateKeyText);
            byte[] publicKeyBytes = Convert.FromBase64String(publicKeyText);

            return new RsaKeys(publicKeyBytes, privateKeyBytes);
        }

        public record RsaKeys(byte[] PublicKeyBytes, byte[] PrivateKeyBytes);
    }
}
