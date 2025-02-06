using Application.AutoMapper;
using Application.Interfaces.Command;
using Application.Interfaces.Query;
using Application.Interfaces.Service;
using Application.Interfaces.Service.Authorization;
using Application.Interfaces.Service.Cryptography;
using Infrastructure.Command;
using Infrastructure.Jwt;
using Infrastructure.Persistence;
using Infrastructure.Query;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using Application.Validations;
using FluentValidation.AspNetCore;
using Application.UseCases.AuthServices;
using Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Seeder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Custom

builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSql")));

//Services

builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ICryptographyService, CryptographyService>();
builder.Services.AddScoped<IAuthTokenService, JwtService>();
builder.Services.AddScoped<IRegisterUserService, RegisterUserService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<ISignOutService, SignOutService>();
builder.Services.AddScoped<IPasswordResetService, PasswordResetService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICryptographyService, CryptographyService>();
builder.Services.AddScoped<IPasswordResetCommand, PasswordResetCommand>();
builder.Services.AddScoped<IPasswordResetQuery, PasswordResetQuery>();



//CQRS

builder.Services.AddScoped<IUserCommand, UserCommand>();
builder.Services.AddScoped<IUserQuery, UserQuery>();
builder.Services.AddScoped<IRefreshTokenCommand, RefreshTokenCommand>();
builder.Services.AddScoped<IRefreshTokenQuery, RefreshTokenQuery>();

//AutoMapper

builder.Services.AddAutoMapper(typeof(AutoMapperConfiguration));

//validaciones
builder.Services.AddValidatorsFromAssembly(typeof(UserRequestValidator).Assembly);
builder.Services.AddFluentValidationAutoValidation();

//TokenConfiguration

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

//Authorizations

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("IsAdmin", "True"));
    options.AddPolicy("ActiveUser", policy => policy.RequireClaim("IsActive", "True"));
});

//Obtener informacion del claim dentro del service

builder.Services.AddHttpContextAccessor();

//CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

//Seeder Admin

builder.Services.AddScoped<AdminSeeder>();


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


//Agrego los admins para la realizacion de Pruebas.

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<AdminSeeder>();
    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
