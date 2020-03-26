using Prova.Modelos;
using System.Threading;
using System.Threading.Tasks;

namespace Prova.Servicos
{
    public interface IUsuarioServices
    {
        Task<UsuarioModel> LoginAsync(string login, string senha, CancellationToken tokenCancelamento = default);
        Task<UsuarioModel> ObterAsync(long idUsuario, CancellationToken tokenCancelamento = default);
    }
}