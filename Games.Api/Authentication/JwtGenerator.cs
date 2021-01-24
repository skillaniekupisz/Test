using Games.Core.Entties.User;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Games.Api.Authentication
{
    public class JwtGenerator
    {
        private readonly AuthenticationOptions _options;

        public JwtGenerator(AuthenticationOptions options)
        {
            _options = options;
        }

        public string Generator(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(User));
            var claims = new List<Claim>
                {
                    new Claim("UserId", user.Id),
                    new Claim("FirstName", user.FirstName),
                    new Claim("Lastname", user.LastName),
                    new Claim("NickName", user.LastName),
                    new Claim("Email", user.Email),
            };
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name)));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.Key);
            var expires = (DateTime.UtcNow + _options.TokenExpiration);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
