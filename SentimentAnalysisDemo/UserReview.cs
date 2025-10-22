using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentAnalysisDemo
{
    public class UserReview
    {
        public string Text { get; set; }

        // Label: true = positive, false = negative
        public bool Label { get; set; }
    }

    public class UserReviewPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        public float Probability { get; set; }
        public float Score { get; set; }
    }
}
