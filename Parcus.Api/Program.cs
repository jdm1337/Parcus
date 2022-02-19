using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parcus.Persistence.Data;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connection = builder.Configuration["Data:CommandAPIConnection:ConnectionString"];

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connection));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ParcusApi", Version = "v1" });
});
builder.Services.AddApiVersioning(options =>
{
    // Provides to the client the different Api version that we have.
    options.ReportApiVersions = true;

    // This will allow the api to automatically provide a default version.
    options.AssumeDefaultVersionWhenUnspecified = true;

    options.DefaultApiVersion = ApiVersion.Default;


});

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

app.MapControllers();

app.Run();
