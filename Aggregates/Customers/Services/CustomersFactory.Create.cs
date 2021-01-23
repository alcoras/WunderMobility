using TestWunderMobilityCheckout.Aggregates.Customers.Models;
using ValidationStatus;

namespace TestWunderMobilityCheckout.Aggregates.Customers.Services
{
    public partial class CustomersFactory
    {
        /// <inheritdoc/>
        public IValidationStatus Create(in CustomerParamsDTO customerParams)
        {
            var status = new ValidationStatusHandler()
            {
                Message = "Customer successfully created",
            };

            var customerListParams = new CustomersList.CustomerParams(
                null, customerParams.Name, customerParams.PromotionalSum, customerParams.PromotionalDiscount, customerParams.IsDeleted);

            status.CombineErrors(CustomersList.ValidateProperties(in customerListParams));

            if (status.HasErrors) return status;

            this.DBContext.CustomersList.Add(new CustomersList(in customerListParams));

            return status;
        }
    }
}
