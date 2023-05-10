using JWTAuthentication.Database.Context;
using JWTAuthentication.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var _configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false).Build();

builder.Services.AddDbContext<JWTAuthenticationContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("JWTAuthenticationDB")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<JWTAuthenticationContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = (string)_configuration.GetValue(typeof(string), "JWT:ValidAudience"),
            ValidIssuer = (string)_configuration.GetValue(typeof(string), "JWT:Issuer"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes((string)_configuration.GetValue(typeof(string), "JWT:Secret")))
        };
    });


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
