using BGA.Framework.AspNetCore.Extensoes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prova.Data;
using Prova.Modelos;
using Prova.Repositorio;
using Prova.Servicos;
using System.Reflection;

namespace Prova.Backend
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddTransient<INotaDeCompraServices, NotaDeCompraServices>();
            services.AddTransient<IUsuarioServices, UsuarioServices>();
            services.AddSingleton<TokenViewModel>();

            services.AddDbContext<ProvaContext>(opcoesContexto =>
            {
                opcoesContexto.UseSqlServer(Configuration.GetConnectionString("ConexaoBD"), opcoes =>
                {
                    opcoes.CommandTimeout(240);
                });
                opcoesContexto.EnableSensitiveDataLogging();
            });
            services.RegistrarUnitOfWork<ProvaContext>();

            Assembly[] assemblies =
            {
                Assembly.Load("Prova.Backend"),
                Assembly.Load("Prova.Poco"),
                Assembly.Load("Prova.Modelos"),
            };

            ExtensoesAutoMapper.RegistrarAutoMapper(services, assemblies);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapBlazorHub();
                //endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
