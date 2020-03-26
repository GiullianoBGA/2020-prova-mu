using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Prova.Modelos
{
    public class LoginModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Login é obrigatório")]
        [StringLength(500, ErrorMessage = "Login muito longo")]
        public string Login { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Senha é obrigatória")]
        public string Password { get; set; }
    }
}
