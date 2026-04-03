using courier.Interfaces;
using courier.Middlewares;
using courier.Services;
using Serilog;
using Serilog.Sinks.GoogleCloudLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File(
        path: "logs/app-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        fileSizeLimitBytes: 10_000_000,
        rollOnFileSizeLimit: true,
        shared: true
    )
    .WriteTo.Console()
    .CreateLogger();

builder.Services.AddScoped<IRecieveService, RecieveService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();