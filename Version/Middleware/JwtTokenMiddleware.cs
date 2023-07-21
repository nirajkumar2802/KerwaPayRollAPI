namespace Version.Middleware
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.IdentityModel.Tokens;
    public class JwtTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _secretKey;

        public JwtTokenMiddleware(RequestDelegate next, string issuer, string audience, string secretKey)
        {
            _next = next;
            _issuer = issuer;
            _audience = audience;
            _secretKey = secretKey;
        }

        public async Task Invoke(HttpContext context)
        {
            // Check if the request contains a specific route or condition to generate the token

            bool IsValidUser = AuthenticateUser(context);
            if (IsValidUser)
            {
                string username = "john.doe"; // Replace with your logic to determine the username

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.Name, username)
                }),
                    Expires = DateTime.UtcNow.AddMinutes(30), // Replace with your desired token expiration
                    Issuer = _issuer,
                    Audience = _audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                // Attach the token to the response headers or cookies, or send it in the response body
                context.Response.Headers.Add("Authorization", "Bearer " + tokenHandler.WriteToken(token));
                
            }

            await _next(context);
        }

        private bool AuthenticateUser(HttpContext context)
        {
            return true;
        }
    }
}
