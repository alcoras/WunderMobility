using TestWunderMobilityCheckout.Aggregates.Products.Models;
using ValidationStatus;

namespace TestWunderMobilityCheckout.Aggregates.Products.Services
{
    public partial class ProductsFactory
    {
        /// <inheritdoc/>
        public IValidationStatus Create(in ProductParamsDTO productParams)
        {
            var status = new ValidationStatusHandler()
            {
                Message = "Product successfully created",
            };

            var productListParams = new ProductList.ProductParams(
                null, productParams.ProductCode, productParams.Name, productParams.Price, productParams.PromotionalQuantity, productParams.PromotionalPrice, productParams.IsDeleted);

            status.CombineErrors(ProductList.ValidateProperties(in productListParams));

            if (status.HasErrors) return status;

            this.DBContext.ProductList.Add(new ProductList(in productListParams));

            return status;
        }
    }
}
