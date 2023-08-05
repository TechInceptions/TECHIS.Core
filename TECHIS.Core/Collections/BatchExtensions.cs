
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECHIS.Core.Collections
{
    public static class BatchExtensions
    {

        public static List<Task> Run<TItems>(this IEnumerable<TItems> items, Func<IEnumerable<TItems>, Task> function, int? batchSize, int? batchCount=null)
        {
            if (batchSize==null && batchCount==null)
            {
                throw new ArgumentNullException(nameof(batchCount), "Either the batchSize or the batchCount is required");
            }

            var batches = batchCount!=null? items.CreateBatches(batchCount.Value) : items.CreateBatchBySize(batchSize.Value);
            return batches.ConvertAll(p => function(p));
        }

        public static List<List<TItems>> CreateBatchBySize<TItems>(this IEnumerable<TItems> items, int batchSize = 10)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            List<List<TItems>> batches = new List<List<TItems>>();

            for (int i = 0; i < items.Count(); i = i + batchSize)
            {
                batches.Add(new List<TItems>(items.Skip(i).Take(batchSize)));
            }

            return batches;
        }
        public static List<List<TItems>> CreateBatches<TItems>(this IEnumerable<TItems> items, int batchCount)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            List<List<TItems>> batches = new List<List<TItems>>();
            int totalCount = items.Count();

            int batchSize = totalCount / batchCount;

            for (int i = 0; i < totalCount; i = i + batchSize)
            {
                batches.Add(new List<TItems>(items.Skip(i).Take(batchSize)));
            }

            if (batches.Count > 1 && batches.Count > batchCount)
            {
                var lastIndex = batches.Count - 1;
                var previousBatch = batches[lastIndex - 1];

                previousBatch.AddRange(batches[lastIndex]);
                batches.RemoveAt(lastIndex);
            }

            return batches;
        }


        public static List<TItems> Merge<TItems>(this IList<IList<TItems>> workerResults)
        {
            if (workerResults==null)
            {
                return null;
            }

            List<TItems> allItems = new List<TItems>();
            foreach (var list in workerResults)
            {
                allItems.AddRange(list);
            }

            return allItems;
        }

    }
}
