namespace Business.DynamicModelReflector.Interfaces
{
    public interface IModelReflector
    {
        /// <summary>
        /// Loads Data into Poco object.
        /// </summary>
        /// <typeparam name="TModel">Generic Poco Mdoel.</typeparam>
        /// <param name="model">Poco model object.</param>
        /// <returns>Load facotry which allowes for diffrent query operation</returns>
        IJoinFactory<TModel> Load<TModel>(TModel model) where TModel : class, new();

        /// <summary>
        /// Loads Data into Poco objects.
        /// </summary>
        /// <typeparam name="TModel">Generic Poco Mdoel.</typeparam>
        /// <param name="models">IEnnumerable Poco model objects.</param>
        /// <returns>Load facotry which allowes for diffrent query operation</returns>
        IJoinFactory<TModel> Load<TModel>(IEnumerable<TModel> models) where TModel : class, new();

        /// <summary>
        /// Deletes Data out of the database from the name of the Poco Model.
        /// </summary>
        /// <typeparam name="TModel">Generic Poco Mdoel.</typeparam>
        /// <param name="model">Poco model object.</param>
        /// <returns>IDeleteUpdate factory which allowes for diffrent query operation.</returns>
        IDeleteUpdateFactory<TModel> Delete<TModel>(TModel model) where TModel : class, new();

        /// <summary>
        /// Updates data in the database which will reflect to the Poco model.
        /// </summary>
        /// <typeparam name="TModel">Generic Poco Mdoel.</typeparam>
        /// <param name="model">Poco model object.</param>
        /// <returns>IDeleteUpdate factory which allowes for diffrent query operation.</returns>
        IDeleteUpdateFactory<TModel> Update<TModel>(TModel model) where TModel : class, new();

        /// <summary>
        /// Inserts data into the database from the Poco model provided.
        /// </summary>
        /// <typeparam name="TModel">Generic Poco Mdoel.</typeparam>
        /// <param name="model">Poco model object.</param>
        /// <returns>IExecutable which allowes for the query to be Executed.</returns>
        IExecutable<TModel> Create<TModel>(TModel model) where TModel : class, new();

        /// <summary>
        /// Inserts data into the database from the Poco models provided.
        /// </summary>
        /// <typeparam name="TModel">Generic Poco Mdoel.</typeparam>
        /// <param name="model">Poco model object.</param>
        /// <returns>IExecutable which allowes for the query to be Executed.</returns>
        IExecutable<TModel> Create<TModel>(IEnumerable<TModel> models) where TModel : class, new();
    }
}
