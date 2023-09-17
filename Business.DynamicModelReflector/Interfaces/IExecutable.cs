namespace Business.DynamicModelReflector.Interfaces
{
    public interface IExecutable<TModel> where TModel : class, new()
    {
        /// <summary>
        /// Executes Database Query.
        /// </summary>
        void Execute();
    }
}