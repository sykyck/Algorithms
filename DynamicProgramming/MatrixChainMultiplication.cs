using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicProgramming
{
    public class MatrixChainMultiplication
    {
        private int[] matrixDimensionArray;
        private int[,] minCostMatrix;
        private int[,] optimalStructureIndexForMatrixRange;
        private IList<string> ungroupedMatrixList;
        public void GetResult(int[] _matrixDimensionArray)
        {
            matrixDimensionArray = _matrixDimensionArray;
            minCostMatrix = new int[(_matrixDimensionArray.Length-1), (_matrixDimensionArray.Length-1)];
            ungroupedMatrixList = new List<string>();
            for(int i=0; i< (_matrixDimensionArray.Length-1); i++)
            {
                ungroupedMatrixList.Add("M" + i.ToString());
            }
            IList<string> listToPrint = new List<string>(ungroupedMatrixList);
            optimalStructureIndexForMatrixRange = new int[(_matrixDimensionArray.Length - 1), (_matrixDimensionArray.Length - 1)];
            fillMinCostMatrix();
            string resultString = String.Join("", printMatrixMultiplicationOrderForWithMinOperations(0, (_matrixDimensionArray.Length - 2), listToPrint));
            Console.WriteLine(resultString);
        }

        public void fillMinCostMatrix()
        {
            //M(P0 X P1)=> M[0], M(P1 X P2)=> M[1], M(P2 X P3)=> M[2] ...M(PN-1 X PN)=> M[N-1]
            for(int i=0; i<= matrixDimensionArray.Length-2; i++)
            {
                for (int j = 0; j <= matrixDimensionArray.Length - 2; j++)
                {
                    if(i==j)
                    {
                        minCostMatrix[i,i] = 0;
                    }
                    else if(i>j)
                    {
                        minCostMatrix[i, j] = -1;
                    }
                    else if(i<j)
                    {
                        //MC[I, J]= MC[I,K] + MC[K+1,J] + P[I-1]*P[K]*P[J]
                        minCostMatrix[i, j] = findMinCostOfMatrixMultiplication(i, j);
                    }
                }
            }
        }

        public int findMinCostOfMatrixMultiplication(int leftMatrixIndex, int rightMatrixIndex)
        {
            //MC[I, J]= MC[I,K] + MC[K+1,J] + P[I-1]*P[K]*P[J]
            if(minCostMatrix[leftMatrixIndex, rightMatrixIndex] != 0)
            {
                return minCostMatrix[leftMatrixIndex, rightMatrixIndex];
            }

            int leftMatrixRows = matrixDimensionArray[leftMatrixIndex];
            int rightMatrixColumns = matrixDimensionArray[rightMatrixIndex+1];
            int minCost = 0;

            if (leftMatrixIndex + 1 == rightMatrixIndex)
            {
                int leftMatrixColumns = matrixDimensionArray[leftMatrixIndex + 1];
                minCost = leftMatrixRows * rightMatrixColumns * leftMatrixColumns;
            }
            else
            {
                for (int kthIndexMatrix = (leftMatrixIndex + 1); kthIndexMatrix < rightMatrixIndex; kthIndexMatrix++)
                {
                    int kthIndexMatrixColumns = matrixDimensionArray[kthIndexMatrix + 1];
                    int operationCost = findMinCostOfMatrixMultiplication(leftMatrixIndex, kthIndexMatrix) + findMinCostOfMatrixMultiplication(kthIndexMatrix + 1, rightMatrixIndex) + leftMatrixRows * kthIndexMatrixColumns * rightMatrixColumns;
                    if(minCost> operationCost || minCost==0)
                    {
                        minCost = operationCost;
                        optimalStructureIndexForMatrixRange[leftMatrixIndex, rightMatrixIndex] = kthIndexMatrix;
                    }
                }
            }

            return minCost;
        }

        public IList<string> printMatrixMultiplicationOrderForWithMinOperations(int startIndex, int endIndex, IList<string> resultString)
        {
            if(((startIndex+1) == endIndex) || (startIndex == endIndex))
            {
                return resultString;
            }

            int optimalGroupingIndexForRange = optimalStructureIndexForMatrixRange[startIndex, endIndex];

            //for left side
            string leftSideStartingMatrix = ungroupedMatrixList[startIndex];
            string leftSideEndingMatrix = ungroupedMatrixList[optimalGroupingIndexForRange];
            int leftSideGroupedStartingMatrixIndex = resultString.IndexOf(leftSideStartingMatrix);
            resultString.Insert(leftSideGroupedStartingMatrixIndex, "(");
            int leftSideGroupedEndingMatrixIndex = resultString.IndexOf(leftSideEndingMatrix);
            resultString.Insert(leftSideGroupedEndingMatrixIndex + 1, ")");
            resultString = printMatrixMultiplicationOrderForWithMinOperations(startIndex, optimalGroupingIndexForRange, resultString);

            string rightSideStartingMatrix = ungroupedMatrixList[optimalGroupingIndexForRange + 1];
            string rightSideEndingMatrix = ungroupedMatrixList[endIndex];
            int rightSideGroupedStartingMatrixIndex = resultString.IndexOf(rightSideStartingMatrix);
            resultString.Insert(rightSideGroupedStartingMatrixIndex, "(");
            int rightSideGroupedEndingMatrixIndex = resultString.IndexOf(rightSideEndingMatrix);
            resultString.Insert(rightSideGroupedEndingMatrixIndex + 1, ")");
            resultString = printMatrixMultiplicationOrderForWithMinOperations(optimalGroupingIndexForRange + 1, endIndex, resultString);

            return resultString;
        }
    }
}
