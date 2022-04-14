using Avocado.Web.Models;
using Avocado.Web.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.Web.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _productService;
		private readonly IWebHostEnvironment _hostEnvironment;
		public ProductController(IProductService productService, IWebHostEnvironment hostEnvironment)
		{
			_productService = productService;
			_hostEnvironment = hostEnvironment;
		}
		public async Task<IActionResult> ProductIndex()
		{
			List<ProductDto> products = new List<ProductDto>();
			var result = await _productService.GetAllProductsAsync<ResponseDto>(null);
			if(result!=null && result.IsSuccess)
			{
				products = JsonConvert.DeserializeObject<List<ProductDto>>(JsonConvert.SerializeObject(result.ResponseObject));
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
				var formFiles = HttpContext.Request.Form.Files;
				if (formFiles.Count() > 0)//si se sube una imagen
				{
					string fileName = Guid.NewGuid().ToString();
					var webPath = _hostEnvironment.WebRootPath;
					var fileExtension = Path.GetExtension(formFiles[0].FileName);
					var uploads = webPath + @"\images\";

					string imageFromDb = null;
					var request = await _productService.GetProductAsync<ResponseDto>(productDto.Id, token: null);
					if(request!=null && request.IsSuccess)
					{
						//busco la imagen en la db para reemplazarla en images folder						
						var productFromDb = JsonConvert.DeserializeObject<ProductDto>(JsonConvert.SerializeObject(request.ResponseObject));
						imageFromDb = productFromDb.ImageUrl;
						if (!String.IsNullOrEmpty(imageFromDb))
						{
							if (System.IO.File.Exists(Path.Combine(uploads, imageFromDb)))
							{
								System.IO.File.Delete(Path.Combine(uploads, imageFromDb));
							}
						}
						
					}					

					using (var fileStream = new FileStream(Path.Combine(uploads, fileName + fileExtension), FileMode.Create))
					{
						formFiles[0].CopyTo(fileStream);
					}

					productDto.ImageUrl = fileName + fileExtension;
				}
				var result=await _productService.UpdateProductAsync<ResponseDto>(productDto, token: null);
				if(result!=null && result.IsSuccess)
				{
					return RedirectToAction(nameof(ProductIndex));
				}
			}
			return View(productDto);
		}
		public IActionResult ProductCreate()
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
				var formFiles = HttpContext.Request.Form.Files;
				if (formFiles.Count() > 0)
				{
					string fileName = Guid.NewGuid().ToString();
					var webPath = _hostEnvironment.WebRootPath;
					var fileExtension = Path.GetExtension(formFiles[0].FileName);
					var uploads = webPath + @"\images\";

					using (var fileStream = new FileStream(Path.Combine(uploads, fileName + fileExtension), FileMode.Create))
					{
						formFiles[0].CopyTo(fileStream);
					}

					productDto.ImageUrl = fileName + fileExtension;
				}
				var result = await _productService.CreateProductAsync<ResponseDto>(productDto, token: null);
				if(result!=null && result.IsSuccess)
				{
					return RedirectToAction(nameof(ProductIndex));
				}
			}
			return View(productDto);
		}
	
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
