using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;

namespace Middleware
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Middleware", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Middleware v1"));
            }
            //==== static files =================================
            // app.UseStaticFiles();
            // app.UseDefaultFiles();// 4 index.html or 3 index.htm 2 default.html or 1 default.htm
            // or
            //app.UseFileServer();// replace UseStaticFiles() + UseDefaultFiles() + UseDirectoryBrowser()

            //DefaultFilesOptions d = new DefaultFilesOptions();
            //======== custom =====================================

            //app.Use(async (context, next) =>
            //{
            //    logger.LogInformation("from logger !");
            //    await context.Response.WriteAsync($"{context.User} \n");

            //    await context.Response.WriteAsync("MW1: Incoming Request \n");
            //    await next();
            //    //Why use response middleware??
            //    await context.Response.WriteAsync("MW1: Outgoing Response \n");
            //});

            //app.Use(async (context, next) =>// short-circuit
            //{
            //    await context.Response.WriteAsync("MW2: Incoming Request \n");
            //    //throw new Exception("");
            //    await next();
            //   // await context.Response.WriteAsync("MW2: Outgoing Response \n");
            //});

            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("MW3: Incoming Request \n");
            //    await next();
            //    await context.Response.WriteAsync("MW3: Outgoing Response \n");
            //});
            ////custom endpoint/last middleware

            //app.Run(async context =>
            //        await context.Response.WriteAsync("MW4: Request handled and response produced \n")
            //  );

            //=====================================================


            app.UseRouting();

            app.UseAuthorization();

            // need   app.UseRouting(); 
            // last middleware endpoints
            // take delegate request

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
