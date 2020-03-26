using AutoMapper;
using Prova.Modelos;
using Prova.Poco;
using Prova.Repositorio;
using System.Threading;
using System.Threading.Tasks;

namespace Prova.Servicos
{
    /// <summary>
    /// Classe de serviços auxiliares dos usuários
    /// </summary>
    public class UsuarioServices : IUsuarioServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly TokenViewModel tokenViewModel;
        private readonly IMapper mapper;

        public UsuarioServices(IUnitOfWork unitOfWork,
                               TokenViewModel tokenViewModel,
                               IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.tokenViewModel = tokenViewModel;
            this.mapper = mapper;
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
        public async Task<UsuarioModel> LoginAsync(string login, string senha, CancellationToken tokenCancelamento = default)
        {
            Usuario usuario = await unitOfWork.UsuarioRepositorio
                                                   .PrimeiroOuDefaultAsync(x => x.Login == login && x.Senha == senha, tokenCancelamento: tokenCancelamento);


            if (usuario == null)
                return null;
            else
            {
                UsuarioModel usuarioModel = mapper.Map<UsuarioModel>(usuario);
                
                tokenViewModel.Login = usuario.Login;
                tokenViewModel.Logged = true;
                tokenViewModel.Token = usuario.Id.ToString();// Todo Giulliano: Como não será implementado o JWT neste teste, utilizarei este campo guardando o Id do Usuário //-- jwtObj.Access_token;

                return usuarioModel;
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
            Usuario usuario = await unitOfWork.UsuarioRepositorio.PrimeiroOuDefaultAsync(x => x.Id == idUsuario, tokenCancelamento: tokenCancelamento);
            if (usuario == null)
                return null;
            else
            {
                UsuarioModel usuarioModel = mapper.Map<UsuarioModel>(usuario);
                return usuarioModel;
            }
        }
    }
}
