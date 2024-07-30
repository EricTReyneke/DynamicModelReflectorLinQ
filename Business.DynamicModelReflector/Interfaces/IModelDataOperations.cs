using Business.DynamicModelReflector.Models;

namespace Business.DynamicModelReflector.Interfaces
{
    public interface IModelDataOperations
    {
        List<object> RetrieveTableData(string key);

        List<PrimaryKeyInfo> InsertTableData(string key, object modelData);
    }
}