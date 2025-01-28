using AutoMapper;
using stockmanagementapi.Models.StockItems;
using stockmanagementapi.Models.StockItems.Command;
using stockmanagementapi.Models.StockItems.Lookup;

namespace stockmanagementapi
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      CreateMap<StockItemCommand, StockItem>();
      CreateMap<StockItem, StockItemLookup>();
    }
  }
}
