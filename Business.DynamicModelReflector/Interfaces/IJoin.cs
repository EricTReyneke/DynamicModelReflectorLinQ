using System.Linq.Expressions;

namespace Business.DynamicModelReflector.Interfaces
{
    public interface IJoin<TModel> where TModel : class, new()
    {
        /// <summary>
        /// Builds Query for Left Joins.
        /// </summary>
        /// <param name="joinCondition">Left join Expression.</param>
        void LeftJoin(Expression<Func<TModel, object>> joinCondition);

        /// <summary>
        /// Builds Query for Right Joins.
        /// </summary>
        /// <param name="joinCondition">Right join Expression.</param>
        void RightJoin(Expression<Func<TModel, object>> joinCondition);

        /// <summary>
        /// Builds Query for Inner Joins.
        /// </summary>
        /// <param name="joinCondition">Inner join Expression.</param>
        void InnerJoin(Expression<Func<TModel, object>> joinCondition);
    }
}