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

            for (int i = 0; i < 1000; i++)
                lekker.Add(new Lekker() { Lekker1 = $"Lekker{i}" });

            ICollection<PrimaryKeyInfo> myMaat = sqlModelReflector
                .Create(lekker)
                .Execute();
        }
    }
}