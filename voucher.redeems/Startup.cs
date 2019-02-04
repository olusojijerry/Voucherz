using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Logic.ClientComm;
using Business.Logic.Domain;
using Business.Logic.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace voucher.redeems
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
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});
			services.AddScoped<IClientCommunication, ClientCommunication>();
			services.AddScoped<IBaseDao, BaseDao>();
			services.AddScoped<IRedeemService, RedeemService>();
			//services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
			//{
			//	builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
			//}));
			services.AddCors();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();
			app.UseCors(options => options
			.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader()
			);

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Redemption}/{action=FrontDetails}/{id?}");
			});
		}
	}
}
