using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using stockmanagementapi;
using stockmanagementapi.Models;
using stockmanagementapi.Models.Users;
using stockmanagementapi.Services.EmailSenderServices;
using stockmanagementapi.Services.StockManagementServices;
using stockmanagementapi.Services.UserServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Read the connection string from environment variables
builder.Services.AddDbContext<ModelDbContext>(options =>
		options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
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
					.WithOrigins("http://localhost:4200")
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

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
