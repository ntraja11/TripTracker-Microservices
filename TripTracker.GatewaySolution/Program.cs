using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using TripTracker.GatewaySolution.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddAppAuthentication();

if (builder.Environment.ToString().ToLower().Equals("production"))
{
    builder.Configuration.AddJsonFile("Ocelot.Production.json", optional: false, reloadOnChange: true);
}
else
{
    //builder.Configuration.AddJsonFile("Ocelot.Development.json", optional: false, reloadOnChange: true);
    builder.Configuration.AddJsonFile("Ocelot.json", optional: false, reloadOnChange: true);
}

builder.Services.AddOcelot(builder.Configuration);

builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);


var app = builder.Build();

app.MapGet("/", () => "Gateway is running");



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
