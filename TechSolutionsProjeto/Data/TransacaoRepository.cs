using Dapper;
using Microsoft.ML;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using TechSolutions.Web.Models;
using TechSolutionsProjeto.Models;

namespace TechSolutions.Web.Repositories
{
    public class TransacaoRepository
    {
        private readonly IDbConnection _dbConnection;

        public TransacaoRepository(IDbConnection dbConnection)
        {
            _dbConnection = new NpgsqlConnection("Host=localhost;Port=5432;Database=techsolutions;Username=postgres;Password=senha123");
        }

        public IEnumerable<Transacoes> RetornarTransacoes()
        {
            var sql = "SELECT * FROM transacoes";
            return _dbConnection.Query<Transacoes>(sql);
        }

        public IEnumerable<Transacoes> ReceberTransacoesporCliente(int clienteId)
        {
            var sql = "SELECT * FROM transacoes WHERE clienteid = @ClienteId";
            return _dbConnection.Query<Transacoes>(sql, new { ClienteId = clienteId }).ToList();
        }

        public void AdicionarTransacao(Transacoes transacao)
        {
            var sql = "INSERT INTO transacoes (clienteid, valor, datatransacao) VALUES (@ClienteId, @Valor, @DataTransacao)";
            _dbConnection.Execute(sql, transacao);
        }

        public IEnumerable<TransacaoData> ObterDadosTransacoesParaML()
        {
            var transacoes = RetornarTransacoes();
            return transacoes.Select(t => new TransacaoData
            {
                Mes = t.DataTransacao.Month,
                Ano = t.DataTransacao.Year,
                Valor = (float)t.Valor
            });
        }

        public Transacoes GetTransacaoById(int transacaoId)
        {
            var sql = "SELECT * FROM transacoes WHERE transacaoid = @TransacaoId";
            return _dbConnection.QuerySingleOrDefault<Transacoes>(sql, new { TransacaoId = transacaoId });
        }

        public void AtualizarValorPrevisto(int transacaoId, float valorPrevisto)
        {
            var sql = "UPDATE transacoes SET valor_previsto = @ValorPrevisto WHERE id = @TransacaoId";
            _dbConnection.Execute(sql, new { ValorPrevisto = valorPrevisto, TransacaoId = transacaoId });
        }


    }
}
