using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Adds.Converters;
using CommonConstants.EventIds;
using CommonTypes.EventDTOs;
using CommonTypes.EventDTOs.Adds;
using ValidationStatus;

namespace TestWunderMobilityCheckout.Actions.ProcessEvents
{
    public partial class ProcessEventsAct
    {
        /// <inheritdoc/>
        public async Task DoEventActionAsync()
        {
            var listOfEvents = this._eventDataFactory.GetListOfUnprocessedEvents();
            if (listOfEvents.Count != 0)
            {
                foreach (var eventJson in listOfEvents)
                {
                    var eventDTOCommonPart = JsonConverters.ProcessJsonExtractEvent<CommonForEvents>(eventJson.Value);
                    switch (eventDTOCommonPart.EventId)
                    {
                        case (int)ListOfIds.EventReceived:
                            break;
                        default:
                            throw new ArgumentException("Unknown type of event");
                    }
                }
            }
        }
    }
}
