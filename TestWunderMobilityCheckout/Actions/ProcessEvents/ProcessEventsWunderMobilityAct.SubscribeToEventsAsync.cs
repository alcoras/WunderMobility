using System.Collections.Generic;
using System.Threading.Tasks;
using Adds.Converters;
using CommonConstants.EventIds;
using CommonTypes.EventDTOs;
using TestWunderMobilityCheckout.Aggregates.Customers.Services;
using TestWunderMobilityCheckout.Aggregates.Products.Services;

namespace TestWunderMobilityCheckout.Actions.ProcessEvents
{
    public partial class ProcessEventsWunderMobilityAct
    {
        /// <inheritdoc/>
        public async Task SubscribeToEventsAsync()
        {
            // Generate event completed event (link to parent event AggregateId)
            var subscribeEvent = new SubscribeToEvents()
            {
                EventId = (int)ListOfIds.Subscribe,
                SourceName = this.currentAssemblyName,
                SourceId = this.currentSourceId,
                EventLevel = (int)CommonTypes.Enums.Events.Level.Information,
                Comment = "Subscribe to initial events",
                CleanSubscriptionList = true,
                IdsTripleList = new List<long[]>()
                {
                    new long[3] { (int)ListOfIds.EventProccessedSuccessfully, 0, 0 },
                    new long[3] { (int)ListOfIds.EventProccessedWithFailes, 0, 0 },
                },
            };

            //string stringJson = JsonConverters.SerializeJson(subscribeEvent);
            //this._eventDataFactory.RegisterEvent(new Communication.EventDataService.EventDataParamsDTO(null, true,  null, null, stringJson));

            //await this._eventDataFactory.DBContext.SaveChangesAsync();

            // Fill test data
            var productTable = await this._productsFactory.ReadFilteredAsync();
            if (productTable.Count == 0)
            {
                this._productsFactory.Create(new ProductParamsDTO(null, "001", "Curry Sauce", 1.95F, null, null, false));
                this._productsFactory.Create(new ProductParamsDTO(null, "002", "Pizza", 5.99F, 2, 3.99F, false));
                this._productsFactory.Create(new ProductParamsDTO(null, "003", "Men's T-Shirt", 25F, null, null, false));
            }

            var customersTable = await this._customersFactory.ReadFilteredAsync();
            if (customersTable.Count == 0)
            {
                this._customersFactory.Create(new CustomerParamsDTO(null, "All", 30, 10, false));
            }

            await this._eventDataFactory.DBContext.SaveChangesAsync();
        }
    }
}
