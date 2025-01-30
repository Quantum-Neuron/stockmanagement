namespace stockmanagementapi.Models
{
	using stockmanagementapi.Models.StockItems;
	using Microsoft.EntityFrameworkCore;
	using stockmanagementapi.Models.StockAccessories;
	using stockmanagementapi.Models.StockItemsAccessories;
	using stockmanagementapi.Models.Users;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using stockmanagementapi.Models.StockItemImages;

	public class ModelDbContext : IdentityDbContext<User>
	{
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
