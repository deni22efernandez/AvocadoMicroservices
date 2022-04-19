using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.Services.ProductAPI.Mapping
{
	public interface IMapper
	{
		T Map<T>(object value);
	}
}
