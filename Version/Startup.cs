using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Version.DataContext;
using Version.InfraStructure;
using Version.Middleware;
using Version.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Version
{
    public class Startup
    {
        public IConfiguration configRoot
        {
            get;
        }
        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpClient();
            services.AddDbContext<PayRollKerwaDbContext>(options => options.UseSqlServer(configRoot.GetConnectionString("DefaultConnection")));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "Kerwa", // Replace with your token issuer
                        ValidAudience = "KerwaUser", // Replace with your token audience
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Asdffdgssgssgg@!gdgdgd@hh^re3364etrtrt4")) // Replace with your secret key
                    };
                });

            services.AddAuthorization();
            services.AddSwaggerGen();
            services.AddScoped<IKerwaEmployeeRepo, KerwaEmployeeRepo>();
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseCors();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseMiddleware<JwtTokenMiddleware>("Kerwa", "KerwaUser", "Asdffdgssgssgg@!gdgdgd@hh^re3364etrtrt4");


            //app.UseStaticFiles();
            // app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V2");
            });
            app.Run();
        }
    }
}
