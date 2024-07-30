using Business.DynamicModelReflector.Interfaces;
using Business.DynamicModelReflector.Wrappers;
using System.Reflection;

namespace Business.DynamicModelReflector.ModelReflectors
{
    public class RedisModelReflector : IModelReflector
    {
        #region Fields
        IModelDataOperations _dataOperations;
        #endregion

        #region Constructors
        public RedisModelReflector(IModelDataOperations dataOperations) =>
            _dataOperations = dataOperations;
        #endregion

        #region Public Methods
        public IExecutable<TModel> Create<TModel>(TModel model) where TModel : class, new()
        {
            IWrapper<TModel> wrapper = new RedisDataOperationsWrapper<TModel>(_dataOperations);
            wrapper.InsertTableData(model);

            return null;
        }

        public IExecutable<TModel> Create<TModel>(IEnumerable<TModel> models) where TModel : class, new()
        {
            IWrapper<TModel> wrapper = new RedisDataOperationsWrapper<TModel>(_dataOperations);
            wrapper.InsertTableData(models);

            return null;
        }

        public IDeleteUpdateFactory<TModel> Delete<TModel>(TModel model) where TModel : class, new()
        {
            throw new NotImplementedException();
        }

        public IJoinFactory<TModel> Load<TModel>(TModel model) where TModel : class, new()
        {
            IWrapper<TModel> wrapper = new RedisDataOperationsWrapper<TModel>(_dataOperations);
            wrapper.RetrieveTableData(model);

            return null;
        }

        public IJoinFactory<TModel> Load<TModel>(IEnumerable<TModel> models) where TModel : class, new()
        {
            IWrapper<TModel> wrapper = new RedisDataOperationsWrapper<TModel>(_dataOperations);
            List<object> lekker = wrapper.RetrieveTableData(models);

            return null;
        }

        public IDeleteUpdateFactory<TModel> Update<TModel>(TModel model) where TModel : class, new()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
