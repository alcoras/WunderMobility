using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValidationStatus;

namespace TestWunderMobilityCheckout.Aggregates.Products.Services
{
    /// <summary>
    /// Products service
    /// </summary>
    public interface IProductsFactory
    {
        /// <summary>
        /// Create new product
        /// </summary>
        /// <param name="productParams"> Creation parameters </param>
        /// <returns> Parameters validation status </returns>
        IValidationStatus Create(in ProductParamsDTO productParams);

        /// <summary>
        /// Filtered read
        /// </summary>
        /// <param name="productCodeList"> Optional filter </param>
        /// <returns> List of products </returns>
        Task<List<ProductParamsDTO>> ReadFilteredAsync(List<string> productCodeList = null);

        /// <summary>
        /// Delete product element
        /// </summary>
        /// <param name="id"> Product table id </param>
        /// <returns> Task </returns>
        Task DeleteAsync(long id);
    }
}
