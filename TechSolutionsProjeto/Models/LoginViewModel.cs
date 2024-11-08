using System.ComponentModel.DataAnnotations;

namespace TechSolutions.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Login é obrigatório.")]
        public string Login { get; set; }  // Alterado de Email para Login

        [Required(ErrorMessage = "Senha é obrigatória.")]
        public string Senha { get; set; }
    }
}
