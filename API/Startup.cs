using API.Extensions;
using API.Helpers;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _Configuration;

        public Startup(IConfiguration configuration)
        {
            _Configuration = configuration;
           
        }

        //public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ITokenService,TokenService>();
            services.AddScoped<IProductRepository,ProductRepository>();
            services.AddScoped<IBasketRepository,BasketRepository>();
            services.AddScoped(typeof(IGenericRepository<>),(typeof(GenericRepository<>)));
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddControllers();
            services.AddDbContext<StoreContext>(x=>x.UseSqlite(_Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<AppIdentityDbContext>(x=>x.UseSqlite(_Configuration.GetConnectionString("IdentityConnection")));

             services.AddSingleton<IConnectionMultiplexer>(c =>{
           
             var configuration = ConfigurationOptions.Parse(_Configuration.GetConnectionString("Redis"),true);
             
             return ConnectionMultiplexer.Connect(configuration);

            });

            services.AddCors(Opts=>{

                Opts.AddPolicy("CorsPolicy",policy=>{

                   policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });


            });
            services.AddIdentityService(_Configuration);
            services.AddSwaggerGen(c=>{

                 c.SwaggerDoc("v1", new OpenApiInfo { Title="MyShop API",Version="V1"});
                 var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Auth Bearer Scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securitySchema);
                var securityRequirement = new OpenApiSecurityRequirement {{securitySchema, new[] {"Bearer"}}};
                c.AddSecurityRequirement(securityRequirement);
            
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

            app.UseRouting();
            
            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c=>{c.SwaggerEndpoint("/swagger/v1/swagger.json","Myshop API v1");});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
