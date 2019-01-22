using Business.Logic.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.Configuration;
using Nancy.Owin;
using Nancy.TinyIoc;

namespace voucherz.redeem
{
	public class Startup
	{
		public IConfiguration Configuration;
		public Startup(IConfiguration configuration)
		{
			this.Configuration = configuration;
		}
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		//public void ConfigureServices(IServiceCollection services)
		//{
		//}

		//// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		//public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		//{
		//	if (env.IsDevelopment())
		//	{
		//		app.UseDeveloperExceptionPage();
		//	}

		//	app.Run(async (context) =>
		//	{
		//		await context.Response.WriteAsync("Hello World!");
		//	});
		//}
		public void Configure(IApplicationBuilder app)
		{
			app.UseOwin().UseNancy(opt => opt.Bootstrapper = new TracingBootstrapper(Configuration));
		}

	}
	public class TracingBootstrapper : Nancy.DefaultNancyBootstrapper
	{
		private IConfiguration config;

		public TracingBootstrapper(IConfiguration config)
		{
			this.config = config;
		}
		public override void Configure(INancyEnvironment env)
		{
			env.Tracing(enabled: true, displayErrorTraces: true);
		}

		protected override void ConfigureApplicationContainer(TinyIoCContainer container)
		{
			base.ConfigureApplicationContainer(container);
			container.Register<IConfiguration>(config);
			container.Register<IRedeemService, redeemService>();
		}
	}
}
