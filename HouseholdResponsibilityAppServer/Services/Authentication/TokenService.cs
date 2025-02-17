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
        private const int ExpirationMinutes = 1;

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
                "apiWithAuthBackend",  //issuer
                "apiWithAuthBackend", //audience
                claims,
                expires: expiration,
                signingCredentials: credentials
            );

        private List<Claim> CreateClaims(User user, string? role, DateTime expiration)
        {
            var claims = new List<Claim>
           {
                new(JwtRegisteredClaimNames.Sub, "TokenForTheApiWithAuth"),//tulajdonos
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),//egyedi guid
                //mikor lett kiadva a token
                new(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.UtcNow).ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                // Explicit "exp" claim hozzáadása:
                new Claim("exp", ((DateTimeOffset)expiration).ToUnixTimeSeconds().ToString())
           };

            if (!string.IsNullOrEmpty(role))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }


        private SigningCredentials CreateSigningCredentials()
        {
            //Az aláíráshoz szükséges hitelesítő adatokat hozza létre(kulcs + algoritmus)
            return new SigningCredentials(
                //  1️ Létrehozunk egy titkos kulcsot, amit a token aláírásához használunk.
                //    - A SymmetricSecurityKey egy olyan kulcs,
                //    amely ugyanazzal a titkos kulccsal írja alá és ellenőrzi a tokent.
                new SymmetricSecurityKey(
                    //  2️ A titkos kulcsot szövegként (string) adjuk meg, de azt bájt tömbbé kell alakítani.
                    //    - Encoding.UTF8.GetBytes() átalakítja a titkos kulcsot byte tömbbé,
                    //    hogy a SymmetricSecurityKey elfogadja.
                    Encoding.UTF8.GetBytes("!SomethingSecret!!SomethingSecret!")
                ),
                //  3️ Megadjuk, hogy milyen algoritmussal történjen az aláírás.
                //    - Itt a HMAC SHA256-ot (HMAC-SHA256) használjuk, ami egy
                //    biztonságos hash-alapú aláírási módszer.
                SecurityAlgorithms.HmacSha256
            );
        }
    }
}
