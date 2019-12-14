using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using jostva.Reactivities.API.Middleware;
using jostva.Reactivities.application.Activities;
using jostva.Reactivities.Data;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace jostva.Reactivities.API
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
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .WithOrigins("http://localhost:3000");
                });
            });

            services.AddMediatR(typeof(List.Handler).Assembly);
            services.AddMvc()
                    .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Create>())
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }           

            //app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            app.UseMvc();
        }
    }
}
