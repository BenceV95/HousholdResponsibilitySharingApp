using HouseholdResponsibilityAppServer.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HouseholdResponsibilityAppServer.Services.Authentication
{
    public class TokenService : ITokenService
    {
        private const int ExpirationMinutes = 10;
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(User user, string role = null)
        {
            var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
            var claims = CreateClaims(user, role, expiration);
            var signingCredentials = CreateSigningCredentials();
            var token = CreateJwtToken(claims, signingCredentials, expiration);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
            DateTime expiration) =>
            new(
                "apiWithAuthBackend",  //issuer    later get it from appsettings.json...
                "apiWithAuthBackend", //audience
                claims,
                expires: expiration,
                signingCredentials: credentials
            );

        private List<Claim> CreateClaims(User user, string? role, DateTime expiration)
        {
            var claims = new List<Claim>
           {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.UtcNow).ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new Claim("exp", ((DateTimeOffset)expiration).ToUnixTimeSeconds().ToString()),
           };

            var householdId = user.Household?.HouseholdId.ToString() ?? ""; // cannot be null unfortunatelly
            claims.Add(new Claim("householdId", householdId));

            if (!string.IsNullOrEmpty(user.GetUserRole))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }


        private SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(

                    Encoding.UTF8.GetBytes(_configuration["Jwt:IssuerSigningKey"])
                ),
                SecurityAlgorithms.HmacSha256
            );
        }
    }
}
