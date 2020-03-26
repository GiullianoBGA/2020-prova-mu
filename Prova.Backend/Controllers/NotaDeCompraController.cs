using Microsoft.AspNetCore.Mvc;
using Prova.Modelos;
using Prova.Poco;
using Prova.Primitivos;
using Prova.Repositorio;
using Prova.Servicos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Prova.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotaDeCompraController : BaseController
    {
        private readonly INotaDeCompraServices notaDeCompraServices;

        public NotaDeCompraController(IUnitOfWork unitOfWork, INotaDeCompraServices notaDeCompraServices) : base(unitOfWork)
        {
            this.notaDeCompraServices = notaDeCompraServices;
        }

        [HttpGet]
        public async Task<IActionResult> Obter()
        {
            return Ok("Tudo certo!");
        }

        /// <summary>
        /// Obtém os dados das notas de compra
        /// </summary>
        /// <param name="idUsuario">Id do usuário</param>
        /// <param name="datainicial">data inicial de pesquisa</param>
        /// <param name="dataFinal">data final da pesquisa</param>
        /// <param name="tokenCancelamento">O token de cancelamento para a operação assíncrona/param>
        /// <returns>Uma lista de notas de compras acrescida da ação do usuário</returns>
        [ProducesResponseType(typeof(NotaDeCompraPaginaModel), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [HttpGet]
        [Route("obter-notas")]
        public async Task<IActionResult> ObterDados(long idUsuario, DateTime datainicial, DateTime dataFinal, CancellationToken tokenCancelamento = default)
        {
            return Ok(await notaDeCompraServices.ObterDadosAsync(idUsuario, datainicial, dataFinal, tokenCancelamento));
        }

        /// <summary>
        /// Registra os vistos nas notas de compra
        /// </summary>
        /// <param name="idUsuario">id de usuário</param>
        /// <param name="idNotaCompra">id da nota de compra</param>
        /// <param name="tokenCancelamento">O token de cancelamento para a operação assíncrona</param>
        /// <returns>Retorna true na execução normal</returns>
        [HttpPut]
        [Route("registrar-visto")]
        public async Task<IActionResult> RegistrarVisto(long idUsuario, long idNotaCompra, CancellationToken tokenCancelamento = default)
        {
            return Ok(await notaDeCompraServices.RegistrarVistoAsync(idUsuario, idNotaCompra, tokenCancelamento));
        }

        /// <summary>
        /// Registra as aprovações nas notas de compra
        /// </summary>
        /// <param name="idUsuario">id de usuário</param>
        /// <param name="idNotaCompra">id da nota de compra</param>
        /// <param name="tokenCancelamento">O token de cancelamento para a operação assíncrona</param>
        /// <returns>Retorna true na execução normal</returns>
        [HttpPut]
        [Route("registrar-aprovacao")]
        public async Task<IActionResult> RegistrarAprovacao(long idUsuario, long idNotaCompra, CancellationToken tokenCancelamento = default)
        {
            return Ok(await notaDeCompraServices.RegistrarAprovacaoAsync(idUsuario, idNotaCompra, tokenCancelamento));
        }
    }
}
