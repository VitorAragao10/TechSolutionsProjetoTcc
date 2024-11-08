using Microsoft.AspNetCore.Mvc;
using TechSolutions.Web.Models;
using Dapper;
using Dapper.FluentMap;
using System.Data;
using Npgsql;
using System.Transactions;

namespace TechSolutionsProjeto.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IDbConnection _dbConnection;

        // Construtor que injeta a conexão com o banco de dados
        public ClienteController()
        {
            // Altere a string de conexão conforme necessário
            _dbConnection = new NpgsqlConnection("Host=localhost;Port=5432;Database=techsolutions;Username=postgres;Password=senha123");
        }

        // GET: /Cliente
        public IActionResult Index()
        {
            var clientes = _dbConnection.Query<Cliente>("SELECT * FROM clientes").ToList();
            return View(clientes);
        }

        // GET: /Cliente/Details/
        public IActionResult Detalhes(int id)
        {
            var cliente = _dbConnection.QueryFirstOrDefault<Cliente>("SELECT * FROM clientes WHERE id = @Id", new { Id = id });
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // GET: /Cliente/CriarCliente
        public IActionResult CriarCliente()
        {
            return View();
        }

        // POST: /Cliente/CriarCliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CriarCliente(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                var sql = "INSERT INTO clientes (nome, email, data_cadastro) VALUES (@Nome, @Email, @DataCadastro)";
                _dbConnection.Execute(sql, cliente);
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: /Cliente/Editar/
        public IActionResult Editar(int id)
        {
            var cliente = _dbConnection.QueryFirstOrDefault<Cliente>("SELECT * FROM clientes WHERE id = @Id", new { Id = id });
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // POST: /Cliente/Editar/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(int id, Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                var sql = "UPDATE clientes SET nome = @Nome, email = @Email, data_cadastro = @DataCadastro WHERE id = @Id";
                _dbConnection.Execute(sql, new { cliente.Nome, cliente.Email, cliente.DataCadastro, Id = id });
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: /Cliente/Deletar/
        public IActionResult Deletar(int id)
        {
            var cliente = _dbConnection.QueryFirstOrDefault<Cliente>("SELECT * FROM clientes WHERE id = @Id", new { Id = id });
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // POST: /Cliente/DeletarCliente/
        [HttpPost, ActionName("DeletarCliente")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletarCliente(int id)
        {
            var sql = "DELETE FROM clientes WHERE id = @Id";
            _dbConnection.Execute(sql, new { Id = id });
            return RedirectToAction(nameof(Index));
        }

        // GET: /Cliente/Transacoes/{id}
        public IActionResult Transacoes(int id)
        {
            var sql = "SELECT * FROM transacoes WHERE clienteid = @Id";
            var transacoes = _dbConnection.Query<Transacoes>(sql, new { Id = id }).ToList();

            if (transacoes == null || transacoes.Count == 0)
            {
                return View("SemTransacoes");
            }

            ViewBag.ClienteId = id;  // Passa o ID do cliente para a view
            return View(transacoes);
        }

    }
}
