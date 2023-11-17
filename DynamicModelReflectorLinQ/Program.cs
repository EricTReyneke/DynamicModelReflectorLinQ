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

            UserInformation userInformation = new();

            sqlModelReflector
                .Load(userInformation)
                .Select(userInformation => userInformation.Id, userInformation => userInformation.Password)
                .Where(userInformation => userInformation.Id == 1)
                .Execute();
        }
    }
}