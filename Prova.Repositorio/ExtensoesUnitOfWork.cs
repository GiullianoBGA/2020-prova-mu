using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Prova.Repositorio
{
    public static class ExtensoesUnitOfWork
    {
        #region Métodos

        /// <summary>
        /// Registra uma unidade de trabalho para um tipo de contexto específico.
        /// </summary>
        /// <typeparam name="TContext">O tipo de contexto.</typeparam>
        /// <param name="servicos"></param>
        /// <returns></returns>
        public static IServiceCollection RegistrarUnitOfWork<TContext>(this IServiceCollection servicos)
            where TContext : DbContext
        {
            return servicos
                .AddTransient<IUnitOfWork, UnitOfWork<TContext>>()
                .AddTransient<IUnitOfWork<TContext>, UnitOfWork<TContext>>();
        }

        #endregion
    }
}
