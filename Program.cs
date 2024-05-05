using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using OurBeautyReferralNetwork.Data;
using OurBeautyReferralNetwork.Models;
using OurBeautyReferralNetwork.Repositories;
using OurBeautyReferralNetwork.Utilities;
using Stripe;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];



// Add services to the container.
builder.Services.AddDbContext<obrnDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

Console.WriteLine($"Connection string: {connectionString}");

builder.Services.AddScoped<CustomerRepo>();
builder.Services.AddScoped<BusinessRepo>();
builder.Services.AddScoped<RoleRepo>();
builder.Services.AddScoped<UserRoleRepo>();
builder.Services.AddScoped<JWTUtilities>();
builder.Services.AddScoped<ReferralRepo>();
builder.Services.AddScoped<UserRepo>();

// Best practice is to scope the NpgsqlConnection to a "using" block
//using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
//{
//    // Connect to the database
//    conn.Open();

//    // Read rows
//    NpgsqlCommand selectCommand = new NpgsqlCommand("SELECT * FROM \"Discount\"", conn);
//    NpgsqlDataReader results = selectCommand.ExecuteReader();

//    // Enumerate over the rows
//    while (results.Read())
//    {
//        Console.WriteLine("Column 0: {0} Column 1: {1}", results[0], results[1]);
//    }
//}

//// Generate random JWT key
// var jwtKey = JWTUtilities.GenerateRandomKey(256); // Generate a 256-bit key (32 bytes)

//// Update program secrets with the generated JWT key
//var configuration = builder.Configuration;
//var jwtSection = configuration.GetSection("Jwt");
//jwtSection["Key"] = jwtKey;

// Load JWT issuer from configuration
var configuration = builder.Configuration;
var jwtSection = configuration.GetSection("Jwt");
var jwtIssuer = jwtSection["Issuer"];

//builder.Services.AddAuthentication("JwtBearer")
//    .AddJwtBearer("JwtBearer", options =>
//    {
//        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//        {
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Convert.FromBase64String(jwtKey)),
//            ValidateIssuer = true,
//            ValidIssuer = jwtIssuer,
//            ValidateAudience = true,
//            ValidAudience = jwtIssuer,
//            ValidateLifetime = true,
//            ClockSkew = TimeSpan.Zero
//        };
//    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"])),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };
    });

// Configure identity options
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});


//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowReactApp",
//        builder =>
//        {
//            builder.WithOrigins("https://calm-hill-024d52d1e.5.azurestaticapps.net/")
//                   .AllowAnyHeader()
//                   .AllowAnyMethod();
//        });
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Set Stripe API Key
StripeConfiguration.ApiKey = builder.Configuration["StripeKey"] ?? "SKey not found";


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp"); 

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();