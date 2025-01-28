using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using stockmanagementapi.Models.StockItems.Command;
using stockmanagementapi.Models.StockItems.Lookup;
using stockmanagementapi.Services;

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
    public Task<List<StockItemLookup>> GetStockItemsAsync()
    {
      return service.GetStockItemsAsync();
    }

    [HttpGet("stock-item")]
    public Task<StockItemLookup> GetStockItemAsync()
    {
      return service.GetStockItemAsync();
    }

    [HttpPost("update-stock-item")]
    public Task<IResult> UpdateStockItemAsync([FromBody] StockItemCommand stockItemCommand)
    {
      return service.UpdateStockItemAsync(stockItemCommand);
    }

    [HttpPost("create-stock-item")]
    public Task<IResult> CreateStockItemAsync([FromBody] StockItemCommand item)
    {
      return service.CreateStockItemAsync(item);
    }

    [HttpDelete("delete-stock-item/{id}")]
    public Task<IResult> DeleteStockItemAsync([FromQuery] int id)
    {
      return service.DeleteStockItemAsync(id);
    }
  }
}
