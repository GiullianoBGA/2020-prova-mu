using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Prova.Repositorio
{
    public interface IRepositorio<T> where T : class
    {
        /// <summary>
        /// Atualiza entidades no mecanismo de persistência.
        /// </summary>
        /// <param name="entidades">A expressão de predicado de consulta das informações.</param>
        /// <returns></returns>
        void Atualizar(
            params T[] entidades);
        /// <summary>
        /// Inclui elementos no mecanimo de persistência.
        /// </summary>
        /// <param name="entidade">A entidade para inclusão.</param>
        /// <returns></returns>
        void Incluir(T entidade);
        /// <summary>
        /// Inclui elementos no mecanismo de persistência.
        /// </summary>
        /// <param name="entidade">A entidade para inclusão.</param>
        /// <param name="tokenCancelamento">O token de cancelamento para a operação assíncrona.</param>
        /// <returns></returns>
        Task IncluirAsync(
            T entidade,
            CancellationToken tokenCancelamento = default(CancellationToken));

        /// <summary>
        /// Obtém um elemento IQueryable com a referência a todos os elementos de uma entidade.
        /// </summary>
        /// <param name="predicado">A expressão de predicado de consulta das informações.</param>
        /// <param name="carregamentoAntecipado">A expressão de carregamento antecipado (eager-load) de elementos vinculados (filhos).</param>
        /// <returns></returns>
        IQueryable<T> Todos(
            Expression<Func<T, bool>> predicado = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> carregamentoAntecipado = null);


        /// <summary>
        /// Obtém o primeiro elemento ou o seu valor padrão.
        /// </summary>
        /// <param name="predicado">A expressão de predicado de consulta das informações.</param>
        /// <param name="ordenacao">A expressão de ordenação dos elementos na consulta.</param>
        /// <param name="carregamentoAntecipado">A expressão de carregamento antecipado (eager-load) de elementos vinculados (filhos).</param>
        /// <param name="tokenCancelamento">O token de cancelamento para a operação assíncrona.</param>
        /// <returns></returns>
        Task<T> PrimeiroOuDefaultAsync(
            Expression<Func<T, bool>> predicado,
            Func<IQueryable<T>, IOrderedQueryable<T>> ordenacao = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> carregamentoAntecipado = null,
            CancellationToken tokenCancelamento = default(CancellationToken));
        /// <summary>
        /// Obtém o primeiro elemento ou o seu valor padrão.
        /// </summary>
        /// <typeparam name="TResultado">O tipo para o qual os resultados serão mapeados.</typeparam>
        /// <param name="predicado">A expressão de predicado de consulta das informações.</param>
        /// <param name="mapeamento">A expressão de mapeamento para os elementos.</param>
        /// <param name="ordenacao">A expressão de ordenação dos elementos na consulta.</param>
        /// <param name="carregamentoAntecipado">A expressão de carregamento antecipado (eager-load) de elementos vinculados (filhos).</param>
        /// <param name="tokenCancelamento">O token de cancelamento para a operação assíncrona.</param>
        /// <returns></returns>
        Task<TResultado> PrimeiroOuDefaultAsync<TResultado>(
            Expression<Func<T, bool>> predicado,
            Expression<Func<T, TResultado>> mapeamento,
            Func<IQueryable<T>, IOrderedQueryable<T>> ordenacao = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> carregamentoAntecipado = null,
            CancellationToken tokenCancelamento = default(CancellationToken));
    }
}
