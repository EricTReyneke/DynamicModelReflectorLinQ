using System.Data;

namespace Business.DynamicModelReflector.Interfaces
{
    public interface IDataOperations
    {
        /// <summary>
        /// Retrieves Data from the Database with the Query Provided.
        /// </summary>
        /// <param name="selectStatmentString">Select Query.</param>
        /// <param name="queryBuilder">queryBuilder to recieve the condition parameters.</param>
        /// <returns>DataTable which will be filled with the data recieves form the Query.</returns>
        DataTable RetrieveTableData(string selectStatmentString, IQueryBuilder queryBuilder);

        /// <summary>
        /// Deletes data from the Database using the Delete Query provided.
        /// </summary>
        /// <param name="deleteStatement">Delete query.</param>
        /// <param name="queryBuilder">queryBuilder to recieve the condition parameters.</param>
        void DeleteTableData(string deleteStatement, IQueryBuilder queryBuilder);

        /// <summary>
        /// Inserts data into the Database using the Insert Query provided.
        /// </summary>
        /// <param name="insertStatement">Insert Query.</param>
        /// <param name="queryBuilder">queryBuilder to recieve the condition parameters.</param>
        void InsertTableData(string insertStatement, IQueryBuilder queryBuilder);

        void BulkInsert<TModel>(DataTable dataTable);

        /// <summary>
        /// Updates data in the Database using the Update Query provided.
        /// </summary>
        /// <param name="updateStatement">Update Query.</param>
        /// <param name="queryBuilder">queryBuilder to recieve the condition parameters.</param>
        void UpdateTableData(string updateStatement, IQueryBuilder queryBuilder);
    }
}