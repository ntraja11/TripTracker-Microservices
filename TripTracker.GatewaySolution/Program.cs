using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using TripTracker.GatewaySolution.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("Ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);
builder.AddAppAuthentication();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");



app.Use(async (context, next) =>
{
    Console.WriteLine("Incoming Headers:");
    foreach (var header in context.Request.Headers)
    {
        Console.WriteLine($"{header.Key}: {header.Value}");
    }
    await next.Invoke();
});


app.UseOcelot().GetAwaiter().GetResult();
app.Run();
