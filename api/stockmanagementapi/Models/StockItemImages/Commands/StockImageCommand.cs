using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations;

namespace stockmanagementapi.Models.StockItemImages.Commands
{
	public class StockImageCommand
	{
		/// <summary>
		/// Gets or sets the primary key.
		/// </summary>
		public int StockImageId { get; set; }

		/// <summary>
		/// Gets or sets the image name.
		/// </summary>
		[StringLength(100)]
		public required string Name { get; set; }

		/// <summary>
		/// Gets or sets the image binary.
		/// </summary>
		public required IFormFile ImageBinary { get; set; }

		/// <summary>
		/// Get or sets the stock item id.
		/// </summary>
		public int StockItemId { get; set; }
	}
}
