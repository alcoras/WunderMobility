using System.Collections.Generic;
using System.Threading.Tasks;
using Adds.Converters;
using CommonConstants.EventIds;
using CommonTypes.EventDTOs;

namespace TestWunderMobilityCheckout.Actions.ProcessEvents
{
    public partial class ProcessEventsAct
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
                IdsTripleList = new List<long[]>()
                {
                    new long[3] { (int)ListOfIds.EventProccessedSuccessfully, 0, 0 },
                    new long[3] { (int)ListOfIds.EventProccessedWithFailes, 0, 0 },
                },
            };

            string stringJson = JsonConverters.SerializeJson(subscribeEvent);
            this._eventDataFactory.RegisterEvent(new Communication.EventDataService.EventDataParamsDTO(null, true,  null, null, stringJson));

            await this._eventDataFactory.DBContext.SaveChangesAsync();
        }
    }
}
