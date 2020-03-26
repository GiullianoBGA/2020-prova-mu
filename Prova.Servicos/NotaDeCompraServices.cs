using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prova.Modelos;
using Prova.Poco;
using Prova.Primitivos;
using Prova.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Prova.Servicos
{
    /// <summary>
    /// Classe de serviços auxiliares das notas de compras
    /// </summary>
    public class NotaDeCompraServices : INotaDeCompraServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUsuarioServices usuarioServices;
        private readonly IMapper mapper;

        public NotaDeCompraServices(IUnitOfWork unitOfWork,
                                    IUsuarioServices usuarioServices,
                                    IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.usuarioServices = usuarioServices;
            this.mapper = mapper;
        }

        /// <summary>
        /// Obtém as notas de compra por usuário
        /// </summary>
        /// <param name="idUsuario">Id do usuário</param>
        /// <param name="datainicial">data inicial de pesquisa</param>
        /// <param name="dataFinal">data final da pesquisa</param>
        /// <param name="tokenCancelamento">O token de cancelamento para a operação assíncrona/param>
        /// <returns>Uma lista de notas de compras acrescida da ação do usuário</returns>
        public async Task<NotaDeCompraPaginaModel> ObterDadosAsync(long idUsuario, DateTime datainicial, DateTime dataFinal, CancellationToken tokenCancelamento = default)
        {
            UsuarioModel usuario = await usuarioServices.ObterAsync(idUsuario);
            AcaoUsuario acaoUsuario = usuario.Papel == "V" ? AcaoUsuario.Vistar : AcaoUsuario.Aprovar;

            List<NotaDeCompra> notaDeComprasTemp = await unitOfWork.NotaDeCompraRepositorio
                                                                   .Todos(predicado: x => x.DataEmissao.Date >= datainicial
                                                                                       && x.DataEmissao.Date <= dataFinal
                                                                                       && x.ValorTotal >= usuario.ValorMinimo
                                                                                       && x.ValorTotal <= usuario.ValorMaximo
                                                                                       && x.Status == false,
                                                                          carregamentoAntecipado: x => x.Include(v => v.HistoricoAprovacoes)
                                                                                                            .ThenInclude(u => u.Usuario))
                                                                   .ToListAsync(tokenCancelamento);

            List<NotaDeCompraModel> notaDeCompras = new List<NotaDeCompraModel>();

            notaDeComprasTemp.ForEach(x =>
            {
                if (!x.HistoricoAprovacoes.Any(h => h.Usuario.Id == idUsuario))
                {
                    StatusNotaDeCompra statusNotaDeCompra = ValidarStatusNotaDeCompra(x);
                    if (usuario.Papel == "V" && statusNotaDeCompra == StatusNotaDeCompra.Pendente)
                        notaDeCompras.Add(mapper.Map<NotaDeCompraModel>(x));
                    else if (usuario.Papel == "A" && statusNotaDeCompra == StatusNotaDeCompra.AguardandoAprovacao)
                        notaDeCompras.Add(mapper.Map<NotaDeCompraModel>(x));
                }
            });

            NotaDeCompraPaginaModel notaDeCompraPaginaModel = new NotaDeCompraPaginaModel
            {
                AcaoUsuario = acaoUsuario,
                NotaDeCompraModels = notaDeCompras
            };
            return notaDeCompraPaginaModel;
        }

        /// <summary>
        /// Registra os vistos nas notas de compra
        /// </summary>
        /// <param name="idUsuario">id de usuário</param>
        /// <param name="idNotaCompra">id da nota de compra</param>
        /// <param name="tokenCancelamento">O token de cancelamento para a operação assíncrona</param>
        /// <returns>Retorna true na execução normal</returns>
        public async Task<bool> RegistrarVistoAsync(long idUsuario, long idNotaCompra, CancellationToken tokenCancelamento = default)
        {
            try
            {
                UsuarioModel usuario = await usuarioServices.ObterAsync(idUsuario);
                NotaDeCompra notaDeCompra = await unitOfWork.NotaDeCompraRepositorio.PrimeiroOuDefaultAsync(predicado: x => x.Id == idNotaCompra,
                                                                                                            carregamentoAntecipado: x => x.Include(v => v.HistoricoAprovacoes)
                                                                                                                                            .ThenInclude(u => u.Usuario),
                                                                                                            tokenCancelamento: tokenCancelamento);

                foreach (var item in notaDeCompra.HistoricoAprovacoes)
                {
                    if (item.Usuario.Id == usuario.Id)
                        break;
                }

                HistoricoAprovacoes historicoAprovacoes = new HistoricoAprovacoes
                {
                    Data = DateTime.Now,
                    NotaDeCompra = notaDeCompra,
                    Operacao = "V",
                    Usuario = await unitOfWork.UsuarioRepositorio.PrimeiroOuDefaultAsync(x => x.Id == idUsuario, tokenCancelamento: tokenCancelamento)
                };

                await unitOfWork.HistoricoAprovacoesRepositorio.IncluirAsync(historicoAprovacoes, tokenCancelamento);

                await unitOfWork.PersistirAsync(tokenCancelamento);

                await ValidarAprovacaoNotaDeCompraAsync(idNotaCompra, tokenCancelamento);

                return true;
            }
            catch (Exception erro)
            {
                return false;
                // Todo Giulliano: Gerar log
                // throw;
            }
        }

        /// <summary>
        /// Registra as aprovações nas notas de compra
        /// </summary>
        /// <param name="idUsuario">id de usuário</param>
        /// <param name="idNotaCompra">id da nota de compra</param>
        /// <param name="tokenCancelamento">O token de cancelamento para a operação assíncrona</param>
        /// <returns>Retorna true na execução normal</returns>
        public async Task<bool> RegistrarAprovacaoAsync(long idUsuario, long idNotaCompra, CancellationToken tokenCancelamento = default)
        {
            try
            {
                UsuarioModel usuario = await usuarioServices.ObterAsync(idUsuario);
                NotaDeCompra notaDeCompra = await unitOfWork.NotaDeCompraRepositorio.PrimeiroOuDefaultAsync(predicado: x => x.Id == idNotaCompra,
                                                                                                  carregamentoAntecipado: x => x.Include(v => v.HistoricoAprovacoes)
                                                                                                                                    .ThenInclude(u => u.Usuario),
                                                                                                  tokenCancelamento: tokenCancelamento);

                foreach (var item in notaDeCompra.HistoricoAprovacoes)
                {
                    if (item.Usuario.Id == usuario.Id)
                        break;
                }

                HistoricoAprovacoes historicoAprovacoes = new HistoricoAprovacoes
                {
                    Data = DateTime.Now,
                    NotaDeCompra = notaDeCompra,
                    Operacao = "A",
                    Usuario = await unitOfWork.UsuarioRepositorio.PrimeiroOuDefaultAsync(x => x.Id == idUsuario, tokenCancelamento: tokenCancelamento)
                };

                await unitOfWork.HistoricoAprovacoesRepositorio.IncluirAsync(historicoAprovacoes, tokenCancelamento);

                await unitOfWork.PersistirAsync(tokenCancelamento);

                await ValidarAprovacaoNotaDeCompraAsync(idNotaCompra, tokenCancelamento);

                return true;
            }
            catch (Exception erro)
            {
                return false;
                // Todo Giulliano: Gerar log
                // throw;
            }
        }

        /// <summary>
        /// Valida se a nota de compra está pronta para a aprovação
        /// </summary>
        /// <param name="idNotaCompra">id da nota de compra</param>
        /// <param name="tokenCancelamento">O token de cancelamento para a operação assíncrona</param>
        /// <returns>Retorna true na execução normal</returns>
        private async Task ValidarAprovacaoNotaDeCompraAsync(long idNotaCompra, CancellationToken tokenCancelamento = default)
        {
            NotaDeCompra notaDeCompra = await unitOfWork.NotaDeCompraRepositorio.PrimeiroOuDefaultAsync(predicado: x => x.Id == idNotaCompra,
                                                                                                  carregamentoAntecipado: x => x.Include(v => v.HistoricoAprovacoes),
                                                                                                  tokenCancelamento: tokenCancelamento);

            Configuracoes configuracoes = ObterConfiguracoes(notaDeCompra.ValorTotal);

            if (notaDeCompra.HistoricoAprovacoes.Count(x => x.Operacao == "A") == configuracoes.Aprovacoes)
            {
                notaDeCompra.Status = true;
                unitOfWork.NotaDeCompraRepositorio.Atualizar(notaDeCompra);

                await unitOfWork.PersistirAsync(tokenCancelamento);
            }
        }

        /// <summary>
        /// Valida o status da nota de compra
        /// </summary>
        /// <param name="notaDeCompra">nota de compra a ser validada</param>
        /// <returns>Status da nota de compra</returns>
        private StatusNotaDeCompra ValidarStatusNotaDeCompra(NotaDeCompra notaDeCompra)
        {
            int qtdVistos = notaDeCompra.HistoricoAprovacoes.Count(x => x.Operacao == "V");
            int qtdAprovacoes = notaDeCompra.HistoricoAprovacoes.Count(x => x.Operacao == "A");

            Configuracoes configuracoes = ObterConfiguracoes(notaDeCompra.ValorTotal);

            if (qtdAprovacoes == configuracoes.Aprovacoes && qtdVistos == configuracoes.Vistos)
                return StatusNotaDeCompra.Aprovada;
            else if (qtdVistos == configuracoes.Vistos)
                return StatusNotaDeCompra.AguardandoAprovacao;
            else
                return StatusNotaDeCompra.Pendente;
        }

        /// <summary>
        /// Obtém as configurações sobre os limites de faixa, quantidade de vistos e aprovações
        /// </summary>
        /// <param name="valor">valor da pesquisa</param>
        /// <returns>Configuração referente ao valor pesquisado</returns>
        private Configuracoes ObterConfiguracoes(decimal valor)
        {
            Configuracoes configuracoes = unitOfWork.ConfiguracoesRepositorio
                                                    .Todos(x => valor <= x.FaixaLimite)
                                                    .OrderBy(o => o.FaixaLimite)
                                                    .FirstOrDefault()
                                                    ;
            return configuracoes;
        }
    }
}
