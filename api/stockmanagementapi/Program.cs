using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using stockmanagementapi;
using stockmanagementapi.Models;
using stockmanagementapi.Models.Users;
using stockmanagementapi.Services.EmailSenderServices;
using stockmanagementapi.Services.StockManagementServices;
using stockmanagementapi.Services.UserServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddAWSService<IAmazonSecretsManager>();
var secretsManager = builder.Services.BuildServiceProvider().GetRequiredService<IAmazonSecretsManager>();
var configuration = builder.Configuration;

try
{
  var connectionStringTask = GetConnectionStringFromSecretsManagerAsync(secretsManager, configuration);
  connectionStringTask.Wait();
  string connectionString = connectionStringTask.Result;

  builder.Services.AddDbContext<ModelDbContext>(options =>
      options.UseSqlServer(connectionString));

  Console.WriteLine("Database connection string retrieved successfully.");
}
catch (Exception ex)
{
  Console.WriteLine($"Error retrieving database connection string: {ex.Message}");
  if (ex.InnerException != null)
  {
    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
  }
  throw; 
}
builder.Services.AddScoped<IStockManagementService, StockManagementService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAuthorization();
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ModelDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IEmailSender<User>, EmailSender>();

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowSpecificOrigin",
      builder => builder
          .WithOrigins("http://localhost:4200", "https://d6ckbuomlwjrt.cloudfront.net")
          .AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials());
});

builder.Services.ConfigureApplicationCookie(options =>
{
  options.Cookie.HttpOnly = true;
  options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
  options.Cookie.SameSite = SameSiteMode.None;
  options.Cookie.Name = ".AspNetCore.Identity.Application";
  options.LoginPath = "/api/login";
  options.LogoutPath = "/api/logout";
  options.AccessDeniedPath = "/api/access-denied";
});



builder.WebHost.ConfigureKestrel(serverOptions =>
{
  var port = Environment.GetEnvironmentVariable("PORT") ?? "80";

  serverOptions.ListenAnyIP(int.Parse(port), listenOptions =>
  {
    string certPath = Environment.GetEnvironmentVariable("CERTIFICATE_PATH");
    string certPassword = Environment.GetEnvironmentVariable("CERTIFICATE_PASSWORD");

    if (!string.IsNullOrEmpty(certPath) && !string.IsNullOrEmpty(certPassword))
    {
      try
      {
        listenOptions.UseHttps(certPath, certPassword);
        Console.WriteLine("HTTPS configured with certificate.");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error configuring HTTPS: {ex.Message}");
      }
    }
    else
    {
      Console.WriteLine("CERTIFICATE_PATH or CERTIFICATE_PASSWORD environment variables not set. Running without HTTPS.");
    }
  });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGroup("api").MapIdentityApi<User>();

using (var scope = app.Services.CreateScope())
{
  var dbContext = scope.ServiceProvider.GetRequiredService<ModelDbContext>();
  await dbContext.Database.MigrateAsync().ConfigureAwait(false);
  await GenerateSeedData.SeedAsync(dbContext).ConfigureAwait(false);
}

app.Run();

async Task<string> GetConnectionStringFromSecretsManagerAsync(IAmazonSecretsManager secretsManager, IConfiguration configuration)
{
  try
  {
    string secretName = configuration?["Secrets:DatabaseConnectionString"] ?? "DBSecret";

    var request = new GetSecretValueRequest { SecretId = secretName };
    var response = await secretsManager.GetSecretValueAsync(request);

    if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
    {
      return response.SecretString;
    }
    else
    {
      Console.WriteLine($"Error getting connection string from Secrets Manager: {response.HttpStatusCode}");
      throw new Exception("Error getting connection string from Secrets Manager");
    }
  }
  catch (Exception ex)
  {
    Console.WriteLine($"Error getting connection string from Secrets Manager: {ex.Message}");
    if (ex.InnerException != null)
    {
      Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
    }
    throw;
  }
}