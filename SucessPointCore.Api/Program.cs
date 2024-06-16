
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SuccessPointCore.Application.Interfaces;
using SuccessPointCore.Application.Services;
using SucessPointCore.Api.Filters;
using SucessPointCore.Api.Helpers;
using SucessPointCore.Api.Middlewares;
using SucessPointCore.Domain.Helpers;
using SucessPointCore.Infrastructure.Interfaces;
using SucessPointCore.Infrastructure.Repositories;
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

    // Define the security scheme for JWT
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    };



    c.OperationFilter<SwaggerIgnoreFilter>();

    // on top icon to enter credentials
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Add the security requirement to operations that require authorization
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecurityKeys:JWTTokenEncryptionKey"]))
            };
        });

builder.Services.AddScoped<IErrorLogRepository, ErrorLogRepository>();
builder.Services.AddScoped<IErrorLogService, ErrorLogService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();


AppConfigHelper.ConnectionString = app.Configuration.GetConnectionString("SucessPointString");
AppConfigHelper.PasswordEncyptionKey = app.Configuration["SecurityKeys:PasswordEncryptionKey"];
AppConfigHelper.JWTTokenEncryptionKey = app.Configuration["SecurityKeys:JWTTokenEncryptionKey"];
AppConfigHelper.Issuer = app.Configuration["SecurityKeys:Issuer"];
AppConfigHelper.Audience = app.Configuration["SecurityKeys:Audience"];



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
    var initiateSqlDbScript = new DbSchemaUpdate(AppConfigHelper.ConnectionString);

    // create ErrorLog table
    initiateSqlDbScript.CreateTable_sp_errorlog();

    // create User table
    initiateSqlDbScript.CreateTable_Sp_Users();

    // create Standard table
    initiateSqlDbScript.CreateTable_SP_Standard();

    // create Course table
    initiateSqlDbScript.CreateTable_sp_course();

    // create Course Videos table
    initiateSqlDbScript.CreateTable_sp_coursevideos();
}
app.Run();
