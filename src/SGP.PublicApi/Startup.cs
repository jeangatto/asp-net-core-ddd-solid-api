using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SGP.Application;
using SGP.Infrastructure;
using SGP.Infrastructure.Migrations;

namespace SGP.PublicApi
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
            services.AddApplication();

            services.AddInfrastructure();

            services.AddContextWithMigrations(Configuration);

            services.ConfigureAppSettings(Configuration);

            services.AddCors();

            services.AddHttpContextAccessor();

            services.AddResponseCompression();

            services.Configure<RouteOptions>(routeOptions =>
            {
                routeOptions.LowercaseUrls = true;
                routeOptions.LowercaseQueryStrings = true;
            });

            services.AddControllers()
                .ConfigureApiBehaviorOptions(apiBehaviorOptions =>
                {
                    apiBehaviorOptions.SuppressMapClientErrors = true;
                    apiBehaviorOptions.SuppressModelStateInvalidFilter = true;
                })
                .AddNewtonsoftJson(jsonOptions =>
                {
                    var namingStrategy = new CamelCaseNamingStrategy();
                    jsonOptions.SerializerSettings.Formatting = Formatting.None;
                    jsonOptions.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    jsonOptions.SerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = namingStrategy };
                    jsonOptions.SerializerSettings.Converters.Add(new StringEnumConverter(namingStrategy));
                });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SGP",
                    Version = "v1"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.DisplayRequestDuration();
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "SGP v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseHsts();

            app.UseRouting();

            app.UseResponseCompression();

            app.UseAuthorization();

            app.UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
            });

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}