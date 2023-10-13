using System.Security.Cryptography;

namespace Version.Middleware
{
    
    //using Microsoft.AspNetCore.Http;
    //using Microsoft.Extensions.Options;
    //using Microsoft.IdentityModel.Tokens;
    //using System;
    //using System.IdentityModel.Tokens.Jwt;
    //using System.Linq;
    //using System.Text;
    //using System.Threading.Tasks;

    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
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

            // Check if Basic Authentication header is present
            if (context.Request.Headers.ContainsKey("Authorization") &&
                context.Request.Headers["Authorization"].ToString().StartsWith("Basic"))
            {
                // Get the Base64 encoded credentials from the Authorization header
                string encodedCredentials = context.Request.Headers["Authorization"].ToString().Substring("Basic ".Length);

                // Decode the Base64 credentials to get the username and password
                string credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
                string[] parts = credentials.Split(':');

                if (parts.Length == 2)
                {
                    string username = parts[0];
                    string password = parts[1];

                    // Now you have the username and password
                }
            }
            return true;
        }
    }

    //public class JwtTokenMiddleware
    //{
    //    private readonly RequestDelegate _next;
    //    private readonly AppSettings _appSettings;
    //    private readonly Client client;

    //    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    //    {
    //        _next = next;
    //        _appSettings = appSettings.Value;
    //        client = new Client();
    //    }

    //    public async Task Invoke(HttpContext context, IUserService userService)
    //    {
    //        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

    //        if (token != null)
    //            attachUserToContext(context, userService, token);

    //        await _next(context);
    //    }
    //    private void attachUserToContext(HttpContext context, IUserService userService, string token)
    //    {
    //        try
    //        {
    //            var tokenHandler = new JwtSecurityTokenHandler();
    //            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
    //            tokenHandler.ValidateToken(token, new TokenValidationParameters
    //            {
    //                ValidateIssuerSigningKey = true,
    //                IssuerSigningKey = new SymmetricSecurityKey(key),
    //                ValidateIssuer = false,
    //                ValidateAudience = false,
    //                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
    //                ClockSkew = TimeSpan.Zero
    //            }, out SecurityToken validatedToken);

    //            var jwtToken = (JwtSecurityToken)validatedToken;
    //            if (jwtToken != null)
    //            {
    //                int userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
    //                bool isValid = client.ValidateClientByUserId(userId);
    //                if (isValid)
    //                    context.Items["AgentId"] = userId;
    //                else
    //                    throw new UnauthorizedAccessException("Unauthorized");

    //                string clientinfo = jwtToken.Claims.First(x => x.Type == "clientinfo").Value;
    //                if (!string.IsNullOrEmpty(clientinfo))
    //                {
    //                    string[] lstClientinfo = clientinfo.Split('|');
    //                    if (lstClientinfo != null && lstClientinfo.Length == 2)
    //                    {
    //                        context.Items["clientId"] = lstClientinfo[0];
    //                        context.Items["Subdomain"] = lstClientinfo[1];
    //                    }
    //                }
    //            }
    //        }
    //        catch
    //        {
    //            // do nothing if jwt validation fails
    //            // user is not attached to context so request won't have access to secure routes
    //        }
    //    }
    //}
}
