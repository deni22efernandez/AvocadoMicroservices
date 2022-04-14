using Avocado.Web.Models;
using Avocado.Web.Services.IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Avocado.Web.Services
{
	public class BaseService : IBaseService
	{
		private readonly IHttpClientFactory _httpclientFactory;
	
		public BaseService(IHttpClientFactory httpclientFactory)
		{
			_httpclientFactory = httpclientFactory;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(true);
		}

		public async Task<T> SendRequest<T>(ApiRequest apiRequest)
		{
			try
			{
				using var client = _httpclientFactory.CreateClient("AvocadoWeb");
				client.DefaultRequestHeaders.Clear();

				HttpRequestMessage requestMessage = new HttpRequestMessage();
				requestMessage.Headers.Add("Accept", "application/json");
				HttpMethod requestType;
				switch (apiRequest.RequestType)
				{
					case Common.RequestType.GET:
						requestType = HttpMethod.Get;
						break;
					case Common.RequestType.POST:
						requestType = HttpMethod.Post;
						break;
					case Common.RequestType.PUT:
						requestType = HttpMethod.Put;
						break;
					default:
						requestType = HttpMethod.Delete;
						break;
				}
				requestMessage.Method = requestType;
				requestMessage.RequestUri = new Uri(apiRequest.Url);
				if (apiRequest.Object != null)
				{
					var message = JsonConvert.SerializeObject(apiRequest.Object);
					requestMessage.Content = new StringContent(message, Encoding.UTF8, "application/json");
				}
				if (!String.IsNullOrEmpty(apiRequest.Token))
				{
					requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiRequest.Token);
				}

				HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);

				var content = await responseMessage.Content.ReadAsStringAsync();
				var responseDto = JsonConvert.DeserializeObject<T>(content);
				return responseDto;

			}
			catch (Exception e)
			{
				ResponseDto response = new ResponseDto
				{
					IsSuccess = false,
					ErrorMessages = new List<string>() { e.ToString() }
				};
				var resp = JsonConvert.SerializeObject(response);
				var responseDto = JsonConvert.DeserializeObject<T>(resp);
				return responseDto;
			}


		}
	}
}
