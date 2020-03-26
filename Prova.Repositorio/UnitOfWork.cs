using Microsoft.EntityFrameworkCore;
using Prova.Poco;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Prova.Repositorio
{
    public class UnitOfWork<TContexto> : IUnitOfWork<TContexto>, IUnitOfWork
        where TContexto : DbContext
    {
        private readonly TContexto ContextoInterno;
        public TContexto Contexto => ContextoInterno;

        #region Repositorios

        private Repositorio<Configuracoes> configuracoesRepositorio = null;
        public IRepositorio<Configuracoes> ConfiguracoesRepositorio
        {
            get
            {
                if (configuracoesRepositorio == null)
                {
                    configuracoesRepositorio = new Repositorio<Configuracoes>(ContextoInterno);
                }
                return configuracoesRepositorio;
            }
        }

        private Repositorio<HistoricoAprovacoes> historicoAprovacoesRepositorio = null;
        public IRepositorio<HistoricoAprovacoes> HistoricoAprovacoesRepositorio
        {
            get
            {
                if (historicoAprovacoesRepositorio == null)
                {
                    historicoAprovacoesRepositorio = new Repositorio<HistoricoAprovacoes>(ContextoInterno);
                }
                return historicoAprovacoesRepositorio;
            }
        }

        private Repositorio<NotaDeCompra> notaDeCompraRepositorio = null;
        public IRepositorio<NotaDeCompra> NotaDeCompraRepositorio
        {
            get
            {
                if (notaDeCompraRepositorio == null)
                {
                    notaDeCompraRepositorio = new Repositorio<NotaDeCompra>(ContextoInterno);
                }
                return notaDeCompraRepositorio;
            }
        }

        private Repositorio<Usuario> usuarioRepositorio = null;
        public IRepositorio<Usuario> UsuarioRepositorio
        {
            get
            {
                if (usuarioRepositorio == null)
                {
                    usuarioRepositorio = new Repositorio<Usuario>(ContextoInterno);
                }
                return usuarioRepositorio;
            }
        }

        #endregion

        #region Construtor

        #region Construtor

        public UnitOfWork(TContexto contexto) => ContextoInterno = contexto ?? throw new ArgumentNullException(nameof(contexto), "O contexto não pode ser nulo");

        #endregion

        public void Commit()
        {
            ContextoInterno.SaveChanges();
        }

        #endregion

        #region IUnitOfWork<TContexto>

        public int Persistir() => PersistirAsync().Result;
        public async Task<int> PersistirAsync(CancellationToken tokenCancelamento = default(CancellationToken)) =>
            await ContextoInterno.SaveChangesAsync(tokenCancelamento);

        public IEnumerable<TElemento> Obter<TElemento>(
            string sql) where TElemento : class =>
                Contexto.Set<TElemento>().FromSqlRaw(sql);

        #endregion


        #region IDisposable

        private bool disposedValue = false; // Para detectar chamadas redundantes
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ContextoInterno.Dispose();
                }

                disposedValue = true;
            }
        }

        // Código adicionado para implementar corretamente o padrão descartável.
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion       
    }
}
