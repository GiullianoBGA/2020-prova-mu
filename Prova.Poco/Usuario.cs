using System.Collections.Generic;

namespace Prova.Poco
{
    public partial class Usuario : BasePoco
    {
        public Usuario()
        {
            HistoricoAprovacoes = new HashSet<HistoricoAprovacoes>();
        }

        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Papel { get; set; }
        public decimal ValorMinimo { get; set; }
        public decimal ValorMaximo { get; set; }

        public virtual ICollection<HistoricoAprovacoes> HistoricoAprovacoes { get; set; }
    }
}