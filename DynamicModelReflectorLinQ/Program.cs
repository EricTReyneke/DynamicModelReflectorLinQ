using Business.DynamicModelReflector.Data.Model;
using Business.DynamicModelReflector.DataOperations;
using Business.DynamicModelReflector.Helpers;
using Business.DynamicModelReflector.Interfaces;
using Business.DynamicModelReflector.ModelReflectors;
using Business.DynamicModelReflector.QueryBuilders;

namespace DynamicModelGeneratorLinq
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IModelReflector sqlModelReflector = new SqlModelReflector(new SqlDataOperations(), new SqlQueryBuilder(new SqlDataOperationHelper()));

            IEnumerable<Lekker> testIdentity = new List<Lekker>()
            {
                new Lekker() { Lekker1 = "Lekker5" },
                new Lekker() { Lekker1 = "Lekker6" },
                new Lekker() { Lekker1 = "Lekker7" },
                new Lekker() { Lekker1 = "Lekker8" },
            };

            sqlModelReflector
                .Create(testIdentity)
                .Execute();
        }
    }
}