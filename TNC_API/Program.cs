using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security;
using TNC_API.Data;
using TNC_API.Interfaces;
using TNC_API.Models;
using TNC_API.Repositories;
using AutoMapper;
using TNC_API.Profiles;

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<AutoMapperProfile>(); // Replace with the name of your AutoMapper profile class
});

var mapper = new Mapper(mapperConfig);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMapper>(mapper);
builder.Services.AddScoped<DatabaseContext>();
builder.Services.AddScoped<SecuritySettings>();
builder.Services.AddScoped<IUser, UserRepository>();
builder.Services.AddScoped<ILogin, LoginRepository>();
builder.Services.AddScoped<IPettyCashRequest, PettyCashRequestRepository>();
// Add services to the container.

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")
    ));

builder.Services.AddControllers();
builder.Services.AddRouting();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", policy =>
    {
        policy.AllowAnyHeader()
                .AllowAnyMethod() //allow any http methods
                .SetIsOriginAllowed(isOriginAllowed: _ => true) //no restriction in any domain
                .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors(option => option
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
