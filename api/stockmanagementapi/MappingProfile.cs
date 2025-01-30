using AutoMapper;
using stockmanagementapi.Models.StockItemImages;
using stockmanagementapi.Models.StockItemImages.Commands;
using stockmanagementapi.Models.StockItemImages.Lookups;
using stockmanagementapi.Models.StockItems;
using stockmanagementapi.Models.StockItems.Command;
using stockmanagementapi.Models.StockItems.Lookup;
using stockmanagementapi.Models.Users;

namespace stockmanagementapi
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      CreateMap<StockItemCommand, StockItem>();
      CreateMap<StockItem, StockItemLookup>();
      CreateMap<RegisterUser, User>();
      CreateMap<StockImageLookup, StockImage>();
      CreateMap<StockImageCommand, StockImage>();
      CreateMap<StockImage,StockImageLookup>();
      CreateMap<StockImage, StockImageCommand>();
		}
  }
}
