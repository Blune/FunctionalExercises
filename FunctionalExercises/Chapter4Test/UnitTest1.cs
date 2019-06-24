using System.Collections.Generic;
using Chapter4;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        // 1 Implement Map for ISet<T> and IDictionary<K, T>. (Tip: start by writing down
        // the signature in arrow notation.)
        [Test]
        public void MapSetTest()
        {
            /*
             * ISet<T> -> (T -> T2) -> ISet<T2>
             */
            
            var testSet = new HashSet<int>{1, 3, 5, 7};
            var mappedSet = testSet.Map(x => x + 1);
            Assert.AreEqual(new HashSet<int>(){2,4,6,8}, mappedSet);
        }

        [Test]
        public void MapDictionaryTest()
        {
            /*
             * IDictionary<TKey, TValue> -> (KeyValuePair<TKey,TValue> -> KeyValuePair<T2Key,T2Value>) -> IDictionary<T2Key, T2Value> 
             */

            var testDictionary = new Dictionary<int, int>{{2,2},{4,4},{6,6}};
            var mappedDictionary = testDictionary.Map(x => new KeyValuePair<int, int>(x.Key, x.Value * 2));
            Assert.AreEqual(new Dictionary<int, int>{{2,4},{4,8},{6,12}}, mappedDictionary);
        }
    }
}