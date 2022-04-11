using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Avocado.Web.Common;

namespace Avocado.Web.Models
{
	public class ApiRequest
	{
		public string Url { get; set; }
		public object Object { get; set; }
		public string Token { get; set; } = "";
		public RequestType RequestType { get; set; }
	}
}
