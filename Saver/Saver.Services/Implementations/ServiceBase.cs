using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saver.Services.Implementations
{
    /// <summary>
    /// Provides a base class with some standard
    /// service level methods used throughout the application
    /// </summary>
    public abstract class ServiceBase
    {
        /// <summary>
        /// Executes the function and orders the results by the given selector of the type
        /// </summary>
        /// <typeparam name="T">The type that should be returned</typeparam>
        /// <param name="returnValuesFunction">The function returning the values</param>
        /// <param name="orderBySelector">The selector for the order key</param>
        /// <returns>An ordered enumerable ordered by the selector</returns>
        internal IEnumerable<T> ExecuteThenOrderBy<T>(Func<IEnumerable<T>> returnValuesFunction, Func<T, object> orderBySelector)
        {
            IEnumerable<T> valuesToOrder = ExecuteWithCheck(returnValuesFunction);
            if (orderBySelector != null && valuesToOrder != null)
                valuesToOrder = valuesToOrder.OrderBy(orderBySelector);

            return valuesToOrder;
        }

        /// <summary>
        /// Executes the function and orders the results (descending) by the given selector of the type
        /// </summary>
        /// <typeparam name="T">The type that should be returned</typeparam>
        /// <param name="returnValuesFunction">The function returning the values</param>
        /// <param name="orderBySelector">The selector for the order key</param>
        /// <returns>An ordered enumerable ordered by the selector (descending)</returns>
        internal IEnumerable<T> ExecuteThenOrderByDescending<T>(Func<IEnumerable<T>> returnValuesFunction, Func<T, object> orderBySelector)
        {
            IEnumerable<T> valuesToOrder = ExecuteWithCheck(returnValuesFunction);
            if (orderBySelector != null && valuesToOrder != null)
                valuesToOrder = valuesToOrder.OrderByDescending(orderBySelector);

            return valuesToOrder;
        }

        /// <summary>
        /// Executes the function and returns the value of the 
        /// function if it is not null
        /// </summary>
        /// <typeparam name="T">The type to be returned in the enumerable</typeparam>
        /// <param name="returnValuesFunction">The function returning the enumerable</param>
        /// <returns>The enumerable of type T -- potentially unordered</returns>
        private static IEnumerable<T> ExecuteWithCheck<T>(Func<IEnumerable<T>> returnValuesFunction)
        {
            //Ensure we can order by something
            if (returnValuesFunction == null)
                throw new ArgumentNullException(nameof(returnValuesFunction), "Please provide a function returning the values to order");

            IEnumerable<T> valuesToOrder = returnValuesFunction();
            return valuesToOrder;
        }
    }
}
