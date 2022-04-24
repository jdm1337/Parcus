using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parcus.Persistence.Data;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Parcus.Api.Authentication.Handlers;
using Parcus.Api.Authentication.Providers;
using Parcus.Domain.Settings;
using Parcus.Application.Interfaces.IServices;
using Parcus.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Parcus.Persistence.DataSeed;
using Parcus.Domain.Identity;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Diagnostics;
using Hangfire;
using Parcus.Api.Initial;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
string connection = builder.Configuration["Data:CommandAPIConnection:ConnectionString"];

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));
builder.Services.Configure<InitializeSettings>(builder.Configuration.GetSection("Initialize"));
builder.Services.Configure<InvestApiSettings>(builder.Configuration.GetSection("InvestApi"));

builder.Services.AddHangfire(x => 
        x.UseSqlServerStorage("Data Source=DESKTOP-P5QBGMD\\SQLEXPRESS;Initial Catalog=ParcusDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));
builder.Services.AddHangfireServer();
builder.Services.AddTransient<DataSeeder>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connection));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

builder.Services.AddTransient<IAuthService, AuthService>();
#pragma warning disable CS0436 // Type conflicts with imported type
builder.Services.AddTransient<ITokenService, TokenService>();
#pragma warning restore CS0436 // Type conflicts with imported type
builder.Services.AddTransient<IPortfolioOperationService, PortfolioOperationService>();
builder.Services.AddTransient<IDataInstrumentService, DataInstrumentService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,

        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});


builder.Services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
builder.Services.AddCors();
builder.Services.AddControllers();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddEndpointsApiExplorer();



builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ParcusApi",
        Description = "Parcus API - REST API для учета, анализа и управления инвестиционным портфелем. <br>" +
        "Так же сервис представляет открытый доступ к получению информации по активам по <a href='https://en.wikipedia.org/wiki/Financial_Instrument_Global_Identifier'>figi</a> идентификатору. <br> <br>" +
        "Специальные коды <br>" +
        "<b>429</b> код - превышен лимит на запрос к ресурсу, timeout-1 минута",
        Contact = new OpenApiContact
        {
            Name = "Taramaly Sergey",

            Email = "taramalys@gmail.com"
        },


    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

   
});
builder.Services.AddApiVersioning(options =>
{
    // Provides to the client the different Api version that we have.
    options.ReportApiVersions = true;

    // This will allow the api to automatically provide a default version.
    options.AssumeDefaultVersionWhenUnspecified = true;

    options.DefaultApiVersion = ApiVersion.Default;


});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

StartApp.Invoke(app);

// Configure the HTTP request pipeline.
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}
*/


app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
app.UseDeveloperExceptionPage();

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features
        .Get<IExceptionHandlerPathFeature>()
        .Error;
    var response = new { error = exception.Message };
    await context.Response.WriteAsJsonAsync(response);
}));
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseHangfireDashboard("/dashboard");

app.MapControllers();

app.Run();


