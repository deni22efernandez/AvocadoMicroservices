using Avocado.Services.ProductAPI.Dapper;
using Avocado.Services.ProductAPI.Mapping;
using Avocado.Services.ProductAPI.Models;
using Avocado.Services.ProductAPI.Models.Dtos;
using Avocado.Services.ProductAPI.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avocado.Services.ProductAPI.Tests
{
	[TestFixture]
	public class Product_RepositoryNUnitTests
	{
		private Mock<IMapper> _mapperMock;
		private Mock<IConfiguration> configuration;
		private ProductDto fake;
		private Mock<IDapperWrapper> _dapperWrapper;
		private string connectionString;


		[SetUp]
		public void Setup()
		{
			_mapperMock = new Mock<IMapper>();
			configuration = new Mock<IConfiguration>();
			_dapperWrapper = new Mock<IDapperWrapper>();
			connectionString = "Server=localhost\\SQLEXPRESS;Database=AvocadoProductAPI;MultipleActiveResultSets=true;Trusted_Connection=true";

			fake = new ProductDto
			{
				Id = 15,
				CategoryName = "fake",
				Description = "fake",
				ImageUrl = "null",
				Name = "fake",
				Price = 200
			};


		}
		[Test]
		public async Task Create_InputProductDto_ReturnsTrue()
		{
			var repo = new Product_sp_Repository(configuration.Object, _dapperWrapper.Object, _mapperMock.Object);
			_dapperWrapper.Setup(x => x.ExecuteAsync(It.Is<IDbConnection>(x => x.ConnectionString == connectionString),
			 It.IsAny<string>(),
			 It.IsAny<object>(),
			 It.IsAny<int>())).ReturnsAsync(1);
			var result = await repo.Create(fake);
			Assert.IsTrue(result);
		}
		[Test]
		public async Task CreateAsync_InputProductDto_ReturnsLastProductIdGenerated()
		{
			var repo = new Product_sp_Repository(configuration.Object, _dapperWrapper.Object, _mapperMock.Object);
			_dapperWrapper.Setup(x => x.ExecuteAsync(It.Is<IDbConnection>(x => x.ConnectionString == connectionString),
			 It.IsAny<string>(),
			 It.IsAny<object>(),
			 It.IsAny<int>())).ReturnsAsync(fake.Id);
			var result = await repo.CreateAsync(fake);
			Assert.AreEqual(fake.Id, result);
		}
		[Test]
		public async Task GetProductById_InputsExistingId_ReturnsProduct()
		{

			_dapperWrapper.Setup(x => x.Query<Product>(It.Is<IDbConnection>(x => x.ConnectionString == connectionString),
														  It.IsAny<string>(),
														  It.IsAny<int>(),
														  It.IsAny<object>()))
														  .Returns(new List<Product> {new Product()
															{
																CategoryName = fake.CategoryName,
																Description = fake.Description,
																Id = fake.Id,
																ImageUrl = fake.ImageUrl,
																Name = fake.Name,
																Price = fake.Price
															} });
			_mapperMock.Setup(x => x.Map<ProductDto>(It.IsAny<string>())).Returns(fake);


			var productRepo = new Product_sp_Repository(configuration.Object, _dapperWrapper.Object, _mapperMock.Object);

			var result = await productRepo.Get(fake.Id);
			Assert.AreEqual(fake, result);

		}


		[Test]
		public async Task GetAllProducts_ReturnsListOfProducts()
		{
			_dapperWrapper.Setup(x=>x.QueryAsync<Product>(It.Is<IDbConnection>(x=>x.ConnectionString==connectionString),
									It.IsAny<string>(),
									It.IsAny<int>(),
									null));
			_mapperMock.Setup(x => x.Map<List<ProductDto>>(It.IsAny<Product>()));
			var productRepo = new Product_sp_Repository(configuration.Object, _dapperWrapper.Object, _mapperMock.Object);

			var result = await productRepo.Get();
			Assert.AreEqual(fake, result);

		}
	}
}
