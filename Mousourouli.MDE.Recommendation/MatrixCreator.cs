using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace Mousourouli.MDE.Recommendation
{
    public class MatrixCreator
    {
        public Matrix Generate(TransactionManager tm, Mapping mapping, WeightingSchema BWSchema)
        {
            Matrix matrix = new Matrix(mapping.Count, mapping.Count);

            foreach (IList<TransactionItem> transaction in tm)
            {
                IList<TransactionItem> indexedTransaction = mapping.Real2IndexMapping(transaction);
                int pos = 0;
                int total_trItems = 0;

                foreach (TransactionItem trItem in indexedTransaction)
                {
                    if (pos > 0)
                    {
                        total_trItems++;
                        int posItems = 0;
                        for (int i = pos - 1; i >= 0; i--)
                        {
                            double weight = 0;
                            if (indexedTransaction[i].IsPositive)
                                posItems++;
                            else
                                continue;

                            if (trItem.IsPositive)
                                weight = BWSchema.CalculatePositiveWeight(posItems, (total_trItems + 1));
                            else
                                weight = BWSchema.CalculateNegativeWeight(posItems, (total_trItems + 1));
                            
                            matrix[indexedTransaction[i].Item, trItem.Item] += weight;
                        }
                    }
                    pos++;
                }
            }

            return NegativeItems(matrix);
        }

        public Matrix NegativeItems(Matrix input)
        {
            int rowCount = input.RowCount;
            int columnCount = input.ColumnCount;
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    if (input[i, j] < 0)
                        input[i, j] = 0;
                }
            }
            return input;
        }

    }
}
