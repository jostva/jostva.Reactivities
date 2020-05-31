#region usings

using jostva.Reactivities.application.Interfaces;
using jostva.Reactivities.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

#endregion

namespace jostva.Reactivities.Infrastructure.Security
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly SymmetricSecurityKey key;


        public JwtGenerator(IConfiguration configuration)
        {
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));
        }


        //public string CreateToken(AppUser user)
        //{
        //    List<Claim> claims = new List<Claim>()
        //    {
        //        //new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
        //        new Claim(ClaimTypes.NameIdentifier, user.UserName)
        //    };

        //    //  Generate signing credentials
        //    SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        //    SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
        //    {
        //        Subject = new ClaimsIdentity(claims),
        //        Expires = DateTime.Now.AddDays(7),
        //        SigningCredentials = credentials
        //    };

        //    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        //    SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        //    return tokenHandler.WriteToken(token);
        //}


        public string CreateToken(AppUser user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}