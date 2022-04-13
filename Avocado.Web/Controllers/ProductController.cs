using Avocado.Web.Models;
using Avocado.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.Web.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _productService;
		public ProductController(IProductService productService)
		{
			_productService = productService;
		}
		public async Task<IActionResult> ProductIndex()
		{
			List<ProductDto> products = new List<ProductDto>();
			var result = await _productService.GetAllProductsAsync<ResponseDto>(null);
			if(result!=null && result.IsSuccess)
			{
				products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(result.ResponseObject));
			}
			return View(products);
		}
		public async Task<IActionResult> ProductEdit(int productId)
		{
			ProductDto productdto = null;
			var result = await _productService.GetProductAsync<ResponseDto>(productId, token:null);
			if (result != null && result.IsSuccess)
			{
				productdto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(result.ResponseObject));
			}
			return View(productdto);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("ProductEdit")]
		public async Task<IActionResult> ProductEditPost(ProductDto productDto)
		{
			if (ModelState.IsValid)
			{
				var result=await _productService.UpdateProductAsync<ResponseDto>(productDto, token: null);
				if(result!=null && result.IsSuccess)
				{
					return RedirectToAction(nameof(ProductIndex));
				}
			}
			return View(productDto);
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[ActionName("ProductCreate")]
		public async Task<IActionResult> ProductCreatePost(ProductDto productDto)
		{
			if (ModelState.IsValid)
			{
				var result = await _productService.CreateProductAsync<ResponseDto>(productDto, token: null);
				if(result!=null && result.IsSuccess)
				{
					return RedirectToAction(nameof(ProductIndex));
				}
			}
			return View(productDto);
		}
		[HttpDelete]
		public async Task<IActionResult> ProductDelete(int productId)
		{
			var result = await _productService.DeleteProductAsync<ResponseDto>(productId, token: null);
			if(result!=null && !result.IsSuccess)
			{
				TempData["error"] = result.ErrorMessages.Single();
			}
			TempData["success"] = "Delete successful";
			return RedirectToAction(nameof(ProductIndex));
		}
	}
}
