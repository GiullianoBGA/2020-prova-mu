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
    public class UsuarioController : BaseController
    {
        private readonly IUsuarioServices usuarioServices;

        public UsuarioController(IUnitOfWork unitOfWork, IUsuarioServices usuarioServices) : base(unitOfWork)
        {
            this.usuarioServices = usuarioServices;
        }

        /// <summary>
        /// Obtém os dados das notas de compra
        /// </summary>
        /// <param name="idUsuario">Id do usuário</param>
        /// <param name="datainicial">data inicial de pesquisa</param>
        /// <param name="dataFinal">data final da pesquisa</param>
        /// <param name="tokenCancelamento">O token de cancelamento para a operação assíncrona/param>
        /// <returns>Uma lista de notas de compras acrescida da ação do usuário</returns>
        [ProducesResponseType(typeof((AcaoUsuario, List<NotaDeCompra>)), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(string login, string senha, CancellationToken tokenCancelamento = default)
        {
            return Ok(await usuarioServices.LoginAsync(login, senha, tokenCancelamento));
        }

        /// <summary>
        /// Registra os vistos nas notas de compra
        /// </summary>
        /// <param name="idUsuario">id de usuário</param>
        /// <param name="idNotaCompra">id da nota de compra</param>
        /// <param name="tokenCancelamento">O token de cancelamento para a operação assíncrona</param>
        /// <returns>Retorna true na execução normal</returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> ObterAsync(long idUsuario, CancellationToken tokenCancelamento = default)
        {
            return Ok(await usuarioServices.ObterAsync(idUsuario, tokenCancelamento));
        }

    }
}
