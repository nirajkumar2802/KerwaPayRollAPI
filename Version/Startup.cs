using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Version.DataContext;
using Version.InfraStructure;
using Version.Middleware;
using Version.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

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
            services.AddScoped<IKerwaEmployeeRepo, KerwaEmployeeRepo>();
            services.AddScoped<IUserRepo, UsersRepo>();

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            ValidIssuer = "Kerwa", // Replace with your token issuer
            //            ValidAudience = "KerwaUser", // Replace with your token audience
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Asdffdgssgssgg@!gdgdgd@hh^re3364etrtrt4")) // Replace with your secret key
            //        };
            //    });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "Kerwa",
                    ValidAudience = "KerwaUser",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Asdffdgssgssgg@!gdgdgd@hh^re3364etrtrt4"))
                };
            });

            services.AddAuthorization();
            services.AddSwaggerGen( option => {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
 {
     {
           new OpenApiSecurityScheme
             {
                 Reference = new OpenApiReference
                 {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                 }
             },
             new string[] {}
     }
 });
            });
            
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
            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // app.UseMiddleware<JwtTokenMiddleware>("Kerwa", "KerwaUser", "Asdffdgssgssgg@!gdgdgd@hh^re3364etrtrt4");


            //app.UseStaticFiles();
            // app.UseRouting();
            app.UseAuthentication();
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
