using Avocado.Services.ProductAPI.Dapper;
using Avocado.Services.ProductAPI.Mapping;
using Avocado.Services.ProductAPI.Models;
using Avocado.Services.ProductAPI.Models.Dtos;
using Avocado.Services.ProductAPI.Repository.IRepository;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.Services.ProductAPI.Repository
{
	public class Product_Repository : IProductRepository
	{
		public IDbConnection _connection;
		private readonly IDapperWrapper _dapperWrapper;
		private readonly IMapper _mapper;
		public Product_Repository(IConfiguration config, IDapperWrapper dapperWrapper, IMapper mapper)
		{
			this._connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
			_dapperWrapper = dapperWrapper;
			_mapper = mapper;
		}

		public async Task<bool> Create(ProductDto productDto)
		{
			string query = @"Insert into Products values (@name, @price, @image, @category, @description)";
			return await _dapperWrapper.ExecuteAsync( _connection, query, commandType: 1, new
			{
				@name = productDto.Name,
				@price = productDto.Price,
				@image = productDto.ImageUrl,
				@category = productDto.CategoryName,
				@description = productDto.Description
			}) > 0;

		}

		public async Task<int> CreateAsync(ProductDto productDto)
		{
			string query = @"Insert into Products values (@name, @price, @image, @category, @description);
							Select Id=scope_identity()";
			return await _dapperWrapper.ExecuteAsync(_connection, query, commandType: 1,
				new {
				@name = productDto.Name,
				@price = productDto.Price,
				@image = productDto.ImageUrl,
				@category = productDto.CategoryName,
				@description = productDto.Description
			});
		}

		public async Task<bool> Delete(int productId)
		{
			string query = "Delete from Products where Id = @productId";
			return await _dapperWrapper.ExecuteAsync(_connection, query, commandType: 1, new { @productId = productId }) > 0;		
		}

		public async Task<ProductDto> Get(int productId)
		{
			string query = "Select * from Products where Id = @productId";
			var result = _dapperWrapper.QueryAsync<Product>(_connection, query, commandType:1, new { @productId = productId }).GetAwaiter().GetResult().Single();

			ProductDto dto = _mapper.Map<ProductDto>(result);
			return dto;

		}

		public async Task<IEnumerable<ProductDto>> Get()
		{
			string query = "Select * from Products";
			var result = await _dapperWrapper.QueryAsync<Product>(_connection, query, commandType:1);			
			List<ProductDto> productDtos = new List<ProductDto>();
			foreach (var item in result)
			{
				productDtos.Add(_mapper.Map<ProductDto>(item));
			}
			return productDtos;
		}

		public async Task<bool> Update(ProductDto productDto)
		{
			if (productDto.ImageUrl == null)
			{
				string query2 = @"Update Products
							set Name=@name, Price=@price, CategoryName=@category, Description=@description
							where Id=@productId";
				return await _dapperWrapper.ExecuteAsync(_connection, query2, commandType: 1, new
				{
					@name = productDto.Name,
					@price = productDto.Price,
					@category = productDto.CategoryName,
					@description = productDto.Description,
					@productId = productDto.Id
				})>0;
			}
			else
			{
				string query1 = @"Update Products
							set Name=@name, Price=@price, ImageUrl=@image, CategoryName=@category, Description=@description
							where Id=@productId";
				return await _dapperWrapper.ExecuteAsync(_connection, query1, commandType: 1, new
				{
					@name = productDto.Name,
					@price = productDto.Price,
					@image = productDto.ImageUrl,
					@category = productDto.CategoryName,
					@description = productDto.Description,
					@productId = productDto.Id
				})>0;
			}
			
		}
	}
}
