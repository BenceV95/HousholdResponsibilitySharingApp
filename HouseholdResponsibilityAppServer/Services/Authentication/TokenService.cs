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

        private const int ExpirationMinutes = 60;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;


        public TokenService(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<string> CreateToken(User user)
        {
            var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
            var claims = await CreateClaimsAsync(user, expiration);
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

        private async Task<List<Claim>> CreateClaimsAsync(User user, DateTime expiration)
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

            var roles = await _userManager.GetRolesAsync(user);


            foreach (var role in roles)
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
