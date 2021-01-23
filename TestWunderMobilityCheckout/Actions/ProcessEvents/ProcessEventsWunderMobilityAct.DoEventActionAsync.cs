using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Adds.Converters;
using CommonConstants.EventIds;
using CommonTypes.EventDTOs;
using CommonTypes.EventDTOs.Adds;
using Communication.EventDataService;
using Newtonsoft.Json;
using TestWunderMobilityCheckout.Aggregates.Products.Services;
using ValidationStatus;

namespace TestWunderMobilityCheckout.Actions.ProcessEvents
{
    public partial class ProcessEventsWunderMobilityAct
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
                        case (int)ListOfIds.TestWunderMobilityProductsQuery:
                            await this.queryProductAsync(eventJson.Value, eventJson.Key);
                            break;
                        case (int)ListOfIds.TestWunderMobilityDelete:
                            await this.deleteProductAsync(eventJson.Value, eventJson.Key);
                            break;
                        case (int)ListOfIds.TestWunderMobilityCreate:
                            await this.createProductAsync(eventJson.Value, eventJson.Key);
                            break;
                        default:
                            throw new ArgumentException("Unknown type of event");
                    }
                }
            }
        }

        private async Task queryProductAsync(string eventJson, long eventKey)
        {
            var eventDTO = JsonConvert.DeserializeObject<TestWunderMobilityCheckoutQuery>(eventJson);

            var foundProducts = new List<ProductParamsDTO>();

            // if list is empty but we pass empty list it will filter against empty list
            if (eventDTO.ProductCodeList != null && eventDTO.ProductCodeList.Count > 0)
                foundProducts = await this._productsFactory.ReadFilteredAsync(eventDTO.ProductCodeList);
            else
                foundProducts = await this._productsFactory.ReadFilteredAsync();

            var foundDTOs = new List<WunderMobilityProduct>();
            foreach (var item in foundProducts)
            {
                foundDTOs.Add(new WunderMobilityProduct
                {
                    Id = item.Id,
                    ProductCode = item.ProductCode,
                    Name = item.Name,
                    Price = item.Price,
                    PromotionalQuantity = item.PromotionalQuantity,
                    PromotionalPrice = item.PromotionalPrice,
                });
            }

            var responseEvent = new TestWunderMobilityCheckoutQueryResults()
            {
                EventId = (int)ListOfIds.TestWunderMobilityProductsQueryResults,
                SourceName = this.currentAssemblyName,
                SourceId = this.currentSourceId,
                EventLevel = (int)CommonTypes.Enums.Events.Level.Information,
                ParentId = eventDTO.AggregateId,
                Comment = "Successfully parsed",
                DataList = foundDTOs,
                TotalRecordsAmount = foundDTOs.Count,
                UserId = eventDTO.UserId,
            };

            var json = JsonConverters.SerializeJson(responseEvent);

            this._eventDataFactory.RegisterEvent(new EventDataParamsDTO(null, true, null, eventDTO.AggregateId, json));
            this._eventDataFactory.MarkEventDone(eventKey);
            await this._eventDataFactory.DBContext.SaveChangesAsync();
        }

        private async Task deleteProductAsync(string eventJson, long eventKey)
        {
            var eventDTO = JsonConvert.DeserializeObject<CommonListOfIds>(eventJson);

            foreach (var item in eventDTO.Ids)
            {
                await this._productsFactory.DeleteAsync(item);
            }

            var actionStatus = new ValidationStatusHandler()
            {
                Message = "Successfully deleted",
            };

            var responseEvent = this.generateCommonEvent(eventDTO.AggregateId, actionStatus, eventDTO.UserId);

            var json = JsonConverters.SerializeJson(responseEvent);

            this._eventDataFactory.RegisterEvent(new EventDataParamsDTO(null, true, null, eventDTO.AggregateId, json));
            this._eventDataFactory.MarkEventDone(eventKey);
            await this._eventDataFactory.DBContext.SaveChangesAsync();
        }

        private async Task createProductAsync(string eventJson, long eventKey)
        {
            var eventDTO = JsonConvert.DeserializeObject<TestWunderMobilityCheckoutCreate>(eventJson);

            var actionStatus = this._productsFactory.Create(new ProductParamsDTO(
                null,
                eventDTO.ProductCode,
                eventDTO.Name,
                eventDTO.Price,
                eventDTO.PromotionalQuantity,
                eventDTO.PromotionalPrice,
                false));

            var responseEvent = this.generateCommonEvent(eventDTO.AggregateId, actionStatus, eventDTO.UserId);

            var json = JsonConverters.SerializeJson(responseEvent);

            this._eventDataFactory.RegisterEvent(new EventDataParamsDTO(null, true, null, eventDTO.AggregateId, json));
            this._eventDataFactory.MarkEventDone(eventKey);
            await this._eventDataFactory.DBContext.SaveChangesAsync();
        }

        private CommonEvents generateCommonEvent(long parentEventAggregateId, IValidationStatus statusReturned, long userId)
        {
            // Generate event completed event (link to parent event AggregateId)
            var generatedEvent = new CommonEvents()
            {
                EventId = (int)ListOfIds.EventProccessedSuccessfully,
                SourceName = this.currentAssemblyName,
                SourceId = this.currentSourceId,
                EventLevel = (int)CommonTypes.Enums.Events.Level.Information,
                ParentId = parentEventAggregateId,
                Comment = statusReturned.Message + statusReturned.GetAllErrors(),
                UserId = userId,
            };

            if (statusReturned.HasErrors)
            {
                generatedEvent.EventId = (int)ListOfIds.EventProccessedWithFailes;
                generatedEvent.EventLevel = (int)CommonTypes.Enums.Events.Level.Errors;
            }

            return generatedEvent;
        }
    }
}
