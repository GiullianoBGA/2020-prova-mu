using Newtonsoft.Json;
using Prova.Frontend.Servicos.Interfaces;
using Prova.Modelos;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Prova.Frontend.Servicos
{
    /// <summary>
    /// Classe de serviços auxiliares dos usuários
    /// </summary>
    public class UsuarioServices : IUsuarioServices
    {
        private readonly TokenViewModel tokenViewModel;
        private static HttpClient _httpClient;
        private static HttpClient HttpClient => _httpClient ?? (_httpClient = new HttpClient());

        public UsuarioServices(TokenViewModel tokenViewModel)
        {
            this.tokenViewModel = tokenViewModel;

            if (tokenViewModel == null)
                tokenViewModel = new TokenViewModel();
        }

        /// <summary>
        /// Obtém um usuário a partir do login e senha
        /// </summary>
        /// <param name="login">login do usuário</param>
        /// <param name="senha">senha do usuário</param>
        /// <param name="tokenCancelamento">O token de cancelamento para a operação assíncrona</param>
        /// <returns></returns>
        public async Task<(UsuarioModel usuario, string mensagem)> LoginAsync(string login, string senha, CancellationToken tokenCancelamento = default)
        {
            string url = $"https://localhost:44376/api/usuario/login?login={login}&senha={senha}";
            HttpResponseMessage response = await HttpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string responseBodyAsText = await response.Content.ReadAsStringAsync();
                UsuarioModel usuarioModel = JsonConvert.DeserializeObject<UsuarioModel>(responseBodyAsText);

                if (usuarioModel == null)
                    return (null, "Usuário e/ou senha inválidos!");
                else
                {
                    tokenViewModel.Login = usuarioModel.Login;
                    tokenViewModel.Logged = true;
                    tokenViewModel.Token = usuarioModel.Id.ToString();// Todo Giulliano: Como não será implementado o JWT neste teste, utilizarei este campo guardando o Id do Usuário //-- jwtObj.Access_token;

                    return (usuarioModel, string.Empty);
                }
            }
            else
            {
                return (null, null);
            }
        }

        /// <summary>
        /// Obtém um usuário pelo id
        /// </summary>
        /// <param name="idUsuario">id do usuário para pesquisa</param>
        /// <param name="tokenCancelamento">O token de cancelamento para a operação assíncrona</param>
        /// <returns>Usuário pesquisado</returns>
        public async Task<UsuarioModel> ObterAsync(long idUsuario, CancellationToken tokenCancelamento = default)
        {
            string url = string.Empty;
            HttpResponseMessage response = await HttpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string responseBodyAsText = await response.Content.ReadAsStringAsync();
                
                UsuarioModel usuarioTemp = JsonConvert.DeserializeObject<UsuarioModel>(responseBodyAsText);
                if (usuarioTemp == null)
                    return null;
                else
                    return usuarioTemp;
            }
            else
            {
                return null;
            }
        }
    }
}
