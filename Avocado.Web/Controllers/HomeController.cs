using Avocado.Web.Models;
using Avocado.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly IProductService _productService;

		public HomeController(IProductService productService)
		{
			_productService = productService;
		}

		public async Task<IActionResult> Index()
		{
			List<ProductDto> productList = new List<ProductDto>();
			var result = await _productService.GetAllProductsAsync<ResponseDto>(null);
			if (result!=null && result.IsSuccess)
			{
				productList=JsonConvert.DeserializeObject<List<ProductDto>>(JsonConvert.SerializeObject(result.ResponseObject));
			}
			return View(productList);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
