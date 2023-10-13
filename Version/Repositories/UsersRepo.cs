using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Version.DataContext;
using Version.EntityModels;
using Version.InfraStructure;

namespace Version.Repositories
{
    public class UsersRepo : IUserRepo
    {
        private readonly PayRollKerwaDbContext _dbContext;
        public UsersRepo(PayRollKerwaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<KerwaUsers> Get()
        {
            KerwaUsers kerwaUser = new KerwaUsers();   
            string userName = "niraj";

            var tokenSecret = "Asdffdgssgssgg@!gdgdgd@hh^re3364etrtrt4"; // Replace with a strong secret key (keep it secure)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: "Kerwa",
                audience: "KerwaUser",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30), // Set the token expiration time
                signingCredentials: credentials
            );

             string s=  new JwtSecurityTokenHandler().WriteToken(token);
             kerwaUser.Token = s;
            return Task.FromResult(kerwaUser);    
        }
    }
}
