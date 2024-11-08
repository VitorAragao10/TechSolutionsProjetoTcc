using System.Data;
using Dapper;
using Npgsql;
using TechSolutions.Web.Models;

namespace TechSolutions.Web.Data;

public class UsuarioRepository
{
    private readonly IDbConnection _dbConnection;

    public UsuarioRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public Usuario ObterPorLogin(string login)
    {
        var sql = "SELECT * FROM Usuario WHERE Login = @Login";
        return _dbConnection.QuerySingleOrDefault<Usuario>(sql, new { Login = login });
    }
}

