using Avocado.Services.ProductAPI.DbContexts;
using Avocado.Services.ProductAPI.Mapping;
using Avocado.Services.ProductAPI.Models;
using Avocado.Services.ProductAPI.Models.Dtos;
using Avocado.Services.ProductAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.Services.ProductAPI.Repository
{
	public class ProductRepository : IProductRepository
	{
		private readonly ApplicationDbContext _db;
		private readonly IMapper _mapper;
		public ProductRepository(ApplicationDbContext db, IMapper mapper)
		{
			_db = db;
			_mapper = mapper;
		}

		public async Task<bool> Create(ProductDto productDto)
		{
			var product = _mapper.Map<Product>(productDto);
			await _db.Products.AddAsync(product);
			if (await _db.SaveChangesAsync() > 0)
				return true;
			else
				return false;
		}

		public async Task<int> CreateAsync(ProductDto productDto)
		{
			var product = _mapper.Map<Product>(productDto);
			await _db.Products.AddAsync(product);
			await _db.SaveChangesAsync();
			return product.Id;
		}

		public async Task<bool> Delete(int productId)
		{
			var prodToDelete = await _db.Products.FirstOrDefaultAsync(x => x.Id == productId);
			if (prodToDelete != null)
			{
				 _db.Products.Remove(prodToDelete);
				await _db.SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task<ProductDto> Get(int productId)
		{
			var productFromDb = await _db.Products.FirstOrDefaultAsync(x => x.Id == productId);
			if (productFromDb != null)
			{
				return _mapper.Map<ProductDto>(productFromDb);
			}
			return null;
		}

		public async Task<IEnumerable<ProductDto>> Get()
		{
			var productsFromDb = await _db.Products.ToListAsync();
			if (productsFromDb != null)
			{
				return _mapper.Map<List<ProductDto>>(productsFromDb);
			}
			return null;
		}

		public async Task<bool> Update(ProductDto productDto)
		{
			var productFromDb = await _db.Products.FirstOrDefaultAsync(x => x.Id == productDto.Id);
			if (productFromDb != null)
			{
				if (productDto.ImageUrl != null)
				{
					productFromDb.ImageUrl = productDto.ImageUrl;
				}
				productFromDb.Name = productFromDb.Name;
				productFromDb.Description = productDto.Description;
				productFromDb.CategoryName = productDto.CategoryName;
				productFromDb.Price = productDto.Price;
				await _db.SaveChangesAsync();
				return true;
			}
			return false;
		}
	}
}
