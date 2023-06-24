
namespace PresentationTrainerVisualization.Helper
{
    public static class UtilityHelper
    {

        /// <summary>
        /// Converts a jagged array to a 2d array.
        /// Author: https://highfieldtales.wordpress.com/2013/08/17/convert-a-jagged-array-into-a-2d-array/
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static double[,] ConvertJaggedTo2D(double[][] source)
        {
            double[,] result = new double[source.Length, source[0].Length];

            for (int i = 0; i < source.Length; i++)
                for (int k = 0; k < source[0].Length; k++)
                    result[i, k] = source[i][k];

            return result;
        }

    }
}
