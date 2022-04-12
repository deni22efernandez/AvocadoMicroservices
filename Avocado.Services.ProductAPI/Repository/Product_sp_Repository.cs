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
	public class Product_sp_Repository : IProductRepository
	{
		protected string _connectionString;
		public Product_sp_Repository(IConfiguration config)
		{
			_connectionString = config.GetConnectionString("DefaultConnection");
		}
		public async Task<bool> Create(ProductDto productDto)
		{
			await using var connection = new SqlConnection(_connectionString);
			var parameters = new DynamicParameters();
			parameters.Add("@productId", 0, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
			parameters.Add("@name", productDto.Name);
			parameters.Add("@price", productDto.Price);
			parameters.Add("@image", productDto.ImageUrl);
			parameters.Add("@category", productDto.CategoryName);
			parameters.Add("@description", productDto.Description);
			await connection.ExecuteAsync("_spCreateProduct", parameters, commandType: System.Data.CommandType.StoredProcedure);
			if (parameters.Get<int>("@productId") != 0)
			{
				return true;
			}
			return false;
		}

		public async Task<bool> Delete(int productId)
		{
			await using var connection = new SqlConnection(_connectionString);
			var parameter = new DynamicParameters();
			parameter.Add("@id", productId);
			return await connection.ExecuteAsync("_spDeleteProduct", parameter, commandType: System.Data.CommandType.StoredProcedure)>0;		

		}

		public async Task<ProductDto> Get(int productId)
		{
			await using var connection = new SqlConnection(_connectionString);
			var param = new DynamicParameters();
			param.Add("@productId", productId);
			var result = connection.QueryAsync<Product>("_spGetProductById", param, commandType: System.Data.CommandType.StoredProcedure).GetAwaiter().GetResult().Single();

			return JsonConvert.DeserializeObject<ProductDto>(JsonConvert.SerializeObject(result));

		}

		public async Task<IEnumerable<ProductDto>> Get()
		{
			await using var connection = new SqlConnection(_connectionString);
			var result = await connection.QueryAsync<Product>("_spGetAllProducts", commandType: System.Data.CommandType.StoredProcedure);
			if (result == null)
			{
				return null;
			}
			return JsonConvert.DeserializeObject<List<ProductDto>>(JsonConvert.SerializeObject(result));
		}

		public async Task<bool> Update(ProductDto productDto)
		{
			await using var connection = new SqlConnection(_connectionString);
			var parameters = new DynamicParameters();
			parameters.Add("@id", productDto.Id, System.Data.DbType.Int32);
			parameters.Add("@name", productDto.Name);
			parameters.Add("@price", productDto.Price);
			parameters.Add("@image", productDto.ImageUrl);
			parameters.Add("@category", productDto.CategoryName);
			parameters.Add("@description", productDto.Description);
			return await connection.ExecuteAsync("_spUpdateProduct", parameters, commandType: System.Data.CommandType.StoredProcedure) > 0;

		}
	}
}
