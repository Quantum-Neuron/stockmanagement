using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stockmanagementapi.Models;
using stockmanagementapi.Models.StockItemImages;
using stockmanagementapi.Models.StockItemImages.Commands;
using stockmanagementapi.Models.StockItemImages.Lookups;
using stockmanagementapi.Models.StockItems;
using stockmanagementapi.Models.StockItems.Command;
using stockmanagementapi.Models.StockItems.Lookup;
using stockmanagementapi.Models.StockItemsAccessories;
using System.Linq.Dynamic.Core;

namespace stockmanagementapi.Services.StockManagementServices
{
	public class StockManagementService : IStockManagementService
	{
		private readonly ModelDbContext dbContext;
		private readonly IMapper mapper;
		private readonly ILogger<StockManagementService> logger;

		public StockManagementService(
			ModelDbContext dbContext,
			IMapper mapper,
			ILogger<StockManagementService> logger
		)
		{
			this.dbContext = dbContext;
			this.mapper = mapper;
			this.logger = logger;
		}

		public async Task<IResult> CreateStockItemAsync(StockItemCommand item)
		{
			try
			{
				logger.LogInformation("Creating stock item.");

				var mappedStockItem = mapper.Map<StockItemCommand, StockItem>(item);

				mappedStockItem.CreationDate = DateTime.Now;
				mappedStockItem.LastModifiedDate = DateTime.Now;

				await dbContext.StockItems.AddAsync(mappedStockItem).ConfigureAwait(false);
				await dbContext.SaveChangesAsync().ConfigureAwait(false);

				foreach (var sia in item.StockItemAccessories)
				{
					var exists = await dbContext.StockItemAccessories
							.AnyAsync(s => s.StockItemId == mappedStockItem.StockItemId && s.StockAccessoryId == sia.StockAccessoryId)
							.ConfigureAwait(false);

					if (!exists)
					{
						var stockItemAccessory = new StockItemAccessory
						{
							StockItemId = mappedStockItem.StockItemId,
							StockAccessoryId = sia.StockAccessoryId
						};
						await dbContext.StockItemAccessories.AddAsync(stockItemAccessory).ConfigureAwait(false);
					}
				}

				await dbContext.SaveChangesAsync().ConfigureAwait(false);
				return Results.Ok("Successfully created stock item.");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error creating stock item");
				return Results.BadRequest("Error creating stock item");
			}
		}

		public async Task<IResult> DeleteStockItemAsync(int id)
		{
			try
			{
				logger.LogInformation("Deleting stock item.");

				var stockItem = await dbContext.StockItems.FirstOrDefaultAsync(
					st => st.StockItemId == id
				) ?? throw new InvalidOperationException("Stock item not found.");

				stockItem.IsDeleted = true;

				dbContext.StockItems.Update(stockItem);
				await dbContext.SaveChangesAsync().ConfigureAwait(false);

				return Results.Ok("Successfully deleted stock item.");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error deleting stock item");
				return Results.BadRequest("Error deleting stock item.");
			}
		}

		public Task<StockItemLookup> GetStockItemAsync()
		{
			throw new NotImplementedException();
		}

		public async Task<PagedStockItems<StockItemLookup>> GetStockItemsAsync(int pageNumber, int pageSize)
		{
			try
			{
				var totalItems = await dbContext.StockItems.CountAsync().ConfigureAwait(false);
				var stockItems = await dbContext.StockItems
						.Skip((pageNumber - 1) * pageSize)
						.Take(pageSize)
						.ToListAsync()
						.ConfigureAwait(false);

				var mappedStockItems = mapper.Map<List<StockItemLookup>>(stockItems);

				return new PagedStockItems<StockItemLookup>
				{
					Items = mappedStockItems,
					TotalItems = totalItems,
					PageNumber = pageNumber,
					PageSize = pageSize
				};
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error getting stock items");
				throw new InvalidOperationException("Failed to get stock items.");
			}
		}

