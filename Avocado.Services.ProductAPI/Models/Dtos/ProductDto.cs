using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.Services.ProductAPI.Models.Dtos
{
	public class ProductDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }
		public string ImageUrl { get; set; }
		public string CategoryName { get; set; }
		public string Description { get; set; }
		[Range(1, 10)]
		public int Count { get; set; } = 1;
	}
}
