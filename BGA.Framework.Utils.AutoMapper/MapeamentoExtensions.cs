using AutoMapper;

namespace BGA.Framework.AspNetCore.Extensoes
{
    public static class MapeamentoExtensions
    {
        private static IMapper Mapeador;
        private static bool IsInicializado = false;

        public static TElemento Mapear<TElemento>(this object origem)
        {
            try { return Mapeador.Map<TElemento>(origem); }
            catch { return default(TElemento); }
        }

        public static void Inicializar(AutoMapper.Configuration.MapperConfigurationExpression expressaoConfiguracao)
        {
            if (!IsInicializado)
            {
                try { Mapper.Initialize(expressaoConfiguracao); Mapeador = Mapper.Instance; }
                catch { }
                IsInicializado = true;
            }
        }
    }
}
