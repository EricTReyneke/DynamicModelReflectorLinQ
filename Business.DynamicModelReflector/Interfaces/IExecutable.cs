using Business.DynamicModelReflector.Models;

namespace Business.DynamicModelReflector.Interfaces
{
    public interface IExecutable<TModel> where TModel : class, new()
    {
        /// <summary>
        /// Executes Database Query.
        /// </summary>
        /// <returns>Nullable PrimaryKeyInformation.</returns>
        ICollection<PrimaryKeyInfo> Execute();
    }
}