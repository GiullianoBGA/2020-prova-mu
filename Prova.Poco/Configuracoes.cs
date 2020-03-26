namespace Prova.Poco
{
    public partial class Configuracoes : BasePoco
    {
        public decimal FaixaLimite { get; set; }
        public short Vistos { get; set; }
        public short Aprovacoes { get; set; }
    }
}