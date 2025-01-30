using Microsoft.AspNetCore.Mvc;
using stockmanagementapi.Models.StockItemImages.Commands;
using stockmanagementapi.Models.StockItemImages.Lookups;
using stockmanagementapi.Models.StockItems;
using stockmanagementapi.Models.StockItems.Command;
using stockmanagementapi.Models.StockItems.Lookup;
using stockmanagementapi.Services.StockManagementServices;

namespace stockmanagementapi.Controllers
{
	[Route("api/[controller]")]
  [ApiController]
  public class StockManagementController : ControllerBase
  {
    private readonly IStockManagementService service;

    public StockManagementController(IStockManagementService service)
    {
      this.service = service;
    }

    [HttpGet("stock-items")]
    public Task<PagedStockItems<StockItemLookup>> GetStockItemsAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
      return service.GetStockItemsAsync(pageNumber, pageSize);
    }

    [HttpGet("stock-item")]
    public Task<StockItemLookup> GetStockItemAsync()
    {
      return service.GetStockItemAsync();
    }

    [HttpPost("update-stock-item")]
    public Task<IResult> UpdateStockItemAsync([FromForm] StockItemCommand stockItemCommand)
    {
      return service.UpdateStockItemAsync(stockItemCommand);
    }

    [HttpPost("create-stock-item")]
    public Task<IResult> CreateStockItemAsync([FromBody] StockItemCommand item)
    {
      return service.CreateStockItemAsync(item);
    }

    [HttpDelete("delete-stock-item/{id}")]
    public Task<IResult> DeleteStockItemAsync([FromRoute] int id)
    {
      return service.DeleteStockItemAsync(id);
    }

    [HttpGet("stock-images/{id}")]
    public Task<List<StockImageLookup>> GetStockImages([FromRoute] int stockItemId)
    {
      return this.service.GetStockImagesAsync(stockItemId);
		}

    [HttpPost("add-stock-images")]
    public Task<List<StockImageLookup>> AddStockImages([FromBody] List<StockImageCommand> stockImageCommands)
		{
			return this.service.AddStockImagesAsync(stockImageCommands);
		}

    [HttpDelete("delete-stock-image/{id}")]
    public Task<IResult> DeleteStockImage([FromQuery] int id)
    {
			return this.service.DeleteStockImageAsync(id);
		}

    [HttpPost("update-stock-image")]
    public Task<StockImageLookup> UpdateStockImage([FromBody] StockImageCommand stockImageCommand)
    {
      return this.service.UpdateStockImageAsync(stockImageCommand);
    }
	}
}
