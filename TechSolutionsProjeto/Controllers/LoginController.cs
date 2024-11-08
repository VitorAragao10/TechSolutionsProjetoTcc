using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using TechSolutions.Web.Data;
using TechSolutions.Web.Models;
using TechSolutions.Web.Repositories;
using TechSolutions.Web.ViewModels;

public class LoginController : Controller
{
    private readonly UsuarioRepository _usuarioRepository;

    public LoginController(UsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Obter o usuário pelo login
        var usuario = _usuarioRepository.ObterPorLogin(model.Login);
        if (usuario == null || !VerificarSenha(model.Senha, usuario.SenhaHash))
        {
            ModelState.AddModelError(string.Empty, "Login ou senha inválidos.");
            return View(model);
        }

        // Sucesso: redireciona para a página inicial ou outro lugar
        return RedirectToAction("Index", "Home");
    }

    private bool VerificarSenha(string senha, string senhaHash)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            var hashSenha = BitConverter.ToString(bytes).Replace("-", "").ToLower();
            return hashSenha == senhaHash;
        }
    }
}
