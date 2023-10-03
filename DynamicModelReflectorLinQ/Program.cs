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
            ValidateUserCredentails("Eras", "eiZoWdkCeTAHHyD+i1WiIlEIYhg=");
;        }

        public static bool ValidateUserCredentails(string userName, string password)
        {
            IModelReflector sqlModelReflector = new SqlModelReflector(new SqlDataOperations(), new SqlQueryBuilder());

            if (userName == null || password == null)
                return false;

            UserInformation userLogin = new();

            sqlModelReflector
                .Load(userLogin)
                .Where(userLogin => userLogin.UserName == userName && userLogin.Password == password)
                .Execute();

            if (string.IsNullOrEmpty(userLogin.UserName) || string.IsNullOrEmpty(userLogin.Password))
                return false;

            return true;
        }
    }
}