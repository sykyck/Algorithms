using BinaryClassification;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Starting ML context...");
        var mlContext = new MLContext();

        Console.WriteLine("Loading data...");
        IDataView dataView = mlContext.Data.LoadFromTextFile<PersonData>(
            path: "data.csv",
            hasHeader: true,
            separatorChar: ',');

        Console.WriteLine("Data loaded. Creating pipeline...");
        var pipeline = mlContext.Transforms.Categorical.OneHotEncoding("Occupation")
                        .Append(mlContext.Transforms.Concatenate("Features", "Age", "EducationNum", "Occupation"))
                        .Append(mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(
                            labelColumnName: "Label", featureColumnName: "Features"));

        Console.WriteLine("Training model...");
        var model = pipeline.Fit(dataView);
        Console.WriteLine("Model trained.");

        var predictionEngine = mlContext.Model.CreatePredictionEngine<PersonData, PersonPrediction>(model);

        var sample = new PersonData() { Age = 35, EducationNum = 10, Occupation = "Tech-support" };
        Console.WriteLine("Making prediction...");
        var prediction = predictionEngine.Predict(sample);

        Console.WriteLine($"Predicted Label: {prediction.PredictedLabel}");

        var predictions = model.Transform(dataView);
        var metrics = mlContext.BinaryClassification.Evaluate(predictions, labelColumnName: "Label");
        Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");

        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}
