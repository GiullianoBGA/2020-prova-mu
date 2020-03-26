using System;
using System.Collections.Generic;

namespace Prova.Poco
{
    public partial class NotaDeCompra : BasePoco
    {
        public NotaDeCompra()
        {
            HistoricoAprovacoes = new HashSet<HistoricoAprovacoes>();
        }

        public DateTime DataEmissao { get; set; }
        public decimal ValorMercadorias { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal ValorTotal { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<HistoricoAprovacoes> HistoricoAprovacoes { get; set; }
    }
}