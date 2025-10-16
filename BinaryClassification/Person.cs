using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryClassification
{
    public class PersonData
    {
        [LoadColumn(0)]
        public float Age { get; set; }

        [LoadColumn(1)]
        public float EducationNum { get; set; }

        [LoadColumn(2)]
        public string Occupation { get; set; }

        [LoadColumn(3)]
        public bool Label { get; set; }  // True if income >50K
    }

    public class PersonPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool PredictedLabel { get; set; }
    }
}
