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

            //IEnumerable<Category> categories = new List<Category>()
            //{
            //    new Category(){ Id = 6, Name = "C6", Level = 3, NumberOfPlayers = 5, Date = DateTime.Now },
            //    new Category(){ Id = 7, Name = "C7", Level = 3, NumberOfPlayers = 5, Date = DateTime.Now },
            //    new Category(){ Id = 8, Name = "C8", Level = 3, NumberOfPlayers = 5, Date = DateTime.Now },
            //    new Category(){ Id = 9, Name = "C9", Level = 3, NumberOfPlayers = 5, Date = DateTime.Now }
            //};

            IEnumerable<Players> players = new List<Players>();

            //sqlModelReflector.Load(players).Select(players => players.Player_Id, players => players.Player_Name);

            sqlModelReflector.Load(players)
                .GroupBy((players => players.Team_Id, AggregateFunctionMenu.Avg), (players => players.Player_Id, AggregateFunctionMenu.Max))
                .OrderBy((p => p.Team_Id, OrderByMenu.Asc), (p => p.Player_Id, OrderByMenu.Desc))
                .Execute();
        }
    }
}

//Table for test cases.

//Create Table Category(
//Id Int Primary Key,
//Name VarChar(50) Not Null,
//Level Int check(Level > 0 And Level <= 3),
//NumberOfPlayers Int,
//Date DateTime)

//Insert Into Category (Id, Name, Level, NumberOfPlayers, Date)
//Values(1, 'C1', 1, 40, '2008-11-11'),
//(2, 'C2', 2, 12, '2008-11-4'),
//(3, 'C3', 2, 23, '2008-11-7'),
//(4, 'C4', 3, 53, '2008-11-1'),
//(5, 'C5', 3, 2, '2008-11-12')