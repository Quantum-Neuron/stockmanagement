namespace stockmanagementapi.Models
{
	using Amazon.SecretsManager.Model;
	using stockmanagementapi.Models.StockItems;
	using Microsoft.EntityFrameworkCore;
	using stockmanagementapi.Models.StockAccessories;
	using stockmanagementapi.Models.StockItemsAccessories;
	using stockmanagementapi.Models.Users;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using stockmanagementapi.Models.StockItemImages;
  using Amazon.SecretsManager;
  using System.Threading.Tasks;

  public class ModelDbContext : IdentityDbContext<User>
	{
		private readonly IAmazonSecretsManager amazonSecretsManager;
		private readonly IConfiguration configuration;

    public ModelDbContext(DbContextOptions<ModelDbContext> options) : base(options)
    {
    }

    public DbSet<StockItem> StockItems { get; set; }

		public DbSet<StockAccessory> StockAccessories { get; set; }

		public DbSet<StockItemAccessory> StockItemAccessories { get; set; }

		public new DbSet<User> Users { get; set; }

		public DbSet<StockImage> StockImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<StockItem>().HasQueryFilter(stockItem => !stockItem.IsDeleted);
			modelBuilder.Entity<StockAccessory>().HasQueryFilter(stockAccessory => !stockAccessory.IsDeleted);
			modelBuilder.Entity<StockItemAccessory>().HasQueryFilter(stockItemAccessory => !stockItemAccessory.StockAccessory.IsDeleted);

			modelBuilder.Entity<StockItemAccessory>()
				.HasKey(stockItemAccessory => new { stockItemAccessory.StockItemId, stockItemAccessory.StockAccessoryId });

			modelBuilder.Entity<StockItemAccessory>()
				.HasOne(sia => sia.StockItem)
				.WithMany(si => si.StockItemAccessories)
				.HasForeignKey(sia => sia.StockItemId);

			modelBuilder.Entity<StockItemAccessory>()
				.HasOne(sia => sia.StockAccessory)
				.WithMany(sa => sa.StockItemAccessories)
				.HasForeignKey(sia => sia.StockAccessoryId);

			base.OnModelCreating(modelBuilder);
		}
	}
}
