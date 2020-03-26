using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Prova.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        protected readonly DbContext Contexto;
        protected readonly DbSet<T> Set;


        public Repositorio(DbContext contexto)
        {
            Contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
            Set = Contexto.Set<T>();
        }

        public void Incluir(T entidade) => Set.AddRange(entidade);
        public async Task IncluirAsync(
            T entidade,
            CancellationToken tokenCancelamento = default(CancellationToken)) =>
                await Set.AddAsync(entidade, tokenCancelamento);
                
        public void Atualizar(
           params T[] entidades) =>
               Set.UpdateRange(entidades);

        public IQueryable<T> Todos(
            Expression<Func<T, bool>> predicado = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> carregamentoAntecipado = null)
        {
            IQueryable<T> query = Set;

            if (carregamentoAntecipado != null)
                query = carregamentoAntecipado(query);

            if (predicado != null)
                query = query.Where(predicado);

            return query;
        }
        public TResultado PrimeiroOuDefault<TResultado>(
            Expression<Func<T, bool>> predicado,
            Expression<Func<T, TResultado>> mapeamento,
            Func<IQueryable<T>, IOrderedQueryable<T>> ordenacao = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> carregamentoAntecipado = null) =>
                PrimeiroOuDefaultAsync<TResultado>(predicado, mapeamento, ordenacao, carregamentoAntecipado).Result;
        public T PrimeiroOuDefault(
            Expression<Func<T, bool>> predicado,
            Func<IQueryable<T>, IOrderedQueryable<T>> ordenacao = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> carregamentoAntecipado = null) =>
                PrimeiroOuDefaultAsync(predicado, ordenacao, carregamentoAntecipado).Result;
        public async Task<TResultado> PrimeiroOuDefaultAsync<TResultado>(
            Expression<Func<T, bool>> predicado,
            Expression<Func<T, TResultado>> mapeamento,
            Func<IQueryable<T>, IOrderedQueryable<T>> ordenacao = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> carregamentoAntecipado = null,
            CancellationToken tokenCancelamento = default(CancellationToken))
        {
            IQueryable<T> query = Set;

            if (carregamentoAntecipado != null)
                query = carregamentoAntecipado(query);

            if (predicado != null)
                query = query.Where(predicado);

            if (ordenacao != null)
                return await ordenacao(query).Select(mapeamento).FirstOrDefaultAsync(tokenCancelamento);
            else
                return await query.Select(mapeamento).FirstOrDefaultAsync(tokenCancelamento);
        }
        public async Task<T> PrimeiroOuDefaultAsync(
            Expression<Func<T, bool>> predicado,
            Func<IQueryable<T>, IOrderedQueryable<T>> ordenacao = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> carregamentoAntecipado = null,
            CancellationToken tokenCancelamento = default(CancellationToken))
        {
            IQueryable<T> query = Set;

            if (carregamentoAntecipado != null)
                query = carregamentoAntecipado(query);

            if (predicado != null)
                query = query.Where(predicado);

            if (ordenacao != null)
                return await ordenacao(query).FirstOrDefaultAsync(tokenCancelamento);
            else
                return await query.FirstOrDefaultAsync(tokenCancelamento);
        }
    }
}
