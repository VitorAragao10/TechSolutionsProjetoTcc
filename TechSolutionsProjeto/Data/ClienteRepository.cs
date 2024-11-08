using System.Data;
using Dapper;
using Npgsql;
using TechSolutions.Web.Models;

namespace TechSolutions.Web.Data;

public class ClienteRepository
{
    private readonly IDbConnection _dbConnection;

    public ClienteRepository(IDbConnection dbConnection)
    {
        _dbConnection = new NpgsqlConnection("Host=localhost;Port=5432;Database=techsolutions;Username=postgres;Password=senha123");
    }

    public async Task<IEnumerable<Cliente>> GetAllAsync()
    {
        var sql = "SELECT * FROM Clientes";
        return await _dbConnection.QueryAsync<Cliente>(sql);
    }

    public async Task AddAsync(Cliente cliente)
    {
        var sql = "INSERT INTO Clientes (Nome, Email) VALUES (@Nome, @Email)";
        await _dbConnection.ExecuteAsync(sql, cliente);
    }

    public IEnumerable<Cliente> ObterTodosClientes()
    {
        var sql = "SELECT * FROM clientes";
        return _dbConnection.Query<Cliente>(sql).ToList();
    }

    public Cliente BuscarCliente(int Id)
    {
        var sql = "SELECT * FROM clientes WHERE id = @Id";
        return _dbConnection.QuerySingleOrDefault<Cliente>(sql, new { Id = Id });
    }

}
