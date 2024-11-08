using Dapper.FluentMap.Dommel.Mapping;
using TechSolutions.Web.Models;

public class ClienteMap : DommelEntityMap<Cliente>
{
    public ClienteMap()
    {
        ToTable("clientes"); // Opcional: Defina o nome da tabela, se necessário
        Map(c => c.Id).IsKey(); // Define a chave primária
        Map(c => c.Nome).ToColumn("nome");
        Map(c => c.Email).ToColumn("email");
        Map(c => c.DataCadastro).ToColumn("data_cadastro");
    }
}
