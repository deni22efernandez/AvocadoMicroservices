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

			string query = @"Insert into Products values (@name, @price, @image, @category, @description)";
			return _connection.ExecuteAsync(query, new
			{
				@name = productDto.Name,
				@price = productDto.Price,
				@image = productDto.ImageUrl,
				@category = productDto.CategoryName,
				@description = productDto.Description
			}).GetAwaiter().GetResult() > 0;

		}

		public async Task<bool> Delete(int productId)
		{
			await using var _connection = new SqlConnection(_connectionString);
			string query = "Delete from Products where Id = @productId";
			var result= _connection.ExecuteAsync(query, new { @productId = productId }).GetAwaiter().GetResult() > 0;
			return result;
		}

		public async Task<ProductDto> Get(int productId)
		{
			await using var _connection = new SqlConnection(_connectionString);
			string query = "Select * from Products where Id = @productId";
			var result = _connection.QueryAsync<Product>(query, new { @productId = productId }).GetAwaiter().GetResult().Single();

			ProductDto dto = JsonConvert.DeserializeObject<ProductDto>(JsonConvert.SerializeObject(result));
			//var json = JsonConvert.SerializeObject(result);
			//ProductDto dto = JsonConvert.DeserializeObject<ProductDto>(json);
			//ProductDto dto = new ProductDto()
			//{
			//	Id = result.Id,
			//	Name = result.Name,
			//	CategoryName = result.CategoryName,
			//	Description = result.Description,
			//	ImageUrl = result.ImageUrl,
			//	Price = result.Price
			//};
			return dto;

		}

		public async Task<IEnumerable<ProductDto>> Get()
		{
			await using var _connection = new SqlConnection(_connectionString);
			string query = "Select * from Products";
			var result = await _connection.QueryAsync<Product>(query);			
			List<ProductDto> productDtos = new List<ProductDto>();
			foreach (var item in result)
			{
				productDtos.Add(JsonConvert.DeserializeObject<ProductDto>(JsonConvert.SerializeObject(item)));
			}
			return productDtos;
		}

		public async Task<bool> Update(ProductDto productDto)
		{
			await using var _connection = new SqlConnection(_connectionString);
			if (productDto.ImageUrl == null)
			{
				string query2 = @"Update Products
							set Name=@name, Price=@price, CategoryName=@category, Description=@description
							where Id=@productId";
				return await _connection.ExecuteAsync(query2, new
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
				return await _connection.ExecuteAsync(query1, new
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
