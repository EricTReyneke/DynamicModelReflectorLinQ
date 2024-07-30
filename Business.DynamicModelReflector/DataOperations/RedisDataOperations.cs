using Business.DynamicModelReflector.Interfaces;
using Business.DynamicModelReflector.Models;

namespace Business.DynamicModelReflector.DataOperations
{
    public class RedisDataOperations : IModelDataOperations
    {
        #region Fields
        private Dictionary<string, List<object>> _cache = new();
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        public List<object> RetrieveTableData(string key)
        {
            try
            {
                ValidateKey(key);
                object lekker = _cache[key].Last();
                return _cache[key];
            }
            catch
            {
                throw;
            }
        }

        public void DeleteTableData(string deleteStatement, IQueryBuilder queryBuilder)
        {
        }

        public List<PrimaryKeyInfo> InsertTableData(string key, object modelData)
        {
            try
            {
                if (_cache.ContainsKey(key))
                    throw new Exception("This Table already exists in the Redis.");

                _cache.Add(key, new List<object>());

                if (IsEnumerable(modelData))
                {
                    foreach (object data in (IEnumerable<object>)modelData)
                        _cache[key].Add(data);

                    return new List<PrimaryKeyInfo>();
                }

                _cache[key].Add(modelData);

                return new List<PrimaryKeyInfo>();
            }
            catch
            {
                throw;
            }
        }

        public void UpdateTableData(string updateStatement, IQueryBuilder queryBuilder)
        {
        }
        #endregion

        #region Private Methods
        public void ValidateKey(string key)
        {
            if (!_cache.ContainsKey(key))
                throw new Exception("This Table does not exist in the Redis.");
        }

        public bool IsEnumerable(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("Data Model is null.");

            return typeof(IEnumerable<object>).IsAssignableFrom(obj.GetType());
        }
        #endregion
    }
}