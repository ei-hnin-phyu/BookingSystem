using Booking.Model;
using Booking.Web.Repository;
using Booking.Web.Repository.Interface;
using Booking.Web.Services;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Pipelines.Sockets.Unofficial;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;
using System.Configuration;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var Configuration = builder.Configuration;
ConfigureSerilog();
var connectionString = Configuration["ConnectionString"];
builder.Services.AddDbContext<BookingDbContext>(o => {
    o.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
var redis = ConfigureRedisConnection(Configuration);
builder.Services.AddHangfire(configuration =>
{
    configuration.UseRedisStorage(redis, new RedisStorageOptions { Prefix = "bookingsystem", Db = 1 });
});
builder.Services.AddHangfireServer();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPackageRepository, PackageRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddHostedService<JobManager>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseHangfireDashboard();
app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 5, DelaysInSeconds = new int[] { 1 } });
app.Run();

ConnectionMultiplexer ConfigureRedisConnection(IConfiguration configuration)
{
    // add redis client
    var redisServerType = configuration.GetSection("Redis:ServerType").Value;
    var redisHost = configuration.GetSection("Redis:Host").Value;
    var redisPort = int.Parse(configuration.GetSection("Redis:Port").Value);
    var redisPassword = configuration.GetSection("Redis:Password").Value;
    var redisDatabase = int.Parse(configuration.GetSection("Redis:Database").Value);
    var option = new ConfigurationOptions
    {
        AbortOnConnectFail = true,
        ConnectRetry = 3,
        ConnectTimeout = 10000,
        AllowAdmin = true,
        EndPoints = { { redisHost, redisPort } },
    };
    ConnectionMultiplexer redisConn = ConnectionMultiplexer.Connect(option);    
    var cacheDb = redisConn.GetDatabase(redisDatabase);
    builder.Services.AddSingleton<ConnectionMultiplexer>(s => redisConn);
    builder.Services.AddSingleton<IDatabase>(s => cacheDb);
    return redisConn;
}
void ConfigureSerilog()
{
    Serilog.Debugging.SelfLog.Enable(Console.WriteLine);

    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        //.Enrich.WithExceptionDetails()
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
        .MinimumLevel.Override("System", LogEventLevel.Error)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
       .WriteTo.Console()
       .CreateLogger();
}