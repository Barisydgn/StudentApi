using Entities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogService _logService;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IConfiguration configuration, ILogService logService)
        {
            _configuration = configuration;
            _logService = logService;
        }

        public  Token CreateToken(bool populateExp)
        {
            Token token = new Token();
            var jwtSettings = _configuration.GetSection("Token");    
            var secretKey2 = _configuration.GetSection("Token:secretKey");
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey2.Value));

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            token.Expiration = DateTime.Now.AddMinutes(Convert.ToInt32(jwtSettings["expires"]));

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtSettings["validIssuer"]
                , audience: jwtSettings["validAudience"],
                expires:token.Expiration,
                signingCredentials:signingCredentials
                );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            token.AccessToken = tokenHandler.WriteToken(jwtSecurityToken);

            byte[] numbers=new byte[32];
          using   RandomNumberGenerator randomNumber=RandomNumberGenerator.Create();
            randomNumber.GetBytes(numbers);
            token.RefreshToken = Convert.ToBase64String(numbers);
            return  token;
            
        }

        public async Task<Token?> ValiteUser(UserAuthentication userAuthentication)
        {
            var result = (userAuthentication.UserName == "Admin" && userAuthentication.Password == "12345");
            if (!result)
            {
                _logService.LogWarning($"{nameof(ValiteUser)} : Giriş Yaparken Hata Oluştu");
                return null;
            }
            return  CreateToken(true);
        }
    }
}
