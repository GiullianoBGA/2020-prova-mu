using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BGA.Framework.AspNetCore.Extensoes
{
    public static class ExtensoesAutoMapper
    {
        public static IServiceCollection RegistrarAutoMapper(this IServiceCollection services, params Assembly[] assemblies)
        {
            var expressaoConfiguracao = new AutoMapper.Configuration.MapperConfigurationExpression();
            expressaoConfiguracao.AddProfiles(assemblies);
            services.AddSingleton<IMapperConfigurationExpression>(expressaoConfiguracao);

            var configuracao = new MapperConfiguration(expressaoConfiguracao);
            services.AddSingleton<MapperConfiguration>(configuracao);

            var mapper = configuracao.CreateMapper();
            services.AddSingleton<IMapper>(mapper);

            MapeamentoExtensions.Inicializar(expressaoConfiguracao);

            return services;
        }
    }
}