		public async Task<IResult> UpdateStockItemAsync(StockItemCommand stockItemCommand)
		{
			try
			{
				var stockItem = await dbContext.StockItems.FirstOrDefaultAsync(
					st => st.StockItemId == stockItemCommand.StockItemId
				) ?? throw new InvalidOperationException("Stock item not found.");

				var mappedStockItems = mapper.Map(stockItemCommand, stockItem);

				mappedStockItems.LastModifiedDate = DateTime.Now;

				dbContext.StockItems.Update(mappedStockItems);
				await dbContext.SaveChangesAsync().ConfigureAwait(false);

				var existingAccessories = dbContext.StockItemAccessories.Where(sia => sia.StockItemId == mappedStockItems.StockItemId);
				dbContext.StockItemAccessories.RemoveRange(existingAccessories);
				await dbContext.SaveChangesAsync().ConfigureAwait(false);

				foreach (var accessory in stockItemCommand.StockItemAccessories)
				{
					var exists = await dbContext.StockItemAccessories
							.AnyAsync(s => s.StockItemId == mappedStockItems.StockItemId && s.StockAccessoryId == accessory.StockAccessoryId)
							.ConfigureAwait(false);

					if (!exists)
					{
						var stockItemAccessory = new StockItemAccessory
						{
							StockItemId = mappedStockItems.StockItemId,
							StockAccessoryId = accessory.StockAccessoryId
						};
						dbContext.StockItemAccessories.Add(stockItemAccessory);
					}
				}

				await dbContext.SaveChangesAsync().ConfigureAwait(false);
				return Results.Ok("Successfully updated stock item.");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error updating stock item");
				return Results.BadRequest("Error updating stock item.");
			}
		}
		public async Task<List<StockImageLookup>> AddStockImagesAsync(List<StockImageCommand> commands)
		{
			try
			{
				var stockImages = new List<StockImage>();

				foreach (var command in commands)
				{
					var stockImage = new StockImage
					{
						Name = command.Name,
						ImageBinary = await ConvertToByteArrayAsync(command.ImageBinary).ConfigureAwait(false),
						StockItemId = command.StockItemId,
					};

					stockImages.Add(stockImage);
				}

				await dbContext.StockImages.AddRangeAsync(stockImages).ConfigureAwait(false);
				await dbContext.SaveChangesAsync().ConfigureAwait(false);

				return mapper.Map<List<StockImageLookup>>(stockImages);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error adding stock images");
				throw new InvalidOperationException("Failed to add stock images.");
			}
		}

		private async Task<byte[]> ConvertToByteArrayAsync(IFormFile file)
		{
			using (var memoryStream = new MemoryStream())
			{
				await file.CopyToAsync(memoryStream).ConfigureAwait(false);
				return memoryStream.ToArray();
			}
		}

		public async Task<IResult> DeleteStockImageAsync(int id)
		{
			try
			{
				var existingStockImage = await dbContext.StockImages
					.FirstOrDefaultAsync(s => s.StockImageId == id)
					.ConfigureAwait(false)
					?? throw new InvalidOperationException("Failed to find stock image.");

				dbContext.StockImages.Remove(existingStockImage);

				await dbContext.SaveChangesAsync().ConfigureAwait(false);

				return Results.Ok("Successfully deleted the stock image");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error deleting stock image");
				return Results.BadRequest("Error deleting stock image.");	
			}
		}

		public async Task<List<StockImageLookup>> GetStockImagesAsync(int stockItemId)
		{
			try
			{
				var stockImages = await dbContext.StockImages
					.Where(si => si.StockItemId == stockItemId)
					.ToListAsync()
					.ConfigureAwait(false);

				return mapper.Map<List<StockImageLookup>>(stockImages);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Failed to get stock images");
				throw new InvalidOperationException("Failed to get stock images.");
			}
		}

		public async Task<StockImageLookup> UpdateStockImageAsync([FromBody] StockImageCommand stockImageCommand)
		{
			try
			{
				var stockImage = await dbContext.StockImages
					.FirstOrDefaultAsync(si => si.StockImageId == stockImageCommand.StockImageId)
					.ConfigureAwait(false)
					?? throw new InvalidOperationException("Failed to find stock image.");

				var mappedStockImage = mapper.Map(stockImageCommand, stockImage);

				stockImage = mappedStockImage;

				dbContext.StockImages.Update(stockImage);
				await dbContext.SaveChangesAsync().ConfigureAwait(false);

				return mapper.Map<StockImageLookup>(stockImage);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error updating stock image");
				throw new InvalidOperationException("Failed to update stock image.");
			}
		}
	}
}
