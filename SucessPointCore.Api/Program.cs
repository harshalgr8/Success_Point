using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SuccessPointCore.Application.Interfaces;
using SuccessPointCore.Application.Services;
using SucessPointCore.Api.Filters;
using SucessPointCore.Domain.Helpers;
using SucessPointCore.Infrastructure;
using SucessPointCore.Infrastructure.Interfaces;
using SucessPointCore.Infrastructure.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policyBuilder =>
        {
            policyBuilder.WithOrigins("https://sp.premiersolution.in", "https://premiersolution.in")
                         .AllowAnyHeader()
                         .AllowAnyMethod()
                         .AllowCredentials();
        });
});

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Success Point Api",
        Description = "Api for accessing Success point learning videos if having paid subscription.",
        Contact = new OpenApiContact
        {
            Name = "Harshal Dawange",
            Email = "harshal.dawange@gmail.com",
            Url = new Uri("https://www.PremierSolution.in"),
        }
    });

    c.OperationFilter<SwaggerIgnoreFilter>();

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
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
            new string[] { }
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["SecurityKeys:Issuer"],
            ValidAudience = builder.Configuration["SecurityKeys:Audience"],
            RequireExpirationTime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecurityKeys:JWTTokenEncryptionKey"]))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var endpoint = context.HttpContext.GetEndpoint();
                if (endpoint != null)
                {
                    var authorizeAttributes = endpoint.Metadata
                        .Where(m => m is Microsoft.AspNetCore.Authorization.AuthorizeAttribute)
                        .ToList();

                    if (authorizeAttributes.Any())
                    {
                        var headerToken = context.HttpContext.Request.Headers["authorization"].FirstOrDefault()?.Split(" ").Last();
                        if (headerToken == null)
                        {
                            return UnAuthorizedResponse(context);
                        }

                        if (!ValidateToken(headerToken))
                        {
                            return UnAuthorizedResponse(context);
                        }
                    }
                }

                return Task.CompletedTask;
            }
        };
    });

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Register your Repositories
builder.Services.AddScoped<IErrorLogRepository, ErrorLogRepository>();
builder.Services.AddScoped<IErrorLogService, ErrorLogService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStandardRepository, StandardRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

// Register your Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IStandardService, StandardService>();

// Build the application
var app = builder.Build();

// Store AppConfig Values
SetAppConfigHelperValues();

// Create Auto Tables/Procedure/Function/etc/etc
CreateDefault();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowSpecificOrigins"); // Add CORS middleware here

app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization();  // Add authorization middleware

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void CreateDefault()
{
    new DbSchemaUpdate(AppConfigHelper.ConnectionString, new ErrorLogRepository()).CreateDefaults();
}

void SetAppConfigHelperValues()
{
    AppConfigHelper.ConnectionString = app.Configuration.GetConnectionString("SucessPointString");
    AppConfigHelper.PasswordEncyptionKey = app.Configuration["SecurityKeys:PasswordEncryptionKey"];
    AppConfigHelper.JWTTokenEncryptionKey = app.Configuration["SecurityKeys:JWTTokenEncryptionKey"];
    AppConfigHelper.Issuer = app.Configuration["SecurityKeys:Issuer"];
    AppConfigHelper.Audience = app.Configuration["SecurityKeys:Audience"];
    AppConfigHelper.TokenExpiryMinute = Convert.ToInt32(app.Configuration["SecurityKeys:TokenExpiryMinutes"]);
    AppConfigHelper.RefreshTokenExpiryMinute = Convert.ToInt32(app.Configuration["SecurityKeys:RefreshTokenExpiryMinutes"]);
    AppConfigHelper.SMTPURL = app.Configuration["VerificationMail:SMTP_Domain"];
    AppConfigHelper.SMTPPORT = app.Configuration["VerificationMail:SMTP_PORT"];
    AppConfigHelper.SignupEmailCredentials = app.Configuration["VerificationMail:SignupEmailCredentials"];
    AppConfigHelper.ForgetEmailCredentials = app.Configuration["VerificationMail:ForgetPasswordEmailCredentials"];
    AppConfigHelper.DeleteAccountEmailCredentials = app.Configuration["VerificationMail:DeleteAccountEmailCredentials"];
    AppConfigHelper.VerificationExpiryMinute = Convert.ToInt32(app.Configuration["VerificationMail:VerificationExpiryMinute"]);
}

static Task UnAuthorizedResponse(MessageReceivedContext context)
{
    context.NoResult();
    context.Response.StatusCode = 401;
    context.Response.ContentType = "text/plain";
    return context.Response.WriteAsync("Un-Authorized request received");
}

bool ValidateToken(string token)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.UTF8.GetBytes(builder.Configuration["SecurityKeys:JWTTokenEncryptionKey"]);

    try
    {
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["SecurityKeys:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["SecurityKeys:Audience"],
            ValidateLifetime = true,
        }, out SecurityToken validatedToken);

        return true;
    }
    catch (SecurityTokenException ex)
    {
        // Log the exception for debugging
        Console.WriteLine($"Token validation failed: {ex.Message}");
        return false;
    }
    catch (Exception ex)
    {
        // Log other exceptions
        Console.WriteLine($"An error occurred: {ex.Message}");
        return false;
    }
}
