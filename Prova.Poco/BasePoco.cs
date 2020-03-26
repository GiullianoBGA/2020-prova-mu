namespace Prova.Poco
{
    /// <summary>
    /// Implementação padrão e elemento de abstração para todo POCO.
    /// </summary>
    public partial class BasePoco : IBasePoco
    {
        public long Id { get; set; }
    }
}