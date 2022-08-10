using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SyncAndAsyncSorting
{
    class Program
    {
        static void Main(string[] args)
        {
            var blockingCollection = new BlockingCollection<int>() { 1, 5, -2, 45, 22, -4, 3 };

            Stopwatch timer = new Stopwatch();
            timer.Start();
            SortingBlockingCollection(blockingCollection);
            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds.ToString());

            timer.Start();
            SortingBlockingCollectionAsync(blockingCollection);
            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds.ToString());

            var concurrentBag = new ConcurrentBag<int>() { 1, 5, -2, 45, 22, -4, 3 };

            timer.Start();
            SortingConcurrentBag(concurrentBag);
            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds.ToString());

            timer.Start();
            SortingConcurrentBagAsync(concurrentBag);
            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds.ToString());

            Console.ReadKey();
        }

        #region sync sorting of Blocking Collection
        private static int FindMinimumElementInBlockingCollection(BlockingCollection<int> _bc)
        {
            int minEl;
            int notMinEl;
            _bc.TryTake(out minEl);
            int temp = minEl;
            for (int i = 0; i <= _bc.Count; i++)
            {
                _bc.TryTake(out minEl);
                if (minEl > temp)
                {
                    minEl = temp;
                }

                _bc.TryTake(out notMinEl);
                if (notMinEl < minEl)
                    minEl = notMinEl;
                temp = minEl;
            }
            return temp;
        }

        private static void SortingBlockingCollection(BlockingCollection<int> _bc)
        {
            BlockingCollection<int> newbc = new BlockingCollection<int>();
            BlockingCollection<int> newnewbc = new BlockingCollection<int>();
            int minEl = FindMinimumElementInBlockingCollection(_bc);

            foreach (var c in _bc)
            {
                if (c != minEl)
                    newbc.Add(c);
                else
                    newnewbc.Add(c);
            }
            SortingBlockingCollection(newbc);
        }
        #endregion

        #region async sorting of Blocking Collection
        private static async Task<int> FindMinimumElementInBlockingCollectionAsync(BlockingCollection<int> _bc)
        {
            int minEl;
            int notMinEl;
            _bc.TryTake(out minEl);
            int temp = minEl;
            for (int i = 0; i <= _bc.Count; i++)
            {
                _bc.TryTake(out minEl);
                if (minEl > temp)
                {
                    minEl = temp;
                }

                _bc.TryTake(out notMinEl);

                if (notMinEl < minEl)
                    minEl = notMinEl;
                temp = minEl;
            }
            return temp;
        }

        private static async Task SortingBlockingCollectionAsync(BlockingCollection<int> _bc)
        {
            BlockingCollection<int> newbc = new BlockingCollection<int>();
            BlockingCollection<int> newnewbc = new BlockingCollection<int>();
            await Task.Run(() =>
            {
                int minEl = FindMinimumElementInBlockingCollectionAsync(_bc).Result;

                foreach (var c in _bc)
                {
                    if (c != minEl)
                        newbc.Add(c);
                    else
                        newnewbc.Add(c);
                }
                SortingBlockingCollectionAsync(newbc);
            });
        }
        #endregion

        #region sync sorting of Concurrent Bag
        private static int FindMinimumElement(ConcurrentBag<int> _cb)
        {
            int minEl;
            int notMinEl;
            _cb.TryTake(out minEl);
            int temp = minEl;
            for (int i = 0; i <= _cb.Count; i++)
            {
                _cb.TryTake(out minEl);
                if (minEl > temp)
                {
                    minEl = temp;
                }

                _cb.TryTake(out notMinEl);

                if (notMinEl < minEl)
                    minEl = notMinEl;
                temp = minEl;
            }
            return temp;
        }

        private static void SortingConcurrentBag(ConcurrentBag<int> _cb)
        {
            var sortedCB = new ConcurrentBag<int>();
            var notSortedCB = new ConcurrentBag<int>();

            int minEl = FindMinimumElement(_cb);

            foreach (var element in _cb)
            {
                if (element != minEl)
                    notSortedCB.Add(element);
                else
                    sortedCB.Add(element);
            }

            SortingConcurrentBag(notSortedCB);
        }
        #endregion

        #region async sorting of Concurrent Bag
        private static async Task<int> FindMinimumElementInConcurrentBagAsync(ConcurrentBag<int> _cb)
        {
            int minEl;
            int notMinEl;
            _cb.TryTake(out minEl);
            int temp = minEl;
            for (int i = 0; i <= _cb.Count; i++)
            {
                _cb.TryTake(out minEl);
                if (minEl > temp)
                {
                    minEl = temp;
                }

                _cb.TryTake(out notMinEl);
                
                if (notMinEl < minEl)
                    minEl = notMinEl;
                temp = minEl;
            }
            return temp;
        }

        private static async Task SortingConcurrentBagAsync(ConcurrentBag<int> _cb)
        {
            ConcurrentBag<int> newbc = new ConcurrentBag<int>();
            ConcurrentBag<int> newnewbc = new ConcurrentBag<int>();
            await Task.Run(() =>
            {
                int minEl = FindMinimumElementInConcurrentBagAsync(_cb).Result;

                foreach (var element in _cb)
                {
                    if (element != minEl)
                        newbc.Add(element);
                    else
                        newnewbc.Add(element);
                }
                SortingConcurrentBagAsync(newbc);
            });
        }
        #endregion
    }
}
