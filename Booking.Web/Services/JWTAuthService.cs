using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using static Booking.Web.Controllers.AccountController;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Booking.Web.Models;

namespace Booking.Web.Services
{
    public interface IJWTAuthService
    {
        string CreateJwtSecurityToken(Claim[] claims);
    }
    public class JWTAuthService : IJWTAuthService
    {
        private readonly SymmetricSecurityKey key;
        private readonly SigningCredentials credentials;
        private readonly JWTAuth jwtAuthoptions;
        public JWTAuthService(IOptions<JWTAuth> jwtAuthoptions)
        {
            this.jwtAuthoptions = jwtAuthoptions.Value;
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtAuthoptions.Secret));
            credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }
        public string CreateJwtSecurityToken(Claim[] claims)
        {
            var jwtToken = new JwtSecurityToken(
                jwtAuthoptions.Issuer,
                jwtAuthoptions.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(jwtAuthoptions.AccessExpiration),
                signingCredentials: credentials
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return token;
        }
    }
}
