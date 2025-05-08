using Microsoft.AspNetCore.Authentication.Cookies;
using TripTracker.Web.Service;
using TripTracker.Web.Service.Interface;
using TripTracker.Web.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddHttpClient<ITravelGroupService, TravelGroupService>();
builder.Services.AddHttpClient<ITripService, TripService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();

StaticDetail.TravelGroupApiBasePath = builder.Configuration["ServiceUrls:TravelGroupAPI"];
StaticDetail.TripApiBasePath = builder.Configuration["ServiceUrls:TripAPI"];
StaticDetail.AuthApiBasePath = builder.Configuration["ServiceUrls:AuthAPI"];

builder.Services.AddScoped<ITokenHandler, TokenHandler>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ITravelGroupService, TravelGroupService>();
builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
