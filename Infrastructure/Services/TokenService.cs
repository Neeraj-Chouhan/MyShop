using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _Key;

        public TokenService(IConfiguration config)
        {
            _config = config;
            _Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));

        }
        public string CreateToken(AppUser appUser)
        {
            var claim = new List<Claim>{

                new Claim(JwtRegisteredClaimNames.Email,appUser.Email),
                new Claim(JwtRegisteredClaimNames.GivenName , appUser.DisplayName)
            };

                var creds = new SigningCredentials(_Key,SecurityAlgorithms.HmacSha512);

                var tokenDescriptor = new SecurityTokenDescriptor{
                 
                 Subject = new ClaimsIdentity(claim),
                 Expires = DateTime.Now.AddDays(7),
                 SigningCredentials = creds,
                 Issuer = _config["Token:Issuer"]

                };
            

                var TokenHandler = new JwtSecurityTokenHandler();

                var token = TokenHandler.CreateToken(tokenDescriptor);

                return TokenHandler.WriteToken(token);
                
            
        }
    }
}