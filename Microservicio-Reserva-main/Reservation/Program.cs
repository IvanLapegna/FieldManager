using Application.AutoMapper;
using Application.Interfaces;
using Application.UseCases;
using Infrastructure.Command;
using Infrastructure.Persistence;
using Infrastructure.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

//Custom
var connectionString = builder.Configuration["ConnectionString"];
//builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseMySQL(connectionString));
builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSql")));

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpClient, HttpClient>();
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IReservationCommand, ReservationCommand>();
builder.Services.AddScoped<IReservationQuery, ReservationQuery>();
builder.Services.AddScoped<IReservationStatusService, ReservationStatusService>();
builder.Services.AddScoped<IReservationStatusQuery, ReservationStatusQuery>();
builder.Services.AddScoped<IPlayersService, PlayersService>();
builder.Services.AddScoped<IPlayersCommand, PlayersCommand>();
builder.Services.AddScoped<IPlayersQuery, PlayersQuery>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();



builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,   //Validar que las aplicaciones externas puedan acceder, despues revisar 
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:key"]!))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("IsAdmin", "True"));
    options.AddPolicy("ActiveUser", policy => policy.RequireClaim("IsActive", "True"));
});



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();






//AUTOMAPPER
builder.Services.AddAutoMapper(typeof(AutoMapperConfiguration));

var app = builder.Build();

app.Use(async (context, next) =>
{
    // Continúa con la solicitud
    await next();

    // Si el estado de la respuesta es 401 (No autorizado), añade los encabezados CORS
    if (context.Response.StatusCode == 401)
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");  // O usa el dominio que prefieras
        context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
        context.Response.Headers.Add("Access-Control-Allow-Headers", "Authorization, Content-Type");
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();