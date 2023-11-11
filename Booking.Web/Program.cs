using Booking.Model;
using Booking.Web.Models;
using Booking.Web.Repository;
using Booking.Web.Repository.Interface;
using Booking.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var Configuration = builder.Configuration;
var connectionString = Configuration["ConnectionString"];
builder.Services.AddDbContext<BookingDbContext>(o => {
    o.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var key = Encoding.ASCII.GetBytes(Configuration["Jwt:secret"]);
var issuer = Configuration["Jwt:Issuer"];

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidIssuer = issuer,
//        ValidateAudience = false,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(key),
//    };
//});
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer();
builder.Services.ConfigureOptions<ConfigureJwtOptions>();
builder.Services.Configure<JWTAuth>(Configuration.GetSection("Jwt"));
builder.Services.AddSingleton<IJWTAuthService, JWTAuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPackageRepository, PackageRepository>();
var app = builder.Build();

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

app.Run();
