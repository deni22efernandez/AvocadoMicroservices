using Avocado.Services.ProductAPI.DbContexts;
using Avocado.Services.ProductAPI.Mapping;
using Avocado.Services.ProductAPI.Models;
using Avocado.Services.ProductAPI.Models.Dtos;
using Avocado.Services.ProductAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Avocado.Services.ProductAPI.Tests
{
	[TestFixture]
	public class ProductRepositoryNUnitTests
	{
		private DbContextOptions<ApplicationDbContext> _options;
		private Mock<IMapper> _mapperMock;
		private ProductDto testProduct;
		private ProductRepository _productRepo;
		[SetUp]
		public void Setup()
		{
			 _options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("InMemoryDb").Options;
			_mapperMock = new Mock<IMapper>();
		     testProduct = new ProductDto
			{
				Id = 1,
				CategoryName = "test",
				Description = "test",
				ImageUrl = "test",
				Name = "test",
				Price = 1
			};
		}
		[Test]
		public async Task Create_InputProductDto_ReturnsTrue()
		{
			//arrange
			_mapperMock.Setup(x => x.Map<Product>(It.IsAny<object>())).Returns(new Product());

			//act
			using (var context = new ApplicationDbContext(_options))
			{
				_productRepo = new ProductRepository(context, _mapperMock.Object);
				await _productRepo.CreateAsync(testProduct);
			}


			//assert
			using (var contxt = new ApplicationDbContext(_options))
			{
				var result = await contxt.Products.FirstOrDefaultAsync(x => x.Id == testProduct.Id);
				Assert.AreEqual(testProduct.Id, result.Id);
			}
			
		}
		

	}
}
