using System;

namespace Prova.Poco
{
    public partial class HistoricoAprovacoes : BasePoco
    {
        public DateTime Data{ get; set; }
        public long UsuarioId { get; set; }
        public long NotaDeCompraId { get; set; }
        public string Operacao { get; set; }

        public virtual NotaDeCompra NotaDeCompra { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}