namespace TestWunderMobilityCheckout.Aggregates.Customers.Services
{
    /// <summary> Immutable struct (pass it for performance using in, out, ref) </summary>
    public readonly struct CustomerParamsDTO
    {
        /// <summary> Immutable struct (pass it for performance using in, out, ref) </summary>
        /// <param name="id"> <see cref="Id"/> </param>
        /// <param name="name"> <see cref="Name"/> </param>
        /// <param name="promotionalSum"> <see cref="PromotionalSum"/> </param>
        /// <param name="promotionalDiscount"> <see cref="PromotionalDiscount"/> </param>
        /// <param name="isDeleted"> <see cref="IsDeleted"/> </param>
        public CustomerParamsDTO(long? id, string name, float? promotionalSum, float? promotionalDiscount, bool? isDeleted)
        {
            this.Id = id;
            this.Name = name;
            this.PromotionalSum = promotionalSum;
            this.PromotionalDiscount = promotionalDiscount;
            this.IsDeleted = isDeleted;
        }

        /// <summary> Table unique id </summary>
        public long? Id { get; }

        /// <summary> Client name </summary>
        public string Name { get; }

        /// <summary> Sum to activate promotion </summary>
        public float? PromotionalSum { get; }

        /// <summary> The percent of discount when the purchase price is more than PromotialnalSum  </summary>
        public float? PromotionalDiscount { get; }

        /// <summary> Soft delete mark </summary>
        public bool? IsDeleted { get; }
    }
}
