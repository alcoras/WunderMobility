using System.Collections.Generic;
using System.Threading.Tasks;
using ValidationStatus;

namespace TestWunderMobilityCheckout.Aggregates.Customers.Services
{
    /// <summary>
    /// Customers service
    /// </summary>
    public interface ICustomersFactory
    {
        /// <summary>
        /// Create new customer
        /// </summary>
        /// <param name="customerParams"> Creation parameters </param>
        /// <returns> Parameters validation status </returns>
        IValidationStatus Create(in CustomerParamsDTO customerParams);

        /// <summary>
        /// Filtered read
        /// </summary>
        /// <param name="Name"> Optional filter on customer's name </param>
        /// <returns> List of customers </returns>
        Task<List<CustomerParamsDTO>> ReadFilteredAsync(string Name);

        /// <summary>
        /// Delete customer element
        /// </summary>
        /// <param name="id"> Customers table id </param>
        /// <returns> Task </returns>
        Task DeleteAsync(long id);
    }
}
