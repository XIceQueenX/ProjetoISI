using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Trabalho_ISI.Data;
using Trabalho_ISI.Services.Interfaces;
using Trabalho_ISI.Services;

var builder = WebApplication.CreateBuilder(args);

#region Read Keys from Configuration / Environment Variables

var googleBooksApiKey = builder.Configuration["ExternalAPIs:GoogleBooks:ApiKey"]
                        ?? throw new Exception("Google Books API key not set!");
var googleBooksBaseUrl = builder.Configuration["ExternalAPIs:GoogleBooks:BaseUrl"]
                         ?? "https://www.googleapis.com/books/v1";

var tmdbApiKey = builder.Configuration["ExternalAPIs:TMDB:ApiKey"]
                 ?? "";
var tmdbBaseUrl = builder.Configuration["ExternalAPIs:TMDB:BaseUrl"]
                  ?? "https://api.themoviedb.org/3";

var jwtKey = builder.Configuration["Jwt:Key"]
             ?? "dhas6d8asdbsdh8dsaidas8";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "TrabalhoISI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "TrabalhoISIUsers";
var jwtExpires = int.TryParse(builder.Configuration["Jwt:ExpiresInMinutes"], out var exp) ? exp : 60;

#endregion

#region AppSettings Object

var appSettings = new AppSettings
{
    Jwt = new JwtSettings
    {
        Key = jwtKey,
        Issuer = jwtIssuer,
        Audience = jwtAudience,
        ExpiresInMinutes = 60
    },
    TMDB = new TmdbSettings
    {
        ApiKey = tmdbApiKey,
        BaseUrl = tmdbBaseUrl
    },
    GoogleBooks = new GoogleBooksSettings
    {
        ApiKey = googleBooksApiKey,
        BaseUrl = googleBooksBaseUrl
    }
};

builder.Services.AddSingleton(appSettings);

#endregion

#region Services Configuration

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        )
    )
);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IDataSeederService, DataSeederService>();

#endregion

#region JWT Authentication

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

#endregion

#region Swagger Configuration

var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ISI API",
        Version = "v1",
        Description = "Trabalho Lufer - Gloria Martins e Paula Canuto"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

#endregion

var app = builder.Build();

#region Middleware Pipeline

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ISI API v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
