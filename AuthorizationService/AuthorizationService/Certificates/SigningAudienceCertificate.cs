using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AuthorizationService.Certificates
{
    public class SigningAudienceCertificate : IDisposable
    {
        private readonly IConfiguration _config;
        private readonly RSA _rsa;

        public SigningAudienceCertificate(IConfiguration config)
        {
            _config = config;
            _rsa = RSA.Create();
        }

        public async Task<SigningCredentials> GetAudienceSigningKey()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var privateKeyXml = await File.ReadAllTextAsync(Path.Combine(path, _config["Jwt:rsaPrivateKeyXml"]));
            _rsa.FromXmlString(privateKeyXml);

            return new SigningCredentials(
                key: new RsaSecurityKey(_rsa),
               algorithm: SecurityAlgorithms.RsaSha256);
        }

        public void Dispose()
        {
            _rsa?.Dispose();
        }
    }
}
