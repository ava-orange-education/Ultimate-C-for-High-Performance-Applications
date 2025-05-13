using System.Collections.Concurrent;

namespace DataTransformation
{
    class DataStore
    {
        private ConcurrentBag<StandardData> dataStore =
            new ConcurrentBag<StandardData>();

        public void StoreData(StandardData data)
        {
            dataStore.Add(data);
        }

        public IEnumerable<StandardData> GetElements(int count)
        {
            return dataStore.Take(count);
        }
    }
}
