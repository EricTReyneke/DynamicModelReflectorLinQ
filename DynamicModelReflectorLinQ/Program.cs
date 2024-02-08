using Business.DynamicModelReflector.Data.Model;
using Business.DynamicModelReflector.DataOperations;
using Business.DynamicModelReflector.Helpers;
using Business.DynamicModelReflector.Interfaces;
using Business.DynamicModelReflector.ModelReflectors;
using Business.DynamicModelReflector.Models;
using Business.DynamicModelReflector.QueryBuilders;

namespace DynamicModelGeneratorLinq
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IModelReflector sqlModelReflector = new SqlModelReflector(new SqlDataOperations(), new SqlQueryBuilder(new SqlDataOperationHelper()));


            IEnumerable<Category> categories = new List<Category>();
            sqlModelReflector.Load(categories).Where(category => category.TournamentId == Guid.Parse("727437D4-FDDA-4F81-B314-4F39959AA834")).Execute();
        }
    }
}