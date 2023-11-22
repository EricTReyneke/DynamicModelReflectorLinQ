using Business.DynamicModelReflector.Context;
using Business.DynamicModelReflector.Executables;
using Business.DynamicModelReflector.Factories;
using Business.DynamicModelReflector.Interfaces;
using System.Collections;
using System.Reflection;
using System.Text;

namespace Business.DynamicModelReflector.ModelReflectors
{
    public class SqlModelReflector : IModelReflector
    {
        #region Fields
        /// <summary>
        /// Specific Database Operations.
        /// </summary>
        IDataOperations _dataOperations;

        /// <summary>
        /// Database specified Query Builder.
        /// </summary>
        IQueryBuilder _queryBuilder;
        #endregion

        #region Constructor
        /// <summary>
        /// Constuctor that Instantiates the SqlModelReflector and allows for injection of IDataOperations and IQueryBuilder.
        /// </summary>
        /// <param name="dataOperations">Specific Database Operations.</param>
        /// <param name="queryBuilder">Database specified Query Builder.</param>
        public SqlModelReflector(IDataOperations dataOperations, IQueryBuilder queryBuilder)
        {
            _dataOperations = dataOperations;
            _queryBuilder = queryBuilder;
        }
        #endregion

        #region Public Methods
        public IJoinFactory<TModel> Load<TModel>(TModel model) where TModel : class, new()
        {
            try
            {
                StringBuilder buildLoadQuery = new();
                IContext<TModel> sqlContext = MapContext(buildLoadQuery, model);
                buildLoadQuery.Append($"Select{_queryBuilder.AddAllColumnsIntoSelect<TModel>()} \nFrom {typeof(TModel).Name} ");
                return new SqlLoadFactory<TModel>(sqlContext);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public IJoinFactory<TModel> Load<TModel>(IEnumerable<TModel> models) where TModel : class, new()
        {
            try
            {
                StringBuilder buildLoadQuery = new();
                TModel model = new();
                IContext<TModel> sqlContext = MapContext(buildLoadQuery, models);
                buildLoadQuery.Append($"Select{_queryBuilder.AddAllColumnsIntoSelect<TModel>()} \nFrom {model.GetType().Name} ");
                return new SqlLoadFactory<TModel>(sqlContext);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public IDeleteUpdateFactory<TModel> Delete<TModel>(TModel model) where TModel : class, new()
        {
            try
            {
                StringBuilder buildLoadQuery = new();
                IContext<TModel> sqlContext = MapContext(buildLoadQuery, model);
                buildLoadQuery.Append($"Delete {typeof(TModel).Name} ");
                return new SqlDeleteUpdateFactory<TModel>(sqlContext);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public IDeleteUpdateFactory<TModel> Update<TModel>(TModel model) where TModel : class, new()
        {
            try
            {
                StringBuilder buildLoadQuery = new();
                IContext<TModel> sqlContext = MapContext(buildLoadQuery, model);
                buildLoadQuery.Append($"Update {typeof(TModel).Name} {_queryBuilder.BuildUpdateSetConditions(model)}");
                return new SqlDeleteUpdateFactory<TModel>(sqlContext);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public IExecutable<TModel> Create<TModel>(TModel model) where TModel : class, new()
        {
            try
            {
                StringBuilder buildLoadQuery = new();
                buildLoadQuery.Append($"Insert Into {typeof(TModel).Name} {_queryBuilder.BuildInsertConditions(model)}");
                IContext<TModel> sqlContext = MapContext(buildLoadQuery, model);
                return new SqlExecutable<TModel>(sqlContext);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public IExecutable<TModel> Create<TModel>(IEnumerable<TModel> models) where TModel : class, new()
        {
            try
            {
                StringBuilder buildLoadQuery = new();

                int listLen = models.Count();

                for (int i = 0; i < listLen; i++)
                    if(i != listLen - 1)
                        buildLoadQuery.Append($"Insert Into {typeof(TModel).Name} {_queryBuilder.BuildInsertConditions(models.ToList()[i])}\n");
                    else
                        buildLoadQuery.Append($"Insert Into {typeof(TModel).Name} {_queryBuilder.BuildInsertConditions(models.ToList()[i])}");

                IContext<TModel> sqlContext = MapContext(buildLoadQuery, models);
                return new SqlExecutable<TModel>(sqlContext);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Maps the SqlContext object with the relating proeprties.
        /// </summary>
        /// <typeparam name="TModel">Generic of the Poco Model</typeparam>
        /// <param name="stringBuilder">StringBuilder where the Query will be build.</param>
        /// <param name="singleModel">Poco model object.</param>
        /// <returns>Mapped SqlContext.</returns>
        private IContext<TModel> MapContext<TModel>(StringBuilder stringBuilder, TModel singleModel) where TModel : class, new()
        {
            return new SqlContext<TModel>
            {
                Model = singleModel,
                QueryBuilder = _queryBuilder,
                DataOperations = _dataOperations,
                StringBuilder = stringBuilder
            };
        }

        /// <summary>
        /// Maps the SqlContext object with the relating proeprties.
        /// </summary>
        /// <typeparam name="TModel">Generic of the Poco Model</typeparam>
        /// <param name="stringBuilder">StringBuilder where the Query will be build.</param>
        /// <param name="models">IEnummerable of Poco model objects.</param>
        /// <returns>Mapped SqlContext.</returns>
        private IContext<TModel> MapContext<TModel>(StringBuilder stringBuilder, IEnumerable<TModel> models) where TModel : class, new()
        {
            return new SqlContext<TModel>
            {
                Models = models,
                QueryBuilder = _queryBuilder,
                DataOperations = _dataOperations,
                StringBuilder = stringBuilder
            };
        }
        #endregion
    }
}