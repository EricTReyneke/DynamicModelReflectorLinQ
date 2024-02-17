using Business.DynamicModelReflector.Models;
using System.Data;
using System.Text;

namespace Business.DynamicModelReflector.Interfaces
{
    public interface IContext<TModel> where TModel : class, new()
    {
        /// <summary>
        /// DataBase specific Query Builder.
        /// </summary>
        IQueryBuilder QueryBuilder { get; set; }

        /// <summary>
        /// String Query Builder.
        /// </summary>
        StringBuilder StringBuilder { get; set; }
        
        /// <summary>
        /// Database Operations.
        /// </summary>
        IDataOperations DataOperations { get; set; }

        /// <summary>
        /// Poco Model Object.
        /// </summary>
        TModel Model { get; set; }

        /// <summary>
        /// IEnumerable of Poco Model Objects.
        /// </summary>
        IEnumerable<TModel> Models { get; set; }

        /// <summary>
        /// Tracks primary keys info inserted into the Database.
        /// </summary>
        ICollection<PrimaryKeyInfo> PrimaryKeyCreationTracker { get; set; }

        /// <summary>
        /// Stores the names of the Joined tables.
        /// </summary>
        ICollection<string> JoinedTables { get; set; }

        /// <summary>
        /// Stores the data for the Bulk Insert.
        /// </summary>
        DataTable DataTable { get; set; }
    }
}