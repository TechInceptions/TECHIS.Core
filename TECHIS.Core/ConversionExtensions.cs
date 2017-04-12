using System;
using System.Collections.Generic;
using System.Text;

namespace TECHIS.Core
{
    public static class ConversionExtensions
    {
        public static List<TResult> ConvertAll<TInput, TResult>(this IList<TInput> inputList, Func<TInput,TResult> converter )
        {
            List<TResult> outputList;
            if (inputList == null)
            {
                outputList = null;
            }
            else
            {
                outputList = new List<TResult>(inputList.Count);
                for (int i = 0; i < inputList.Count; i++)
                {
                    outputList[i] = converter(inputList[i]);
                }
            }

            return outputList;
        }
    }
}
