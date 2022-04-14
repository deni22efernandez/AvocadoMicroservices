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
using Microsoft.OpenApi.Models;
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
			services.AddSwaggerGen(x =>
			{
				x.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Avocado.Services.ProductAPI",
					Version = "v1"
				});
				x.EnableAnnotations();
				x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Description = "Enter Bearer [space] and your token",
					Scheme = "Bearer"
				});
				x.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
					{
						new OpenApiSecurityScheme
						{
							 Reference=new OpenApiReference
							 {
								  Id="Bearer",
								  Type= ReferenceType.SecurityScheme
							 },
							  In=ParameterLocation.Header,
							   Scheme="OAUTH2",
								Name="Bearer"
						},
						new List<string>()
					}
				});
			});
			services.AddAuthentication("Bearer")
				.AddJwtBearer("Bearer", x =>
			{
				x.Authority = "https://localhost:44388/";
				x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
				{
					ValidateAudience = false
				};
			});
			services.AddAuthorization(x =>
			{
				x.AddPolicy("ApiScope", policy =>
				{
					policy.RequireAuthenticatedUser();
					policy.RequireClaim("scope","mango");
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
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
