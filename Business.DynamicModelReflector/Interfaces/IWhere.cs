using System.Linq.Expressions;

namespace Business.DynamicModelReflector.Interfaces
{
    public interface IWhere<TModel> where TModel : class, new()
    {
        /// <summary>
        /// Builds the Where Conditions.
        /// </summary>
        /// <param name="whereCondition">Where Condition Expression.</param>
        /// <returns>IWhereFactory for query manipulation.</returns>
        void Where(Expression<Func<TModel, bool>> whereCondition);
    }
}