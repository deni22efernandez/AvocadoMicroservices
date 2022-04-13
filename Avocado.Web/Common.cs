using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.Web
{
	public class Common
	{
		public const string ProductBaseUri = "https://localhost:44313/api/products";
		public enum RequestType
		{
			GET,POST,PUT,DELETE
		}
	}
}
