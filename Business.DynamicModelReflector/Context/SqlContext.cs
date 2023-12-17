using Business.DynamicModelReflector.Interfaces;
using Business.DynamicModelReflector.Models;
using System.Text;

namespace Business.DynamicModelReflector.Context
{
    public class SqlContext<TModel> : IContext<TModel> where TModel : class, new()
    {
        public IQueryBuilder QueryBuilder { get; set; }

        public StringBuilder StringBuilder { get; set; }

        public IDataOperations DataOperations { get; set; }

        public TModel Model { get; set; }

        public IEnumerable<TModel> Models { get; set; }

        public ICollection<PrimaryKeyInfo> PrimaryKeyCreationTracker { get; set; } = new List<PrimaryKeyInfo>();
    }
}