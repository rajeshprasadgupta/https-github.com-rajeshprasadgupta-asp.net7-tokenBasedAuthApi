using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TokenBasedAuthApi.Dto;

namespace TokenBasedAuthApi.Services
{
    public interface ITokenService
    {
        // Generate a JWT token for the specified user name, roles and expirationTime
        TokenResponseDto CreateToken(string username, List<string> roles, int expirationInMinutes = 60);
    }

    public class TokenService : ITokenService
    {
        
        private readonly string _issuer;
        private readonly SigningCredentials _jwtSigningCredentials;
        private readonly string _audiences;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
        {
            _logger = logger;
            //The recipient that the JWT is intended for, that is, who can consume the token
            _audiences = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience not found.");
            //Issuer identifies the name of the entity that is creating the token
            _issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer not found.");
            //key to sign the bearer is known only by the server, so using SymmetricSecurityKey, which is never shared with clients
            //the key should be at least 32 bytes or 16 characters long
            var issuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key not found.")));
            //we defined the credentials (SigningCredentials) to sign the JWT bearer
            //specified the Hash-Based Message Authentication Code (HMAC) and the hash function, SHA256, specifying the SecurityAlgorithms.HmacSha256 value
            _jwtSigningCredentials = new SigningCredentials(issuerSigningKey,
                SecurityAlgorithms.HmacSha256Signature);
        }

        public TokenResponseDto CreateToken(string username, List<string> roles, int expirationInMinutes = 60)
        {
            _logger.LogDebug("Creating token for {username} which expires after {expirationInMinutes} minutes", username, expirationInMinutes);
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            //bearer contains information that allows verifying the user identity, along with other declarations ( Claims ) that describe the properties of the user
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, username));

            var id = Guid.NewGuid().ToString().GetHashCode().ToString("x", CultureInfo.InvariantCulture);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, id));
            foreach (string role in roles)
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            //JwtSecurityTokenHandler is responsible for actually generating the bearer token
            var handler = new JwtSecurityTokenHandler();
            var expires = DateTime.UtcNow.AddMinutes(expirationInMinutes);
            var jwtToken = handler.CreateJwtSecurityToken(
            issuer: _issuer,
            audience: _audiences,
            subject: identity,
            notBefore: DateTime.UtcNow,
            expires: expires,
            issuedAt: DateTime.UtcNow,
            signingCredentials: _jwtSigningCredentials);
            var token = handler.WriteToken(jwtToken);
            return new TokenResponseDto { AccessToken = token, Expiration= expires };
        }
    }
}
