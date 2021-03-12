using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProductCatalog.Api.Infrastructure;
using ProductCatalog.Api.Infrastructure.Data;
using ProductCatalog.Api.OpenApiHelpers;
using ProductCatalog.Api.Repositories;
using System;
using System.IO;
using System.Reflection;

namespace ProductCatalog.Api
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
            ConfigureCatalogDbContext(services);
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddScoped<ICatalogItemRepository, CatalogItemRepository>();
            services.AddScoped<IBrandsRepository, BrandsRepository>();

            services.AddAutoMapper(typeof(Startup).Assembly);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "ProductCatalog.Api",
                        Description = "Through this API you can access our Product Catalog",
                        Contact = new OpenApiContact
                        {
                            Email = "piyush.rathi.engg@gmail.com",
                            Name = "Piyush Rathi",
                            Url = new Uri("https://www.linkedin.com/in/piyush-rathi")
                        },
                        License = new OpenApiLicense()
                        {
                            Name = "MIT License",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        }
                    });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);

                // Add support for Vendor media type document generation
                // using AlsoProducesAttribute
                c.OperationFilter<CustomMediaTypeOperationFilter>();
            });

            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                 {
                     options.InvalidModelStateResponseFactory = context =>
                     {
                         // create a problem details object
                         var problemDetailsFactory = context.HttpContext.RequestServices
                                 .GetRequiredService<ProblemDetailsFactory>();

                         ValidationProblemDetails problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                                 context.HttpContext,
                                 context.ModelState,
                                 statusCode: StatusCodes.Status422UnprocessableEntity,
                                 title: "One or more validation errors occurred.",
                                 detail: "See the errors field for details.",
                                 instance: context.HttpContext.Request.Path);

                         return new UnprocessableEntityObjectResult(problemDetails)
                         {
                             ContentTypes = { "application/problem+json" }
                         };
                     };
                 });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "MyMovieStore.API v1");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private IServiceCollection ConfigureCatalogDbContext(IServiceCollection services)
            => services.AddDbContext<CatalogContext>(c => c.UseInMemoryDatabase("Catalog"));
    }
}
