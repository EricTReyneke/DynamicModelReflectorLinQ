using Business.DynamicModelReflector.Interfaces;
using Business.DynamicModelReflector.Models;
using System.Data;
using System.Text;

namespace Business.DynamicModelReflector.Context
{
    public class SqlContext<TModel> : IContext<TModel> where TModel : class, new()
    {
        public IQueryBuilder QueryBuilder { get; set; }

        public StringBuilder StringBuilder { get; set; }

        public IQueryDataOperations DataOperations { get; set; }

        public TModel Model { get; set; }

        public IEnumerable<TModel> Models { get; set; }

        public ICollection<PrimaryKeyInfo> PrimaryKeyCreationTracker { get; set; } = new List<PrimaryKeyInfo>();

        public ICollection<string> JoinedTables { get; set; } = new List<string>();

        public DataTable DataTable { get; set; }
    }
}