using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JobPortal.Exceptions;

namespace JobPortal.Helpers
{
    public class JWT
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _tokenExpiryInDays;

        public JWT(IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JWT");
            _secretKey = jwtSettings["SecretKey"] ?? throw new ArgumentNullException(nameof(_secretKey));
            _issuer = jwtSettings["Issuer"] ?? throw new ArgumentNullException(nameof(_issuer));
            _audience = jwtSettings["Audience"] ?? throw new ArgumentNullException(nameof(_audience));
            _tokenExpiryInDays = int.TryParse(jwtSettings["TokenExpiryInDays"], out var expiry) ? expiry : 1;
        }

        public string GenerateToken(int userId, string name, string email, string role)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException(nameof(role));

            // Create claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("name", name),
                new Claim("email", email),
                new Claim("role", role)
            };

            // Create a key from the secret key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            // Create credentials using the key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the token
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_tokenExpiryInDays), // Token expiration time
                signingCredentials: creds);

            // Return the token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ClockSkew = TimeSpan.Zero // Remove delay of token when expire
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new TokenValidationException("Token validation failed", ex);
            }
        }
    }
}
