using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCore.AutoRegisterDi;
using TrueLayer.Model;
using TrueLayer.Service;

namespace TrueLayerTest.Api
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

            services.AddHttpClient();

            services.AddTransient<IPokemonService, PokemonService>();
            services.AddTransient<ITranslationServiceFactory, TranslationServiceFactory>();
            services.AddSingleton(typeof(IHttpService<TranslateResponse>), typeof(HttpService<TranslateResponse>));
            services.AddSingleton(typeof(IHttpService<PokemonSpecies>), typeof(HttpService<PokemonSpecies>));

            services.RegisterAssemblyPublicNonGenericClasses(
                    Assembly.GetAssembly(typeof(ITranslationService)))
                .Where(c => c.Name.EndsWith("TranslationService"))
                .AsPublicImplementedInterfaces();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
