using FMSBay.BAL.IService;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FMSBay.BAL.Service
{
    public class TokenService : ITokenService
    {
        private ILogger<TokenService> _logger;
        private const double EXPIRY_DURATION_MINUTES = 30;
        private readonly IConfiguration _configuration;
        public TokenService(ILogger<TokenService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public string? BuildToken(string? key, string? issuer, string? audience, IEnumerable<Claim> claims)
        {

            _logger.LogTrace("Token generate");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new JwtSecurityToken(issuer, audience, claims,
                expires: DateTime.Now.AddMinutes(EXPIRY_DURATION_MINUTES), signingCredentials: credentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return tokenString;
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string? key, string? issuer, string? audience, string? token)
        {
            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                ValidateIssuer = false,
                ValidateAudience = false,
                //ValidIssuer = issuer,
                ValidateLifetime = false,
                //ValidAudience = audience,
                IssuerSigningKey = mySecurityKey,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public bool IsValidToken(string? key, string? issuer, string? audience, string? token)
        {
            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

    }
}
