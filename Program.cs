using FMSBay.DBContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using System.Text;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Swashbuckle.AspNetCore.SwaggerUI;
using FMSBay.Controllers;
using FMSBay.DAL;
using FMSBay.BAL;
using FMSBay.Utilites;
using FMSBay.BAL.IService;
using FMSBay.BAL.Service;
using FMSBay.DAL.IRepo;
using FMSBay.DAL.Repo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Database connection service
builder.Services.AddDbContext<MyDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionString")), ServiceLifetime.Transient);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Other management services
//builder.Services.AddTransient<INewsMgmtRepo, NewsMgmtRepo>();

// Add memory cache
builder.Services.AddMemoryCache();

// Handling JSON null values
builder.Services.Configure<JsonOptions>(options =>
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull);

// Configure form options
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

// Add CORS policy
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

// JWT Token services
builder.Services.AddScoped<ITokenService, TokenService>();

// Register UserMgmtRepo and UserMgmtService as Scoped
builder.Services.AddScoped<IUserMGMTService, UserMGMTService>();
builder.Services.AddScoped<IContactFrmService, ContactFrmService>();
builder.Services.AddScoped<IUserMGMT, UserMGMT>();
 



//swagger

//builder.Services.AddSwaggerGen();.
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Trucks API Services",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddEndpointsApiExplorer();

// handling nulls in json
builder.Services.Configure<JsonOptions>(options =>
         options.SerializerOptions.DefaultIgnoreCondition
   = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull);

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
#region jwttoken settings
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
builder.Services
     .AddAuthentication(options =>
     {
         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
         options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
     });
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
            ValidAudience = builder.Configuration["Jwt:ValidAudience"],
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        };
    });
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trucks WEB API");
    c.DocumentTitle = "CRM API";
    c.DocExpansion(DocExpansion.List);
});
app.UseStaticFiles();//root folders
// Configure the HTTP request pipeline.
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("corsapp");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.MapControllers();

app.Run();