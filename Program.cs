using moneygram_api.Services.Interfaces;
using moneygram_api.Services.Implementations;
using moneygram_api.Services.Background;
using moneygram_api.Settings;
using moneygram_api.Middleware;
using moneygram_api.Data;
using moneygram_api.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MoneyGram API", Version = "v1" });

    // Add JWT Authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Register the ISendConsumerLookUp service
builder.Services.AddScoped<ISendConsumerLookUp, SendConsumerLookUp>();

// Register the IFetchCodeTable service
builder.Services.AddScoped<IFetchCodeTable, FetchCodeTable>();

// Register the ISendFeeLookUp service
builder.Services.AddScoped<IFeeLookUp, FeeLookUp>();

// Register the IGFFP service
builder.Services.AddScoped<IGFFP, SendGFFP>();

// Register the ISendValidation service
builder.Services.AddScoped<ISendValidation, SendValidation>();

// Register the ICommitTransaction service
builder.Services.AddScoped<ICommitTransaction, CommitTransaction>();

// Register the IGetCountryInfo service
builder.Services.AddScoped<IGetCountryInfo, GetCountryInfo>();

// Register the ICurrencyInfo service
builder.Services.AddScoped<IFetchCurrencyInfo, FetchCurrencyInfo>();

// Register the IConfigurations service
builder.Services.AddSingleton<IConfigurations, Configurations>();

// Register the ILoggingService
builder.Services.AddScoped<ILoggingService, LoggingService>();

// Register the ICustomerLookupService
builder.Services.AddScoped<ICustomerLookupService, CustomerLookupService>();

// Register the IAuthService
builder.Services.AddScoped<IAuthService, AuthService>();

// Register the IMGSendTransactionService
builder.Services.AddScoped<IMGSendTransactionService, MGSendTransactionService>();

// Register the ISaveRewards service
builder.Services.AddScoped<ISaveRewards, SaveRewards>();

//Register the ISignatureVerificationService
builder.Services.AddScoped<ISignatureVerificationService, SignatureVerificationService>();

//Register the IpValidationService
builder.Services.AddScoped<IIpValidationService, IpValidationService>();

//Register the ITokenService
builder.Services.AddScoped<ITokenService, TokenService>();

// Register the ILocalCodeTableService
builder.Services.AddScoped<ILocalCodeTableService, LocalCodeTableService>();

// Register the ILocalCountryInfoService
builder.Services.AddScoped<ILocalCountryInfoService, LocalCountryInfoService>();

// Register the ILocalCurrencyInfoService
builder.Services.AddScoped<ILocalCurrencyInfoService, LocalCurrencyInfoService>();

// Register the DataSyncBackgroundService
builder.Services.AddHostedService<DataSyncBackgroundService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOriginWithCredentials", policy =>
    {
        policy.AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
              .SetIsOriginAllowed(origin => true); // Allow any origin
    });
});

// Configure the main database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure the Clientele database context
builder.Services.AddDbContext<KycDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("KycConnection")));

// Add health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>("DbContextHealthCheck")
    .AddDbContextCheck<KycDbContext>("ClienteleDbContextHealthCheck")
    .AddCheck<MoneyGramHealthCheck>("MoneyGramHealthCheck");

// Register HttpClient for MoneyGramHealthCheck
builder.Services.AddHttpClient<MoneyGramHealthCheck>();

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
    };
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("AllowAnyOriginWithCredentials"); // Apply CORS policy here

// Use the JWT Authentication Middleware
app.UseMiddleware<JwtAuthenticationMiddleware>(); // Add the JWT authentication middleware here

// Add LoggingMiddleware after JWT Authentication Middleware
app.UseMiddleware<LoggingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// Enable middleware to serve generated Swagger as a JSON endpoint
app.UseSwagger();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MoneyGram API v1");
        options.RoutePrefix = "swagger"; 
    });
}

app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = CustomHealthCheckResponseWriter.WriteResponse
});

app.Run();