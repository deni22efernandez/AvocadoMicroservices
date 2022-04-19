using Newtonsoft.Json;

namespace Avocado.Services.ProductAPI.Mapping
{
	public class Mapper : IMapper
	{
		public T Map<T>(object value)
		{
			return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(value));
		}
	}
}
