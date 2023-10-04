using AspNetCoreRateLimit;
using Entities.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation.ActionFilters;
using Repositories.Contracts;
using Repositories.EFCore;
using Service;
using Service.Contracts;
using System.Text;

namespace EmployeeApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration) => services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlCon")));

        public static void ConfigureServiceManager(this IServiceCollection services) => services.AddScoped<IServiceManager, ServiceManager>();
     
        public static void ConfigureEmployeeRepo(this IServiceCollection services) => services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        //public static void ConfigureEmployeeRepo(this IServiceCollection services) => services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        public static void ConfigureAuthenticaService(this IServiceCollection services,IConfiguration configuration) => services.AddScoped<IAuthenticationService, AuthenticationService>();
        public static void ConfigureRepositoryManager(this IServiceCollection services)=> services.AddScoped<IRepositoryManager,RepositoryManager>();
       
        public static void ConfigureLogService(this IServiceCollection services)=> services.AddSingleton<ILogService,LogService>();
        public static void ConfigureDataShaper(this IServiceCollection services) => services.AddScoped<IDataShaper<Employee>, DataShaper<Employee>>();

        public static void ConfigureResponceCache(this IServiceCollection services) => services.AddResponseCaching();

        public static void ConfigureHttpCache(this IServiceCollection services)=> services.AddHttpCacheHeaders(options =>
        {
            options.CacheLocation = CacheLocation.Private;
            options.MaxAge = 70;
        });

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod().WithExposedHeaders("X-Pagination");
                });
            });
        }

        public static void ConfigureFilterAttribute(this IServiceCollection services)
        {
            services.AddSingleton<LogFilterAttribute>();
            services.AddScoped<ValidationFilterAttribute>();
        }

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = (new ApiVersion(1, 0));
                options.ApiVersionReader = (new HeaderApiVersionReader("api-version"));
            });
        }

        public static void ConfigureRateLimit(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>()
            {
                new RateLimitRule()
                {
                    Endpoint="*",
                    Period="1m",
                    Limit=10
                }
            };
            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitRules;
            });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

        }

        public static void ConfigureJwtBearer(this IServiceCollection services,IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Token");
            var secretKey = jwtSettings["secretKey"];

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["validAudience"],
                    ValidIssuer = jwtSettings["validIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateLifetime= true,
                    ValidateIssuerSigningKey=true
                };
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Baris",
                        Version = "v1",
                        Description = "Baris ASP.NET Core Web API",
                        TermsOfService = new Uri("https://google.com/"),
                        Contact = new OpenApiContact
                        {
                            Name = "Barış Aydoğan",
                            Email = "baris_aydogan_36@hotmail.com",
                          
                        }
                    });

                s.SwaggerDoc("v2", new OpenApiInfo { Title = "Baris2", Version = "v2" });

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id="Bearer"
                            },
                            Name = "Bearer"
                        },
                        new List<string>()
                    }
                });
            });
        }
    }
}
