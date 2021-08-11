using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ParkyAPI.Data;
using ParkyAPI.Repository.IRepository;
using ParkyAPI.ParkyMapper;
using ParkyAPI.Repository;
using System.Reflection;
using System.IO;
using System;

namespace ParkyAPI
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
            services.AddDbContext<ApplicationDbContext>
            (
                options => options.UseSqlServer(Configuration.GetConnectionString("DefultConnection"))
            );
            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<ITrailRepository, TrailRepository>();

            services.AddAutoMapper(typeof(ParkyMappings));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("ParkyOpenAPISpec", new OpenApiInfo 
                { 
                    Title = "ParkyAPI", 
                    Version = "v1",
                    Description = "Udemy Parky API",
                    Contact = new OpenApiContact
                    {
                        Email = "parky@mail.com",
                        Name = "Irina",
                        Url = new Uri("https://aurorahost.ru")
                    }, 
                    License = new OpenApiLicense
                    {
                        Name = "MIT Licence",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_Licence")
                    }
                });
                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                c.IncludeXmlComments(cmlCommentsFullPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(
                    c => {
                        c.SwaggerEndpoint("/swagger/ParkyOpenAPISpec/swagger.json", "Parky API");
                        c.RoutePrefix = "";
                    });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
