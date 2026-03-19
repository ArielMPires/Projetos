using Domus.DataBase;
using Domus.Models.DB;
using Domus.Repositories;
using Domus.Repositories.Interfaces;
using Domus.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO.Compression;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// =============== PERFORMANCE GLOBAL ===============
builder.WebHost.UseKestrel(options =>
{
    options.Limits.MaxConcurrentConnections = 1000;
    options.Limits.MaxConcurrentUpgradedConnections = 1000;
    options.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(60);
    options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(30);
});

// Compressão de resposta (gzip + brotli)
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add<BrotliCompressionProvider>();
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Warning);

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddDbContextFactory<ApplicationDbContext>((sp, options) =>
{
    var tenant = sp.GetRequiredService<TenantProvider>();
    var con = tenant.GetConnectionStrings();

    options.UseMySql(con, ServerVersion.AutoDetect(con))
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
    .EnableSensitiveDataLogging();
}, ServiceLifetime.Scoped);


builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<TenantProvider>();
builder.Services.AddSingleton<ITenantDbContextFactory, TenantDBContext>();

builder.Services.Configure<TenantConnectionStrings>(options => 
builder.Configuration.GetSection(nameof(TenantConnectionStrings)).Bind(options));

builder.Services.AddTransient<IUsers, UsersRepository>();
builder.Services.AddTransient<IMaintenance, MaintenanceRepository>();
builder.Services.AddTransient<IRequest, RequestRepository>();
builder.Services.AddTransient<IFiles, FilesRepository>();
builder.Services.AddTransient<ICheckList, CheckListRepository>();
builder.Services.AddTransient<IProducts, ProductRepository>();
builder.Services.AddTransient<INF_Input, NF_InputRepository>();
builder.Services.AddTransient<IService_Order, ServiceOrderRepository>();
builder.Services.AddTransient<IPassword, PasswordRepository>();
builder.Services.AddTransient<IPatrimony, PatrimonyRepository>();
builder.Services.AddTransient<IProvider_Order, Provider_OrderRepository>();
builder.Services.AddTransient<IProject, ProjectRepository>();
builder.Services.AddTransient<IManuals, ManualsRepository>();

builder.Services.AddControllers().AddNewtonsoftJson(x =>
 x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"])),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWTAuthAuthentication", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme",
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
                         new List<string>()
                    }
                });
});

builder.Configuration
    .AddJsonFile("Secret.json", optional: true, reloadOnChange: true);
builder.Services.Configure<AesKeyConfig>(builder.Configuration.GetSection("AES"));
builder.Services.AddSingleton<AesEncryption>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseCompression();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHub<NotificationHub>("/Domus/Notification");

app.MapControllers();

app.Run();
