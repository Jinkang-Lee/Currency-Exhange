using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Currency_Exchange
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, 
        // visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);

            // adding authentication handler for Account using authentication scheme "AdminAccount"
            services
               .AddAuthentication("AdminAccount")
               .AddCookie("AdminAccount",
                   options =>
                   {
                       options.LoginPath = "/AdminAccount/Login/";
                       options.AccessDeniedPath = "/AdminAccount/Forbidden/";
                   });

            // adding authentication handler for Account using authentication scheme "StaffAccount"
            services
               .AddAuthentication("StaffAccount")
               .AddCookie("StaffAccount",
                   options =>
                   {
                       options.LoginPath = "/StaffAccount/Login/";
                       options.AccessDeniedPath = "/StaffAccount/Forbidden/";
                   });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(
               routes =>
               {
                   routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
               });
        }
    }
}

