namespace stockmanagementapi.Models.StockItemImages.Lookups
{
	public class StockImageLookup
	{
		public int StockImageId { get; set; }

		public required string Name { get; set; }

		public required byte[] ImageBinary { get; set; }
	}
}
