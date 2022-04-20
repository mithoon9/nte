//using LeaveManagement.Repository;

using LeaveMgmt1.Interface;
using LeaveMgmt1.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveMgmt1
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
            
            services.AddControllers();


            //services.AddDbContextPool<ApiDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("MyConnection")));
            services.AddScoped<ITbl_user_repository, Tbl_user_repository>();
            services.AddScoped<ITbl_leave_type_repository, Tbl_leave_type_repository>();
            services.AddScoped<ITbl_leave_repository, Tbl_leave_repository>();
            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "LeaveMgmt1", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Standard Authorization header using the Bearer scheme(\"bearer {token}\"",
                    Type = SecuritySchemeType.ApiKey

                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                
            });

            services.AddAuthentication(
            
                JwtBearerDefaults.AuthenticationScheme
                
            ).AddJwtBearer(jwt =>
            {
                var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);

                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    
                    
                };
            });

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LeaveMgmt1 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();          

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
