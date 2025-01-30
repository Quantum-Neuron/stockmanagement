using System.ComponentModel.DataAnnotations;

namespace stockmanagementapi.Models.StockItemImages
{
	public class StockImage
	{
		public int StockImageId { get; set; }

		/// <summary>
		/// Gets or sets the image name.
		/// </summary>
		[StringLength(100)]
		public required string Name { get; set; }

		/// <summary>
		/// Gets or sets the image binary.
		/// </summary>
		public required byte[] ImageBinary { get; set; }

		/// <summary>
		/// Gets or sets the stock item id.
		/// </summary>
		public int StockItemId { get; set; }
	}
}
