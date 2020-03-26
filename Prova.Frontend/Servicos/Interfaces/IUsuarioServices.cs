using Prova.Modelos;
using System.Threading;
using System.Threading.Tasks;

namespace Prova.Frontend.Servicos.Interfaces
{
    public interface IUsuarioServices
    {
        Task<(UsuarioModel usuario, string mensagem)> LoginAsync(string login, string senha, CancellationToken tokenCancelamento = default);
        Task<UsuarioModel> ObterAsync(long idUsuario, CancellationToken tokenCancelamento = default);
    }
}
