using TripTracker.Web.Service;
using TripTracker.Web.Service.Interface;
using TripTracker.Web.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<ITravelGroupService, TravelGroupService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();

StaticDetail.TravelGroupApiBasePath = builder.Configuration["ServiceUrls:TravelGroupAPI"];
StaticDetail.AuthApiBasePath = builder.Configuration["ServiceUrls:AuthAPI"];

builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ITravelGroupService, TravelGroupService>();
builder.Services.AddScoped<IAuthService, AuthService>();




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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
