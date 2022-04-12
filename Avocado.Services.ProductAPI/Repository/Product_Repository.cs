using Avocado.Services.ProductAPI.Models;
using Avocado.Services.ProductAPI.Models.Dtos;
using Avocado.Services.ProductAPI.Repository.IRepository;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.Services.ProductAPI.Repository
{
	public class Product_Repository : IProductRepository
	{
		private string _connectionString = "";
		public Product_Repository(IConfiguration config)
		{
			_connectionString = config.GetConnectionString("DefaultConnection");
		}

		public async Task<bool> Create(ProductDto productDto)
		{
			await using var _connection = new SqlConnection(_connectionString);

			string query = @"Insert into Products (Name, Price, ImageUrl, CategoryName, Description) values
												(@name, @price, @image, @category, @description)";
			var result = await _connection.QueryAsync<int>(query, new
			{
				@name = productDto.Name,
				@price = productDto.Price,
				@image = productDto.ImageUrl,
				@category = productDto.CategoryName,
				@description = productDto.Description
			});

			return result.Count() > 0;
		}

		public async Task<bool> Delete(int productId)
		{
			await using var _connection = new SqlConnection(_connectionString);
			string query = "Delete from Products where Id == @productId";
			var result = await _connection.QueryAsync<int>(query, new { @productId = productId });
			return result.Count() > 0;
		}

		public async Task<ProductDto> Get(int productId)
		{
			await using var _connection = new SqlConnection(_connectionString);
			string query = "Select * from Products where Id == @productId";
			var result = await _connection.QueryAsync<Product>(query, new { @productId = productId });
			if (result.Any())
			{
				ProductDto dto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(result));
				return dto;
			}
			return null;
		}

		public async Task<IEnumerable<ProductDto>> Get()
		{
			await using var _connection = new SqlConnection(_connectionString);
			string query = "Select * from Products";
			var result = await _connection.QueryAsync<IEnumerable<Product>>(query);
			if (result.Count() > 0)
			{
				List<ProductDto> productDtos = new List<ProductDto>();
				foreach (var item in result)
				{
					productDtos.Add(JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(item)));
				}
				return productDtos;
			}
			return null;
		}

		public async Task<bool> Update(ProductDto productDto)
		{
			await using var _connection = new SqlConnection(_connectionString);
			string query1 = @"Update Products
							set Name=@name, Price=@price, ImageUrl=@image, CategoryName=@category, Description=@description
							where Id==@productId";
			string query2 = @"Update Products
							set Name=@name, Price=@price, CategoryName=@category, Description=@description
							where Id==@productId";

			IEnumerable<int> result = new List<int>();

			if (productDto.ImageUrl == null)
			{
				result = await _connection.QueryAsync<int>(query2, new
				{
					@name = productDto.Name,
					@price = productDto.Price,
					@category = productDto.CategoryName,
					@description = productDto.Description,
					@productId = productDto.Id
				});
			}
			else
			{
				result = await _connection.QueryAsync<int>(query1, new
				{
					@name = productDto.Name,
					@price = productDto.Price,
					@image=productDto.ImageUrl,
					@category = productDto.CategoryName,
					@description = productDto.Description,
					@productId = productDto.Id
				});
			}
			return result.Count() > 0;
		}
	}
}
