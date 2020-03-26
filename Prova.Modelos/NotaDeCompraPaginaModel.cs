using Prova.Primitivos;
using System.Collections.Generic;

namespace Prova.Modelos
{
    public partial class NotaDeCompraPaginaModel
    {
        public AcaoUsuario AcaoUsuario { get; set; }
        public List<NotaDeCompraModel> NotaDeCompraModels { get; set; }
    }
}
