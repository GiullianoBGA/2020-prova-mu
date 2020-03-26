using System;

namespace Prova.Modelos
{
    public partial class NotaDeCompraModel : BaseModel
    {
        public DateTime DataEmissao { get; set; }
        public decimal ValorMercadorias { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal ValorTotal { get; set; }
        public bool Status { get; set; }
    }
}
