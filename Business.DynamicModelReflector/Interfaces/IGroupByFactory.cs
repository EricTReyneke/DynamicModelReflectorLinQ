using Business.DynamicModelReflector.Data.Model;
using System.Linq.Expressions;

namespace Business.DynamicModelReflector.Interfaces
{
    public interface IGroupByFactory<TModel> where TModel : class, new()
    {
        /// <summary>
        /// Builds OrderBy condition.
        /// </summary>
        /// <param name="orderByCondition">OrderBy Expression.</param>
        /// <returns>IExecutable which allows for execution of Query.</returns>
        IExecutable<TModel> OrderBy(Expression<Func<TModel, OrderByMenu>> orderByCondition);

        /// <summary>
        /// Executes query.
        /// </summary>
        void Execute();
    }
}