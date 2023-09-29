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
            IQueryBuilder sqlQueryBuilder = new SqlQueryBuilder();
            IDataOperations sqlDataOperation = new SqlOperations();
            IModelReflector sqlModelReflector = new SqlModelReflector(sqlDataOperation,sqlQueryBuilder);

            IEnumerable<Players> players = new List<Players>();

            sqlModelReflector
                .Load(players)
                .Select(players => players.Player_Id, players => players.Player_Name, players => players.Team_Id)
                .InnerJoin(players => players.Team_Id)
                .OrderBy((players => players.Player_Id, OrderByMenu.Desc))
                .Execute();
        }
    }
}