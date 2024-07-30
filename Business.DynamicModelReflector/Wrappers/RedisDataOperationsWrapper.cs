using Business.DynamicModelReflector.Interfaces;

namespace Business.DynamicModelReflector.Wrappers
{
    public class RedisDataOperationsWrapper<TModel> : IWrapper<TModel> where TModel : class, new()
    {
        #region Fields
        IModelDataOperations _dataOperations;
        #endregion

        #region Constructors
        public RedisDataOperationsWrapper(IModelDataOperations dataOperations) =>
            _dataOperations = dataOperations;
        #endregion

        #region Public Methods
        public List<object> RetrieveTableData(TModel modelData) =>
            _dataOperations.RetrieveTableData(typeof(TModel).Name);

        public List<object> RetrieveTableData(IEnumerable<TModel> modelsData) =>
            _dataOperations.RetrieveTableData(typeof(TModel).Name);

        //public void DeleteTableData<TModel>(TModel moidel) where TModel : class, new() =>


        public void InsertTableData(TModel modelData) =>
            _dataOperations.InsertTableData(typeof(TModel).Name, modelData);

        public void InsertTableData(IEnumerable<TModel> modelsData) =>
            _dataOperations.InsertTableData(typeof(TModel).Name, modelsData);

        //public void UpdateTableData<TModel>(TModel moidel) where TModel : class, new() =>

        #endregion

        #region Private Methods

        #endregion
    }
}