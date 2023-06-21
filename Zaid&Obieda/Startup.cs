using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaid_Obieda.Db;
using Zaid_Obieda.Models;
using Zaid_Obieda.Services;
using Zaid_Obieda.Utility;


using Newtonsoft.Json;

using Microsoft.AspNetCore.Identity.UI;
using Zaid_Obieda.Validator;
using FluentValidation;

namespace Zaid_Obieda
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<EmployeeValidator>();

            // Register your validators
            services.AddScoped<IValidator<Employee>, EmployeeValidator>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddControllers();
            services.AddDbContext<DataDbContext>(options =>
             options.UseSqlServer(
             Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<AppUser>
     (options =>
     {
         options.SignIn.RequireConfirmedAccount = true;
         // Configure password validation rules
         options.Password.RequireDigit = true; // Require at least one digit
         options.Password.RequiredLength = 6; // Minimum length requirement
         options.Password.RequireNonAlphanumeric = true; // Require at least one non-alphanumeric character
         options.Password.RequireUppercase = true; // Require at least one uppercase letter
         options.Password.RequireLowercase = true; // Require at least one lowercase letter
     })
     .AddRoles<IdentityRole>()
     .AddEntityFrameworkStores<DataDbContext>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options =>
                        {
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = false,
                                ValidateAudience = false,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("secret_Key").Value)),
                                ClockSkew = TimeSpan.Zero

                            };
                        });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

