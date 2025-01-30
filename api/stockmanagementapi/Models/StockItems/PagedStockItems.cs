namespace stockmanagementapi.Models.StockItems
{
	public class PagedStockItems<T>
	{
		public List<T> Items { get; set; }
		
		public int TotalItems { get; set; }

		public int PageNumber { get; set; }

		public int PageSize { get; set; }
	}
}
