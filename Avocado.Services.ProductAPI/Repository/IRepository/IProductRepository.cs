using Avocado.Services.ProductAPI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.Services.ProductAPI.Repository.IRepository
{
	public interface IProductRepository
	{
		Task<ProductDto> Get(int productId);
		Task<IEnumerable<ProductDto>> Get();
		Task<bool> Create(ProductDto productDto);
		Task<bool> Update(ProductDto productDto);
		Task<bool> Delete(int productId);
	}
}
