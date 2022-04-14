using Avocado.Web.Services;
using Avocado.Web.Services.IServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.Web
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
			services.AddControllersWithViews();

			services.AddHttpClient<IProductService, ProductService>();
			services.AddScoped<IProductService, ProductService>();
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = "Cookies";
				x.DefaultChallengeScheme = "oidc";
			}).AddCookie("Cookies", x=> {
				x.ExpireTimeSpan = TimeSpan.FromMinutes(10);
			}).AddOpenIdConnect("oidc", x=> {
				x.Authority = "https://localhost:44388";
				x.ClientId = "mango";
				x.ClientSecret = "secret";
				x.ResponseType = "code";
				x.SaveTokens = true;
				x.Scope.Add("mango");
				x.TokenValidationParameters.RoleClaimType = "role";
				x.TokenValidationParameters.NameClaimType = "name";
				x.GetClaimsFromUserInfoEndpoint = true;
			});
			
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
