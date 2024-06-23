
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SuccessPointCore.Application.Interfaces;
using SuccessPointCore.Application.Services;
using SucessPointCore.Api.Filters;
using SucessPointCore.Domain.Helpers;
using SucessPointCore.Infrastructure;
using SucessPointCore.Infrastructure.Interfaces;
using SucessPointCore.Infrastructure.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

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
                        // check if the endpoint has the authorize attribute
                        var authorizeattributes = endpoint.Metadata
                            .Where(m => m is Microsoft.AspNetCore.Authorization.AuthorizeAttribute)
                            .ToList();

                        if (authorizeattributes.Any())
                        {
                            var header_token = context.HttpContext.Request.Headers["authorization"].FirstOrDefault()?.Split(" ").Last();

                            if (header_token == null)
                            {
                                return Un_AuthorizedResponse(context);
                            }



                            if (!ValidateToken(header_token))
                            {
                                return Un_AuthorizedResponse(context);
                            }
                        }
                    }

                    return Task.CompletedTask;
                },
                //OnAuthenticationFailed = context =>
                //{
                //    context.NoResult();
                //    context.Response.StatusCode = 500;
                //    context.Response.ContentType = "text/plain";
                //    return context.Response.WriteAsync(context.Exception.ToString());
                //},
                //OnTokenValidated = context =>
                //{
                //    // log additional details here if needed
                //    return Task.CompletedTask;
                //},
                //OnChallenge = context =>
                //{
                //    context.HandleResponse();
                //    context.Response.StatusCode = 401;
                //    context.Response.ContentType = "application/json";
                //    var result = JsonConvert.SerializeObject(new { error = "you are not authorized" });
                //    return context.Response.WriteAsync(result);
                //}
            };
        });

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddScoped<IErrorLogRepository, ErrorLogRepository>();
builder.Services.AddScoped<IErrorLogService, ErrorLogService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();


var app = builder.Build();

SetAppConfigHelperValues();
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


//AppConfigHelper.PasswordEncyptionKey =builder.Configuration["SecurityKeys:PasswordEncryptionKey"]);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization();  // Add authorization middleware
//app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

void CreateDefault()
{
    // Note I had single hosting with limited database capacity.
    // hence I thought to keep table with SP abbrivation to identify for which project this table refers to.
    // here sp_SP means small sp (store procedure abbrivation) and capital (SP) is for SuccessPoint project database.
    new DbSchemaUpdate(AppConfigHelper.ConnectionString, new ErrorLogRepository()).CreateDefaults();
}

void SetAppConfigHelperValues() {

    AppConfigHelper.ConnectionString = app.Configuration.GetConnectionString("SucessPointString");
    AppConfigHelper.PasswordEncyptionKey = app.Configuration["SecurityKeys:PasswordEncryptionKey"];
    AppConfigHelper.JWTTokenEncryptionKey = app.Configuration["SecurityKeys:JWTTokenEncryptionKey"];
    AppConfigHelper.Issuer = app.Configuration["SecurityKeys:Issuer"];
    AppConfigHelper.Audience = app.Configuration["SecurityKeys:Audience"];
    AppConfigHelper.TokenExpiryMinute = Convert.ToInt32(app.Configuration["SecurityKeys:TokenExpiryMinutes"]);
    AppConfigHelper.RefreshTokenExpiryMinute = Convert.ToInt32(app.Configuration["SecurityKeys:RefreshTokenExpiryMinutes"]);

    // SMTP Email Configuration details
    AppConfigHelper.SMTPURL = app.Configuration["VerificationMail:SMTP_Domain"];
    AppConfigHelper.SMTPPORT = app.Configuration["VerificationMail:SMTP_PORT"];

    //Email Account Credentials for sending email by UserSignupVerification, ForgetPasswordVerification, DeleteAccountVerification.
    AppConfigHelper.SignupEmailCredentials = app.Configuration["VerificationMail:SignupEmailCredentials"];
    AppConfigHelper.ForgetEmailCredentials = app.Configuration["VerificationMail:ForgetPasswordEmailCredentials"];
    AppConfigHelper.DeleteAccountEmailCredentials = app.Configuration["VerificationMail:DeleteAccountEmailCredentials"];

    AppConfigHelper.VerificationExpiryMinute = Convert.ToInt32(app.Configuration["VerificationMail:VerificationExpiryMinute"]);
}

app.Run();

static Task Un_AuthorizedResponse(MessageReceivedContext context)
{
    context.NoResult();
    context.Response.StatusCode = 401;
    context.Response.ContentType = "text/plain";
    return context.Response.WriteAsync("Un-Authorized request received");
}

bool ValidateToken(string token)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.UTF8.GetBytes(builder.Configuration["SecurityKeys:JWTTokenEncryptionKey"]); // Change here

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