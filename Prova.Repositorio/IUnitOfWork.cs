using Prova.Poco;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Prova.Repositorio
{
    public interface IUnitOfWork : IDisposable
    {
        public IRepositorio<Configuracoes> ConfiguracoesRepositorio { get; }
        public IRepositorio<HistoricoAprovacoes> HistoricoAprovacoesRepositorio { get; }
        public IRepositorio<NotaDeCompra> NotaDeCompraRepositorio { get; }
        public IRepositorio<Usuario> UsuarioRepositorio { get; }

        /// <summary>
        /// Persiste as mudanças enviando todas as pendências para o mecanismo de persistência.
        /// </summary>
        /// <returns>A quantidade de alterações realizadas no mecanismo de persistência.</returns>
        int Persistir();
        /// <summary>
        /// Persiste as mudanças enviando todas as pendências para o mecanismo de persistência.
        /// </summary>
        /// <param name="tokenCancelamento">O token de cancelamento para a operação assíncrona.</param>
        /// <returns>A quantidade de alterações realizadas no mecanismo de persistência.</returns>
        Task<int> PersistirAsync(
            CancellationToken tokenCancelamento = default(CancellationToken));

    }

    /// <summary>
    /// Define elementos de uma unidade de trabalho vinculada a um contexto do EntityFramework.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : Microsoft.EntityFrameworkCore.DbContext
    {
        /// <summary>
        /// Obtém o contexto de dados vinculado ao mecanismo de persistência.
        /// </summary>
        TContext Contexto { get; }
    }
}
