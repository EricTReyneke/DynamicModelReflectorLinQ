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
            IModelReflector redisModelReflector = new RedisModelReflector(new RedisDataOperations());

            IEnumerable<Lekker> lekker = new List<Lekker>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Lekker1 = "Lekker1"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Lekker1 = "Lekker2"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Lekker1 = "Lekker3"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Lekker1 = "Lekker4"
                }
            };

            redisModelReflector.Create(lekker);

            redisModelReflector.Load(lekker);
        }
    }
}