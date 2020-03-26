namespace Prova.Modelos
{
    public partial class UsuarioModel : BaseModel
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Papel { get; set; }
        public decimal ValorMinimo { get; set; }
        public decimal ValorMaximo { get; set; }
    }
}
