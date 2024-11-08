using System.Transactions;

namespace TechSolutions.Web.Models;


public class Transacoes
{
    public int TransacaoId { get; set; }
    public int ClienteId { get; set; }  // Chave estrangeira
    public decimal Valor { get; set; }
    public DateTime DataTransacao { get; set; }
    public float? Valor_Previsto { get; set; }

    // Propriedade de navegação
    public Cliente Cliente { get; set; }
}
