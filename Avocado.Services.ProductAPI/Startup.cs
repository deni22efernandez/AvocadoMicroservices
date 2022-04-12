using Avocado.Services.ProductAPI.DbContexts;
using Avocado.Services.ProductAPI.Repository;
using Avocado.Services.ProductAPI.Repository.IRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.Services.ProductAPI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			services.AddDbContext<ApplicationDbContext>(x =>
			{
				x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
			});
			services.AddScoped<IProductRepository, Product_sp_Repository>();
			//services.AddScoped<IProductRepository, Product_Repository>();
			//services.AddScoped<IProductRepository, ProductRepository>();
			services.AddSwaggerGen(x=> {
				x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
				{
					Title = "Avocado.Services.ProductAPI",
					Version = "v1"
				});
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(x =>
				{
					x.SwaggerEndpoint("/swagger/v1/swagger.json", "Avocado.Services.ProductAPI v1");
				});
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
