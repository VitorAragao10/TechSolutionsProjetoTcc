using Dapper.FluentMap.Dommel.Mapping;
using TechSolutions.Web.Models;

namespace TechSolutionsProjeto.Map
{
    public class UsuarioMap : DommelEntityMap<Usuario>
    {
        public UsuarioMap()
        {
            ToTable("clientes");
            Map(u => u.UsuarioId).IsKey();
            Map(u => u.Nome).ToColumn("Nome");
            Map(u => u.Email).ToColumn("Email");
            Map(u => u.SenhaHash).ToColumn("SenhaHash");
        }
    }
}
