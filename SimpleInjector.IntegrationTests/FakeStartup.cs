using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SimpleInjector.WebAPI;

namespace SimpleInjector.IntegrationTests
{
    public class FakeStartup : Startup
    {
        public FakeStartup(IConfiguration configuration, IHostEnvironment environment) : base(configuration, environment)
        {
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Container.Options.AllowOverridingRegistrations = true;
            Container.RegisterInstance<IMyDependency>(new MyFakeDependency());
            Container.Options.AllowOverridingRegistrations = false;
            base.Configure(app, env);
        }
    }
}
