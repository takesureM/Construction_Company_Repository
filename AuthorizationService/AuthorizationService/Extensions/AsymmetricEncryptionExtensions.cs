using AuthorizationService.Certificates;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace AuthorizationService.Extensions
{
    public static class AsymmetricEncryptionExtensions
    {
        public static IServiceCollection AddAsymmetricAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var issuerSigningCertificate = new SigningIssuerCertificate(config);
            RsaSecurityKey issuerSingningKey = issuerSigningCertificate.GetIssuerSigningKey();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Issuer"],
                    IssuerSigningKey = issuerSingningKey
                };
            });     
            return services;
        }
    }
}
