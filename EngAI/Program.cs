using EngAI.Models;
using EngAI.Services;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Npgsql;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddDebug();

builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddAuthentication(option =>
{
    option.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
    option.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
    option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie().AddGoogleOpenIdConnect(options =>
{
    options.ClientId = builder.Configuration["Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Google:ClientSecret"]!;
    options.Scope.Add("email");
    options.Scope.Add("profile");
});

// Add NewtonsoftJson as the default JSON serializer
builder.Services.AddControllers().AddNewtonsoftJson(option =>
{
    option.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add http client service to interact with Gemini API
builder.Services.AddHttpClient();

// Add Redis as external cache server
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.ConfigurationOptions = new()
    {
        EndPoints = { builder.Configuration.GetConnectionString("RedisEndpoint")! },
        Ssl = true,
    };
});

// Add Entity Framework Core with PostgreSQL support
builder.Services.AddNpgsql<AppDbContext>(connectionString);

// Register Dapper's IDbConnection for dependency injection
builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection(connectionString));

// Register application services
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<LessonService>();
builder.Services.AddSingleton<OpenAPIService>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<IdentityUser>();
app.MapControllers();

app.Run();
