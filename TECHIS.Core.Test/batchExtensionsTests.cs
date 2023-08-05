
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECHIS.Core.Modelling;

using Xunit;
using Attribute = TECHIS.Core.Modelling.Attribute;

namespace TECHIS.Core.Collections
{
    [Trait("Category", "UnitTests")]
    public class batchExtensionsTests
    {
        [Fact]
        public async Task FunctionExecution_BatchSizeFunction_Success()
        {
            int totalCount = 513;
            int batchSize = 10;

            var allItems = GetObjectSampleSet<Attribute>(totalCount);

            await Task.WhenAll(allItems.Run(Process, batchSize));

            foreach (var item in allItems)
            {
                Assert.NotNull(item.Name);
            }

            Task Process(IEnumerable<Attribute> workerResults)
            {
                foreach (var worker in workerResults)
                {
                    worker.Name = DateTime.UtcNow.ToLongTimeString();
                }
                return Task.CompletedTask;
            }
        }

        [Fact]
        public async Task FunctionExecution_BatchCountFunction_Success()
        {
            int totalCount = 513;
            int batchCount = 2;

            var allItems = GetObjectSampleSet<Attribute>(totalCount);

            await Task.WhenAll(allItems.Run(Process, null, batchCount));

            foreach (var item in allItems)
            {
                Assert.NotNull(item.Name);
            }

            Task Process(IEnumerable<Attribute> workerResults)
            {
                foreach (var worker in workerResults)
                {
                    worker.Name = DateTime.UtcNow.ToLongTimeString();
                }
                return Task.CompletedTask;
            }
        }

        [Fact]
        public void MergeList_Merge_Success()
        {
            //int totalCount = 513;
            int batchSize = 10;
            int lastBatchSize = 3;

            var batch1 = GetObjectSampleSet<Attribute>(batchSize);
            var batch2 = GetObjectSampleSet<Attribute>(batchSize);
            var batch3 = GetObjectSampleSet<Attribute>(lastBatchSize);

            int totalCount = batchSize + batchSize + lastBatchSize;

            var listOfBatches = new List<IList<Attribute>> { batch1, batch2, batch3 };

            var allItems = listOfBatches.Merge();

            Assert.InRange(allItems.Count, totalCount, totalCount);
        }
        [Fact]
        public void FunctionExecution_BatchBySize_Success()
        {
            int totalCount = 513;
            int batchSize = 10;

            var batches = GetObjectSampleSet<Attribute>(totalCount).CreateBatchBySize(batchSize);

            int lastBatchSize = totalCount % batchSize;
            int batchCount = totalCount / batchSize;
            if (lastBatchSize > 0)
            {
                batchCount = batchCount + 1; //Add the remainder batch
            }
            int lastIndex = batches.Count - 1;

            Assert.InRange(batches.Count, batchCount, batchCount);

            //Confirm batch sizes
            for (int i = 0; i < batches.Count; i++)
            {
                if (i != lastIndex)
                {
                    Assert.InRange(batches[i].Count, batchSize, batchSize);
                }
                else
                {
                    Assert.InRange(batches[i].Count, lastBatchSize, lastBatchSize);
                }
            }

        }
        //[Fact]
        //public void FunctionExecution_BatchByCount_Success()
        //{
        //    int totalCount = 247;
        //    int batchCount = 3;

        //    var batches = GetObjectSampleSet<WorkerResult>(totalCount).CreateBatches(batchCount);

        //    int batchSize = totalCount / batchCount;

        //    int lastBatchSize = (totalCount / batchCount) + (totalCount % batchCount);

        //    int lastIndex = batches.Count - 1;

        //    Assert.InRange(batches.Count, batchCount, batchCount);

        //    //Confirm batch sizes
        //    for (int i = 0; i < batches.Count; i++)
        //    {
        //        if (i != lastIndex)
        //        {
        //            Assert.InRange(batches[i].Count, batchSize, batchSize);
        //        }
        //        else
        //        {
        //            Assert.InRange(batches[i].Count, lastBatchSize, lastBatchSize);
        //        }
        //    }

        //}

        [Fact]
        public void CreateBatches_Void_Success()
        {
            int totalCount = 247;
            int batchCount = 3;
            int indexStart = 1;

            var sampleSet = GetObjectSampleSet(totalCount, indexStart);
            var batches = sampleSet.CreateBatches(batchCount);

            ConcurrentDictionary<int, int> instanceCounter = new ConcurrentDictionary<int, int>();

            for (int i = 0; i < sampleSet.Count; i++)
            {
                var obj = sampleSet[i];
                batches.ForEach(batch =>
                {
                    var id = Convert.ToInt32(obj.Id);
                    var currentCount = instanceCounter.GetOrAdd( id, 0);
                    instanceCounter[id] = currentCount + batch.Count(p => p == obj);

                });
            }

            //Ensure object is in 1 and only 1 batch
            foreach (var kvp in instanceCounter)
            {
                Assert.InRange(kvp.Value, 1, 1);
            }
        }
        private List<T> GetObjectSampleSet<T>(int count) where T : new()
        {
            List<T> outputList = new List<T>(count);
            for (int i = 0; i < count; i++)
            {
                outputList.Add(new T());
            }

            return outputList;
        }
        private List<Attribute> GetObjectSampleSet(int count, int offSet = 0)
        {
            List<Attribute> outputList = new List<Attribute>(count);
            for (int i = 0; i < count; i++)
            {
                outputList.Add(new Attribute() { Name=DateTime.UtcNow.ToLongTimeString(),Id=((i + offSet).ToString()).ToString() });
            }

            return outputList;
        }
    }
}
