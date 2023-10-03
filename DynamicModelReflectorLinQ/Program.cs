using Business.DynamicModelReflector.Data.Model;
using Business.DynamicModelReflector.DataOperations;
using Business.DynamicModelReflector.Interfaces;
using Business.DynamicModelReflector.ModelReflectors;
using Business.DynamicModelReflector.QueryBuilders;

namespace DynamicModelGeneratorLinq
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IModelReflector sqlModelReflector = new SqlModelReflector(new SqlDataOperations(), new SqlQueryBuilder());

            IEnumerable<Players> players = new List<Players>();

            int lekker = 1;
            int lekker2 = 1;

            sqlModelReflector
                .Load(players)
                .Where(players => players.Player_Id == lekker && players.Team_Id == lekker2)
                .Execute();
        }
    }
}