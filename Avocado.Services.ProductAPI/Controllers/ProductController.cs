using Avocado.Services.ProductAPI.Models.Dtos;
using Avocado.Services.ProductAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.Services.ProductAPI.Controllers
{
	[Route("api/products")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		//private readonly IProductRepository _productRepo;
		private readonly IProductRepository _spProductRepo;
		protected ResponseDto responseDto;
		public ProductController(/*IProductRepository productRepo, */IProductRepository spProductRepo)
		{
			//_productRepo = productRepo;
			_spProductRepo = spProductRepo;
			responseDto = new ResponseDto();
		}
		[HttpGet("{productId:int}")]
		public async Task<object> Get(int productId)
		{
			try
			{
				//var result = await _productRepo.Get(productId);
				var result = await _spProductRepo.Get(productId);
				responseDto.ResponseObject = result;
			}
			catch (Exception e)
			{

				responseDto.IsSuccess = false;
				responseDto.ErrorMessages = new List<string>() { e.ToString() };
			}
			return responseDto;
		}
		[HttpGet]
		public async Task<object> Get()
		{
			try
			{
				//var result = await _productRepo.Get();
				var result = await _spProductRepo.Get();
				responseDto.ResponseObject = result;
			}
			catch (Exception e)
			{
				responseDto.IsSuccess = false;
				responseDto.ErrorMessages = new List<string>() { e.ToString() };
			}
			return responseDto;
		}
		[HttpPost]
		public async Task<object> Post([FromBody] ProductDto productDto)
		{
			try
			{
				//await _productRepo.Create(productDto);
				//await _spProductRepo.Create(productDto);
				await _spProductRepo.CreateAsync(productDto);//returns last id created
			}
			catch (Exception e)
			{
				responseDto.IsSuccess = false;
				responseDto.ErrorMessages = new List<string>() { e.ToString() };
			}
			return responseDto;
		}
		[HttpPut]
		public async Task<object> Put([FromBody] ProductDto productDto)
		{
			try
			{
				//await _productRepo.Update(productDto);
				await _spProductRepo.Update(productDto);
			}
			catch (Exception e)
			{
				responseDto.IsSuccess = false;
				responseDto.ErrorMessages = new List<string>() { e.ToString() };
			}
			return responseDto;
		}

		[HttpDelete("{productId:int}")]
		public async Task<object> Delete(int productId)
		{
			if (!await _spProductRepo.Delete(productId))
			//if (!await _productRepo.Delete(productId))
			{
				responseDto.IsSuccess = false;
				responseDto.ErrorMessages = new List<string>() { "Error while deleting" };
			}
			return responseDto;
		}
	}
}
