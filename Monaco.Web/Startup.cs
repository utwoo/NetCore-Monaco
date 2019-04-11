using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monaco.Core.Autofac;
using Monaco.Core.Caching;
using Monaco.Core.Infrastructure.Extensions;
using Monaco.Core.MessageQueue;
using Monaco.Core.SEQ;
using Monaco.Data.Core.Infrastructure.Extensions;

namespace Monaco.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Initialize Configurations
            var monacoConfig = services.InitializeConfigurations(Configuration);
            // Register HttpContext Accessor
            services.AddHttpContextAccessor();
            // Register AutoMapper
            services.AddMonacoMapper();
            // Register DataBase Context
            services.AddMonacoDbContext(monacoConfig);
            // Register MVC
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // Register Autofac
            var serviceProvider = services.AddMonacoAutoFac(monacoConfig);

            return serviceProvider;
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
