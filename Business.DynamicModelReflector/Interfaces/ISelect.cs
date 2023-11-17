using System.Linq.Expressions;

namespace Business.DynamicModelReflector.Interfaces
{
    public interface ISelect<TModel> where TModel : class, new()
    {
        /// <summary>
        /// Builds the columns which will be selected in query.
        /// </summary>
        /// <param name="selectCondition">columns to select.</param>
        void Select(params Expression<Func<TModel, object>>[] selectCondition);
    }
}