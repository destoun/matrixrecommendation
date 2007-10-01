using System;
using System.Collections.Generic;
using System.Text;


namespace Mousourouli.MDE.Recommendation
{
    
    public interface WeightingSchema
    {
        
        //position = distance from the current item;only for positive items (1,2,3...)
        //totalItems = number of positive items in transaction including the current item
         double CalculatePositiveWeight(double position,double totalItems);
         double CalculateNegativeWeight(double position,double totalItems);
    }

    public class BooleanWeightingSchema : WeightingSchema
    {
        #region WeightingSchema Members

        public double CalculatePositiveWeight(double position, double totalItems)
        {
            System.Diagnostics.Debug.Assert(position < totalItems);
            System.Diagnostics.Debug.Assert(position > 0);

            return 1;       
        }

        public double CalculateNegativeWeight(double position, double totalItems)
        {
            System.Diagnostics.Debug.Assert(position < totalItems);
            System.Diagnostics.Debug.Assert(position > 0);


            return -1;
        }

        #endregion
    }

    public class DistanceBasedWeightingSchema : WeightingSchema
    {
        public static readonly double aValue = 1;

        #region WeightingSchema Members

        public double CalculatePositiveWeight(double position, double totalItems)
        {
            System.Diagnostics.Debug.Assert(position < totalItems);
            System.Diagnostics.Debug.Assert(position > 0);

            return aValue / position;
            
        }

        public double CalculateNegativeWeight(double position, double totalItems)
        {
            System.Diagnostics.Debug.Assert(position < totalItems);
            System.Diagnostics.Debug.Assert(position > 0);

            if (position == 1)
                return -1;
            else
                return -aValue / (totalItems - 1);

        }

        #endregion
    }


}
