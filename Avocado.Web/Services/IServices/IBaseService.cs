using Avocado.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.Web.Services.IServices
{
	public interface IBaseService:IDisposable
	{
		Task<T> SendRequest<T>(ApiRequest apiRequest);
	}
}
