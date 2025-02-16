using Amazon;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using stockmanagementapi;
using stockmanagementapi.Models;
using stockmanagementapi.Models.Users;
using stockmanagementapi.Services.EmailSenderServices;
using stockmanagementapi.Services.StockManagementServices;
using stockmanagementapi.Services.UserServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IStockManagementService, StockManagementService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.WebHost.ConfigureKestrel(serverOptions =>
{
  var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
  serverOptions.ListenAnyIP(int.Parse(port));
});

var awsConfig = builder.Configuration.GetAWSOptions();

if (builder.Environment.IsDevelopment())
{
  awsConfig.Credentials = new EnvironmentVariablesAWSCredentials();
}
else
{
  awsConfig.Credentials = new InstanceProfileAWSCredentials();
}
awsConfig.Region = RegionEndpoint.EUNorth1;

builder.Services.AddDefaultAWSOptions(awsConfig);
builder.Services.AddAWSService<IAmazonSecretsManager>();

try
{
  var secretsManager = builder.Services.BuildServiceProvider().GetRequiredService<IAmazonSecretsManager>();
  string secretName = "DBSecrets";

  var connectionStringTask = await GetConnectionStringFromSecretsManagerAsync(secretsManager, secretName);
  string connectionString = connectionStringTask;

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

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowSpecificOrigin",
      builder => builder
          .WithOrigins("https://d6ckbuomlwjrt.cloudfront.net", "http://d6ckbuomlwjrt.cloudfront.net")
          .AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials());
});

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ModelDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IEmailSender<User>, EmailSender>();

builder.Services.ConfigureApplicationCookie(options =>
{
  options.Cookie.HttpOnly = true;
  options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
  options.Cookie.SameSite = SameSiteMode.None;
  options.Cookie.Name = ".AspNetCore.Identity.Application";
  options.LoginPath = "/api/login";
  options.LogoutPath = "/api/logout";
  options.AccessDeniedPath = "/api/access-denied";

  if (builder.Environment.IsDevelopment())
  {
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    options.Cookie.SameSite = SameSiteMode.Lax;
  }
  else
  {
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
  }
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();

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

async Task<string> GetConnectionStringFromSecretsManagerAsync(IAmazonSecretsManager secretsManager, string secretName)
{
  try
  {
    var request = new GetSecretValueRequest
    {
      SecretId = secretName,
      VersionStage = "AWSCURRENT"
    };

    var response = await secretsManager.GetSecretValueAsync(request);

    string secretJson = response.SecretString;
    try
    {
      var secretDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(secretJson);

      string connectionString = $"Server={secretDictionary["host"]},{secretDictionary["port"]};Database=stockapp-db;User Id={secretDictionary["username"]};Password={secretDictionary["password"]};TrustServerCertificate=True;";

      return connectionString;
    }
    catch (JsonReaderException jsonEx)
    {
      throw new Exception($"Error parsing JSON: {jsonEx.Message}", jsonEx);
    }
    catch (KeyNotFoundException keyEx)
    {
      throw new Exception($"Missing key in Secrets Manager JSON: {keyEx.Message}", keyEx);
    }

  }
  catch (Exception ex)
  {
    Console.WriteLine($"Error getting secret from Secrets Manager: {ex.Message}");
    throw;
  }
}