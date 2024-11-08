using System.Transactions;

namespace TechSolutions.Web.Models;

public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public DateTime DataCadastro { get; set; }


    // Relacionamento com Transações
    public List<Transaction> Transacoes { get; set; } = new List<Transaction>();
}
