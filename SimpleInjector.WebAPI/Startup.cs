using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SimpleInjector.WebAPI
{
    public class Startup
    {
        protected readonly Container Container;

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Container = new Container();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSimpleInjector(Container, options =>
            {
                options
                    .AddAspNetCore()
                    .AddControllerActivation();
                options.AddLogging();
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(configure => configure.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            Container.RegisterInstance<IMyDependency>(new MyRealDependency());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSimpleInjector(Container);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            Container.Verify();
        }
    }
}
