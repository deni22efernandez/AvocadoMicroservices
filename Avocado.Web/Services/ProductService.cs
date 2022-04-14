using Avocado.Web.Models;
using Avocado.Web.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Avocado.Web.Services
{
	public class ProductService : BaseService, IProductService
	{
		private readonly IHttpClientFactory _httpClient;
		public ProductService(IHttpClientFactory httpClient) : base(httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task<T> CreateProductAsync<T>(ProductDto productDto, string token)
		{
			return await SendRequest<T>(new ApiRequest
			{
				Object = productDto,
				RequestType = Common.RequestType.POST,
				Token = "",
				Url = Common.ProductBaseUri
			});
		}

		public async Task<T> DeleteProductAsync<T>(int productId, string token)
		{
			return await SendRequest<T>(new ApiRequest
			{
				RequestType = Common.RequestType.DELETE,
				Token = "",
				Url = Common.ProductBaseUri + "/" + productId
			});
		}

		public async Task<T> GetAllProductsAsync<T>(string token)
		{
			return await SendRequest<T>(new ApiRequest
			{
				Token = "",
				RequestType = Common.RequestType.GET,
				Url = Common.ProductBaseUri 
			});
		}

		public async Task<T> GetProductAsync<T>(int productId, string token)
		{
			return await SendRequest<T>(new ApiRequest
			{
				Token = "",
				RequestType = Common.RequestType.GET,
				Url = Common.ProductBaseUri + "/" + productId
			});
		}

		public async Task<T> UpdateProductAsync<T>(ProductDto productDto, string token)
		{
			return await SendRequest<T>(new ApiRequest
			{
				Object=productDto,
				Token = "",
				RequestType = Common.RequestType.PUT,
				Url = Common.ProductBaseUri
			});
		}
	}
}
