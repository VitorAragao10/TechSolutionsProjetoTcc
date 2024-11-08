using Microsoft.ML;
using TechSolutions.Web.Models;

public class MachineLearningService
{
    private readonly MLContext _mlContext;
    private ITransformer _model;

    public MachineLearningService()
    {
        _mlContext = new MLContext();
    }

    public void TreinarModelo(IEnumerable<TransacaoData> dadosTransacoes)
    {

        // Carrega os dados
        var dataView = _mlContext.Data.LoadFromEnumerable(dadosTransacoes);

        // Define o pipeline de treinamento, configurando a coluna "Label" para o campo `Valor`
        var pipeline = _mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: nameof(TransacaoData.Valor))
            .Append(_mlContext.Transforms.Concatenate("Features", nameof(TransacaoData.Mes), nameof(TransacaoData.Ano)))
            .Append(_mlContext.Regression.Trainers.Sdca());

        // Treina o modelo
        _model = pipeline.Fit(dataView);
    }


    public float PredizerValor(TransacaoData novaTransacao)
    {
        if (_model == null)
        {
            throw new InvalidOperationException("O modelo de IA não foi treinado. Por favor, treine o modelo antes de fazer previsões.");
        }

        var predictionEngine = _mlContext.Model.CreatePredictionEngine<TransacaoData, TransacaoPrediction>(_model);
        var prediction = predictionEngine.Predict(novaTransacao);
        return prediction.PredictedValor;
    }


    // Método para obter o valor previsto para várias transações
    public IEnumerable<TransacaoPrediction> PredizerValores(IEnumerable<TransacaoData> transacoes)
    {
        var dataView = _mlContext.Data.LoadFromEnumerable(transacoes);
        var predictions = _model.Transform(dataView);
        return _mlContext.Data.CreateEnumerable<TransacaoPrediction>(predictions, reuseRowObject: false);
    }
}
