using System;
using System.Collections.Generic;

namespace DynamicProgramming
{
    class Program
    {
        static void Main(string[] args)
        {
            MatrixChainMultiplication matrixChainMultiplication = new MatrixChainMultiplication();
            int[] arrayDimensions = new int[]{ 4, 10, 3, 12, 20, 7 };
            matrixChainMultiplication.GetResult(arrayDimensions);
        }
    }
}
