using AspNetCoreRateLimit;
using EmployeeApi.Extensions;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using NLog;
using Service.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

builder.Services.AddControllers(options =>
{
    //XML ÝÇERÝK PAZARLIÐI
    options.RespectBrowserAcceptHeader= true;
    options.ReturnHttpNotAcceptable= true;

    options.CacheProfiles.Add("5mins", new CacheProfile() { Duration = 300 });//BURDA 300 SANÝYE KAYDETME YAPIYORUZ
}).AddXmlDataContractSerializerFormatters().AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();

builder.Services.ConfigureServiceManager();
builder.Services.ConfigureEmployeeRepo();
builder.Services.ConfigureLogService();
builder.Services.ConfigureResponceCache();
builder.Services.ConfigureHttpCache();
builder.Services.ConfigureFilterAttribute();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureCors();
builder.Services.ConfigureDataShaper();
builder.Services.ConfigureAuthenticaService(builder.Configuration);

builder.Services.ConfigureJwtBearer(builder.Configuration);

builder.Services.AddMemoryCache();//rate limit için
builder.Services.ConfigureRateLimit();

builder.Services.Configure<ApiBehaviorOptions>(options =>//422 HATA KODUNU KULLANMAK ÝÇÝN YAZDIK
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogService>();
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "Baris v1");
        s.SwaggerEndpoint("/swagger/v2/swagger.json", "Baris v2");
    });
}

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseIpRateLimiting();
app.UseCors("CorsPolicy");
app.UseResponseCaching();
app.UseHttpCacheHeaders();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
