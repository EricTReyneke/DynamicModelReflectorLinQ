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

            ICollection<Lekker> lekker = new List<Lekker>();

            for (int i = 1; i < 20; i++)
                lekker.Add(new Lekker() { Id = i, Lekker1 = $"Lekker{i}" });

            sqlModelReflector
                .Create(lekker)
                .Execute();
        }
    }
}