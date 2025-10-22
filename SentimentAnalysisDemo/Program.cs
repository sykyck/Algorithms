using Microsoft.ML;
using Microsoft.ML.Data;
using SentimentAnalysisDemo;
using System;

class Program
{
    static void Main()
    {
        // STEP 2: Create MLContext
        var mlContext = new MLContext();

        // STEP 3: Load sample data
        // ✅ Add more examples for better learning
        var samples = new[]
        {
            // Positive
            new UserReview { Text = "This product is amazing!", Label = true },
            new UserReview { Text = "I love it, works perfectly!", Label = true },
            new UserReview { Text = "Absolutely fantastic experience", Label = true },
            new UserReview { Text = "Very satisfied with the quality", Label = true },
            new UserReview { Text = "Great value for money", Label = true },
            new UserReview { Text = "Excellent service, I’m happy", Label = true },
            new UserReview { Text = "Wonderful experience overall", Label = true },
            new UserReview { Text = "Highly recommend it", Label = true },

            // Negative
            new UserReview { Text = "Terrible quality, very disappointed", Label = false },
            new UserReview { Text = "I hate it, waste of money", Label = false },
            new UserReview { Text = "Not good at all", Label = false },
            new UserReview { Text = "Worst experience ever", Label = false },
            new UserReview { Text = "The product broke after one use", Label = false },
            new UserReview { Text = "Really bad and cheap", Label = false },
            new UserReview { Text = "Poor service and rude staff", Label = false },
            new UserReview { Text = "I’m unhappy with this purchase", Label = false },
        };

        var data = mlContext.Data.LoadFromEnumerable(samples);

        // STEP 4: Build pipeline
        var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(UserReview.Text))
            .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));

        // STEP 5: Train model
        var model = pipeline.Fit(data);

        // STEP 6: Test single prediction
        var predEngine = mlContext.Model.CreatePredictionEngine<UserReview, UserReviewPrediction>(model);

        var testReview = new UserReview { Text = "The service was great and I’m happy!" };
        var result = predEngine.Predict(testReview);

        Console.WriteLine($"Text: {testReview.Text}");
        Console.WriteLine($"Prediction: {(result.Prediction ? "Positive" : "Negative")}");
        Console.WriteLine($"Probability: {result.Probability:P2}");
    }
}
