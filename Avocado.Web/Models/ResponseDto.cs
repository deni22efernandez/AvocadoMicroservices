using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.Web.Models
{
	public class ResponseDto
	{
		public bool IsSuccess { get; set; } = true;
		public object ResponseObject { get; set; }
		public List<string> ErrorMessages { get; set; } = new List<string>();
	}
}
