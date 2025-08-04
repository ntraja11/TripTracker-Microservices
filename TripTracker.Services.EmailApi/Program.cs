using DotNetEnv;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.EntityFrameworkCore;
using TripTracker.Services.EmailApi.Extensions;
using TripTracker.Services.EmailApi.Messaging;
using TripTracker.Services.EmailApi.Services;
using TripTracker.Services.TripApi.Data;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();

//builder.Services.AddDbContext<EmailDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//var optionsBuilder = new DbContextOptionsBuilder<EmailDbContext>();
//optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddSingleton(new EmailService());

var app = builder.Build();

//app.UseSwagger();
//app.UseSwaggerUI(s =>
//{
//    s.SwaggerEndpoint("/swagger/v1/swagger.json", "TripTracker.AuthApi");
//    s.RoutePrefix = string.Empty;
//});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//ApplyMigrations();

app.UseAzureServiceBusConsumer();

app.MapGet("/status", () => Results.Ok("EmailApi is running"));

app.Run();

//void ApplyMigrations()
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var dbContext = scope.ServiceProvider.GetRequiredService<EmailDbContext>();
//        if (dbContext.Database.GetPendingMigrations().Any())
//        {
//            dbContext.Database.Migrate();
//        }
//    }
//}
