namespace TestWunderMobilityCheckout.Aggregates.Products.Services
{
    /// <summary> Immutable struct (pass it for performance using in, out, ref) </summary>
    public readonly struct ProductParamsDTO
    {
        /// <summary> Immutable struct (pass it for performance using in, out, ref) </summary>
        /// <param name="id"> <see cref="Id"/> </param>
        /// <param name="productCode"> <see cref="ProductCode"/> </param>
        /// <param name="name"> <see cref="Name"/> </param>
        /// <param name="price"> <see cref="Price"/> </param>
        /// <param name="promotionalQuantity"> <see cref="PromotionalQuantity"/> </param>
        /// <param name="promotionalPrice"> <see cref="PromotionalPrice"/> </param>
        /// <param name="isDeleted"> <see cref="IsDeleted"/> </param>
        public ProductParamsDTO(long? id, string productCode, string name, float? price, long? promotionalQuantity, float? promotionalPrice, bool? isDeleted)
        {
            this.Id = id;
            this.ProductCode = productCode;
            this.Name = name;
            this.Price = price;
            this.PromotionalQuantity = promotionalQuantity;
            this.PromotionalPrice = promotionalPrice;
            this.IsDeleted = isDeleted;
        }

        /// <summary> Table unique id </summary>
        public long? Id { get; }

        /// <summary> Product code </summary>
        public string ProductCode { get; }

        /// <summary> Product name </summary>
        public string Name { get; }

        /// <summary> Product price </summary>
        public float? Price { get; }

        /// <summary> Minimum quantity to apply promotional price </summary>
        public long? PromotionalQuantity { get; }

        /// <summary> Price if buy promotional quantity or more </summary>
        public float? PromotionalPrice { get; }

        /// <summary> Soft delete mark </summary>
        public bool? IsDeleted { get; }
    }
}
