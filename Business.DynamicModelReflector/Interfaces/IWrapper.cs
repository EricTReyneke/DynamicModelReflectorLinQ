namespace Business.DynamicModelReflector.Interfaces
{
    public interface IWrapper<TModel> where TModel : class, new()
    {
        List<object> RetrieveTableData(TModel modelData);

        List<object> RetrieveTableData(IEnumerable<TModel> modelsData);

        void InsertTableData(TModel modelData);

        void InsertTableData(IEnumerable<TModel> modelsData);
    }
}