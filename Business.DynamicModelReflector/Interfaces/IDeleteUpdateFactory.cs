using System.Linq.Expressions;

namespace Business.DynamicModelReflector.Interfaces
{
    public interface IDeleteUpdateFactory<TModel> where TModel : class, new()
    {
        /// <summary>
        /// Builds the Where Conditions.
        /// </summary>
        /// <param name="whereCondition">Where condtion Expression.</param>
        /// <returns></returns>
        IExecutable<TModel> Where(Expression<Func<TModel, bool>> whereCondition);

        /// <summary>
        /// Executes query.
        /// </summary>
        void Execute();
    }
}