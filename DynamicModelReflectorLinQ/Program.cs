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

            IEnumerable<Lekker> lekker = new List<Lekker>()
            {
                new Lekker() { Lekker1 = "Lekker1"},
                new Lekker() { Lekker1 = "Lekker2"},
                new Lekker() { Lekker1 = "Lekker3"},
                new Lekker() { Lekker1 = "Lekker4"},
                new Lekker() { Lekker1 = "Lekker5"},
                new Lekker() { Lekker1 = "Lekker6"}
            };

            IEnumerable<PrimaryKeyInfo> primaryKeyInfos = sqlModelReflector
                                                                .Create(lekker)
                                                                .Execute();
        }
    }
}