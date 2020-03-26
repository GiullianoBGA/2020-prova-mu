using Newtonsoft.Json;
using Prova.Frontend.Servicos.Interfaces;
using Prova.Modelos;
using Prova.Primitivos;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Prova.Frontend.Servicos
{
    /// <summary>
    /// Classe de serviços auxiliares das notas de compras
    /// </summary>
    public class NotaDeCompraServices : INotaDeCompraServices
    {
        private static HttpClient _httpClient;
        private static HttpClient HttpClient => _httpClient ?? (_httpClient = new HttpClient());

        public NotaDeCompraServices()
        {
        }

        /// <summary>
        /// Obtém as notas de compra por usuário
        /// </summary>
        /// <param name="idUsuario">Id do usuário</param>
        /// <param name="datainicial">data inicial de pesquisa</param>
        /// <param name="dataFinal">data final da pesquisa</param>
        /// <param name="tokenCancelamento">O token de cancelamento para a operação assíncrona/param>
        /// <returns>Uma lista de notas de compras acrescida da ação do usuário</returns>
        //public async Task<(AcaoUsuario acaoUsuario, List<NotaDeCompraModel> notaDeCompras)> ObterDadosAsync(long idUsuario, DateTime datainicial, DateTime dataFinal, CancellationToken tokenCancelamento = default)
        public async Task<NotaDeCompraPaginaModel> ObterDadosAsync(long idUsuario, DateTime datainicial, DateTime dataFinal, CancellationToken tokenCancelamento = default)
        {
            string url = $"https://localhost:44376/api/NotaDeCompra/obter-notas?idusuario={idUsuario}&datainicial={datainicial.ToString("yyyy-MM-dd")}&dataFinal={dataFinal.ToString("yyyy-MM-dd")}";
            HttpResponseMessage response = await HttpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string responseBodyAsText = await response.Content.ReadAsStringAsync();
                NotaDeCompraPaginaModel notaDeCompraPaginaModel = JsonConvert.DeserializeObject<NotaDeCompraPaginaModel>(responseBodyAsText);

                return notaDeCompraPaginaModel;
            }
            else
            {
                return new NotaDeCompraPaginaModel
                {
                    AcaoUsuario = AcaoUsuario.Vistar,
                    NotaDeCompraModels = null
                };
            }
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
                using (var client = new HttpClient())
                {
                    string url = $"registrar-visto?idusuario={idUsuario}&idnotacompra={idNotaCompra}";
                    client.BaseAddress = new Uri("https://localhost:44376/api/NotaDeCompra/");
                    var response = client.PutAsync(url, null).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Console.Write("Success");
                    }
                    else
                        Console.Write("Error");
                }

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
                using (var client = new HttpClient())
                {
                    string url = $"registrar-aprovacao?idusuario={idUsuario}&idnotacompra={idNotaCompra}";
                    client.BaseAddress = new Uri("https://localhost:44376/api/NotaDeCompra/");
                    var response = client.PutAsync(url, null).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Console.Write("Success");
                    }
                    else
                        Console.Write("Error");
                }
                return true;
            }
            catch (Exception erro)
            {
                return false;
                // Todo Giulliano: Gerar log
                // throw;
            }
        }
    }
}
