using Microsoft.EntityFrameworkCore;
using stockmanagementapi;
using stockmanagementapi.Models;
using stockmanagementapi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Read the connection string from environment variables
builder.Services.AddDbContext<ModelDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IStockManagementService, StockManagementService>();

builder.Services.AddCors(options =>
{
  options.AddPolicy("MyCors", builder =>
  {
    builder.WithOrigins("http://localhost:4200")
    .AllowAnyMethod()
    .AllowAnyHeader();
  });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyPolicy");

app.UseHttpsRedirection();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
  var dbContext = scope.ServiceProvider.GetRequiredService<ModelDbContext>();
  await GenerateSeedData.SeedAsync(dbContext).ConfigureAwait(false);
}

app.Run();
