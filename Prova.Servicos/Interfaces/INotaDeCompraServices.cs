using Prova.Modelos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Prova.Servicos
{
    public interface INotaDeCompraServices
    {
        Task<NotaDeCompraPaginaModel> ObterDadosAsync(long idUsuario, DateTime datainicial, DateTime dataFinal, CancellationToken tokenCancelamento = default);
        Task<bool> RegistrarAprovacaoAsync(long idUsuario, long idNotaCompra, CancellationToken tokenCancelamento = default);
        Task<bool> RegistrarVistoAsync(long idUsuario, long idNotaCompra, CancellationToken tokenCancelamento = default);
    }
}