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
	public class Product_sp_Repository : IProductRepository
	{
		public IDbConnection connection;
		public IDapperWrapper _dapperWrapper;
		private IMapper _mapper;
		public Product_sp_Repository(IConfiguration config, IDapperWrapper dapperWrapper, IMapper mapper)
		{
			_mapper = mapper;
			_dapperWrapper = dapperWrapper;
			this.connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
		}
		public async Task<bool> Create(ProductDto productDto)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@productId", productDto.Id, direction: ParameterDirection.Output);
			parameters.Add("@name", productDto.Name);
			parameters.Add("@price", productDto.Price);
			parameters.Add("@image", productDto.ImageUrl);
			parameters.Add("@category", productDto.CategoryName);
			parameters.Add("@description", productDto.Description);
			await _dapperWrapper.ExecuteAsync(connection, "_spCreateProduct", commandType: 2, parameters);
			//await connection.ExecuteAsync("_spCreateProduct", parameters, commandType: System.Data.CommandType.StoredProcedure);
			if (parameters.Get<int>("@productId") != 0)
			{
				return true;
			}
			return false;
		}

		public async Task<int> CreateAsync(ProductDto productDto)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@productId", productDto.Id, direction: ParameterDirection.Output);
			parameters.Add("@name", productDto.Name);
			parameters.Add("@price", productDto.Price);
			parameters.Add("@image", productDto.ImageUrl);
			parameters.Add("@category", productDto.CategoryName);
			parameters.Add("@description", productDto.Description);
			
			await _dapperWrapper.ExecuteAsync(connection,"_spCreateProduct", commandType: 2, parameters);
			var result = parameters.Get<int>("@productId");
			return result;
		}

		public async Task<bool> Delete(int productId)
		{
			var parameter = new DynamicParameters();
			parameter.Add("@id", productId);
			return await _dapperWrapper.ExecuteAsync(connection, "_spDeleteProduct", commandType: 2, parameter)>0;		

		}

		public async Task<ProductDto> Get(int productId)
		{
			var param = new DynamicParameters();
			param.Add("@productId", productId);
			var result = _dapperWrapper.Query<Product>(connection,"_spGetProductById", commdType: 2, param);
			
			return _mapper.Map<ProductDto>(result);

		}

		public async Task<IEnumerable<ProductDto>> Get()
		{
			var result = await _dapperWrapper.QueryAsync<Product>(connection, "_spGetAllProducts", commandType: 2);
			if (result == null)
			{
				return null;
			}
			return _mapper.Map<List<ProductDto>>(result);
		}

		public async Task<bool> Update(ProductDto productDto)
		{
			var parameters = new DynamicParameters();
			parameters.Add("@id", productDto.Id, System.Data.DbType.Int32);
			parameters.Add("@name", productDto.Name);
			parameters.Add("@price", productDto.Price);
			if (productDto.ImageUrl != null)
			{
				parameters.Add("@image", productDto.ImageUrl);
			}			
			parameters.Add("@category", productDto.CategoryName);
			parameters.Add("@description", productDto.Description);
			if (productDto.ImageUrl != null)
			{
				return await _dapperWrapper.ExecuteAsync(connection, "_spUpdateProduct", commandType: 2, parameters) > 0;
			}
			else
			{
				return await _dapperWrapper.ExecuteAsync(connection, "_spUpdateProductWithoutImage", commandType: 2, parameters)> 0;
			}	

		}

		
	}
}
