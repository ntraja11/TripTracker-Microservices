using AutoMapper;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TripTracker.Services.AuthApi;
using TripTracker.Services.AuthApi.Data;
using TripTracker.Services.AuthApi.Models;
using TripTracker.Services.AuthApi.Models.Dto;
using TripTracker.Services.AuthApi.Service;
using TripTracker.Services.AuthApi.Service.Interface;
using TripTracker.Services.AuthApi.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlOptions => sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<ResponseDto>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<BackendApiAuthenticationHttpClientHandler>();
builder.Services.AddScoped<ITravelGroupApiService, TravelGroupApiService>();

builder.Services.AddHttpClient("TravelGroupApi", u => u.BaseAddress =
    new Uri(builder.Configuration["ServiceUrls:TravelGroupApi"]))
    .AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});


builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear(); // Trust all networks
    options.KnownProxies.Clear();  // Trust all proxies
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "TripTracker.AuthApi");
    s.RoutePrefix = string.Empty;
});

app.UseForwardedHeaders();
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); 
});


app.MapControllers();

ApplyMigrations();
app.Run();


void ApplyMigrations()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
        }
    }
}
