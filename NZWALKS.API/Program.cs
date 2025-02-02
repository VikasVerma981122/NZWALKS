using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NZWALKS.API.Data;
using NZWALKS.API.Mappings;
using NZWALKS.API.Repositories;
using Serilog;
using System.Security.Cryptography.Xml;
using System.Text;
using NZWALKS.API.Middlewares;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/NZWalks_log.txt", rollingInterval: RollingInterval.Minute)
    //.MinimumLevel.Information() it shows all informations of logs..
    .MinimumLevel.Warning()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{ 
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "NZ Walks API", Version = "V1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    { 
      Name = "Authorization",
      In = ParameterLocation.Header,
      Type =SecuritySchemeType.ApiKey,
      Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement 
    {
        {
            new OpenApiSecurityScheme
            {
             Reference = new OpenApiReference
             {
                 Type = ReferenceType.SecurityScheme,
                 Id = JwtBearerDefaults.AuthenticationScheme
             },
                 Scheme = "Oauth2",
                 Name = JwtBearerDefaults.AuthenticationScheme,
                 In = ParameterLocation.Header  
             },
            new List<String>()
        }
    });
});
builder.Services.AddDbContext<NZWalkDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalkConnectionString")));
builder.Services.AddDbContext<NZWalkAuthDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalkAuthConnectionString")));
builder.Services.AddScoped<IRegionRepository,SqlRegionRepository>();
builder.Services.AddScoped<IWalkRepository, SqlWalkRepository>();
builder.Services.AddScoped<ITokenRepository,TokenRepository>();
builder.Services.AddScoped<IImageRepository,LocalImageRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
    .AddEntityFrameworkStores<NZWalkAuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{ 
  options.Password.RequireDigit = false;
  options.Password.RequireLowercase = false;
  options.Password.RequireNonAlphanumeric = false;
  options.Password.RequireUppercase = false;
  options.Password.RequiredLength = 6;
  options.Password.RequiredUniqueChars = 1;
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
}) ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<NZWALKS.API.Middlewares.ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"Images")),
    RequestPath = "/Images"
});

app.MapControllers();

app.Run();
