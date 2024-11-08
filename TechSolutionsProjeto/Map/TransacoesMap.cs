using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Mapping;
using TechSolutions.Web.Models;

public class TransacaoMap : DommelEntityMap<Transacoes>
{
    public TransacaoMap()
    {
        ToTable("transacoes"); // Opcional: Defina o nome da tabela, se necessário
        Map(c => c.TransacaoId).IsKey(); // Define a chave primária
        Map(t => t.ClienteId).ToColumn("clienteid");
        Map(t => t.Valor).ToColumn("valor");
        Map(t => t.DataTransacao).ToColumn("datatransacao");
    }
}