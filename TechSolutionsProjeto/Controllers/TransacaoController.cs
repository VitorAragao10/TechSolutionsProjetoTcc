using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Transactions;
using TechSolutions.Web.Data;
using TechSolutions.Web.Models;
using TechSolutions.Web.Repositories;

namespace TechSolutions.Web.Controllers
{
    public class TransacaoController : Controller
    {
        private readonly TransacaoRepository _transacaoRepository;
        private readonly ClienteRepository _clienteRepository;
        private readonly MachineLearningService _machineLearningService;


        public TransacaoController(TransacaoRepository transacaoRepository, ClienteRepository clienteRepository, MachineLearningService machineLearningService)
        {
            _transacaoRepository = transacaoRepository;
            _clienteRepository = clienteRepository;
            _machineLearningService = machineLearningService;
        }


        public IActionResult Index()
        {
            var transacoes = _transacaoRepository.RetornarTransacoes();
            return View(transacoes);
        }

        public IActionResult CriarTransacao()
        {
            // Carrega a lista de clientes para exibir no dropdown
            var clientes = _clienteRepository.ObterTodosClientes(); // Certifique-se de ter este método no repositório de clientes
            ViewBag.Clientes = clientes;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdicionarTransacao(Transacoes transacao)
        {
            if (transacao != null)
            {
                // Salva a nova transação no banco de dados             
                _transacaoRepository.AdicionarTransacao(transacao);
                return RedirectToAction("Index");
            }

            return RedirectToAction("CriarTransacao");
        }

        public IActionResult Relatorio()
        {
            // Definindo o contexto de licença para uso não comercial
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var transacoes = _transacaoRepository.RetornarTransacoes();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Relatório de Transações");

                // Cabeçalho
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Data";
                worksheet.Cells[1, 3].Value = "Valor";
                worksheet.Cells[1, 3].Value = "Cliente";

                // Preenchendo as células
                int row = 2;
                foreach (var transacao in transacoes)
                {
                    var cliente = _clienteRepository.BuscarCliente(transacao.ClienteId);

                    worksheet.Cells[row, 1].Value = transacao.TransacaoId;
                    worksheet.Cells[row, 2].Value = transacao.DataTransacao.ToShortDateString();
                    worksheet.Cells[row, 3].Value = transacao.Valor;
                    worksheet.Cells[row, 3].Value = cliente.Nome;
                    row++;
                }

                // Ajustar largura das colunas
                worksheet.Cells.AutoFitColumns();

                // Retornando o arquivo Excel para download
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                string fileName = $"Relatorio_Transacoes.xlsx";
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                return File(stream, contentType, fileName);
            }
        }

        [HttpGet]
        public IActionResult Detalhes(int id)
        {
            // Busca a transação pelo ID
            var transacao = _transacaoRepository.GetTransacaoById(id);

            // Verifica se a transação existe
            if (transacao == null)
            {
                return NotFound();
            }

            // Retorna a view com os detalhes da transação
            return View(transacao);
        }

        public IActionResult TreinarModelo()
        {
            // Obter os dados de transações para treinamento
            var dadosTransacoes = _transacaoRepository.ObterDadosTransacoesParaML();

            // Treinar o modelo uma vez e salvar
            _machineLearningService.TreinarModelo(dadosTransacoes);

            return Content("Modelo treinado com sucesso.");
        }


        public IActionResult CalcularValorPrevisto(int transacaoId)
        {
            // Obter a transação específica para previsão
            var transacao = _transacaoRepository.GetTransacaoById(transacaoId);

            if (transacao == null)
            {
                return NotFound("Transação não encontrada.");
            }

            // Preparar os dados para o modelo de IA
            var dadosTransacao = new TransacaoData
            {
                Valor = (float)transacao.Valor,
                Mes = transacao.DataTransacao.Month,
                Ano = transacao.DataTransacao.Year
            };

            // Obter a previsão para essa transação
            var valorPrevisto = _machineLearningService.PredizerValor(dadosTransacao);

            // Atualizar a transação com o valor previsto
            _transacaoRepository.AtualizarValorPrevisto(transacao.TransacaoId, valorPrevisto);

            return Content("Valor previsto calculado e salvo com sucesso.");
        }


    }
}
