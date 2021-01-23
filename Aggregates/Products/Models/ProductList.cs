using System;
using ValidationStatus;

namespace TestWunderMobilityCheckout.Aggregates.Products.Models
{
    /// <summary> List of products </summary>
    public class ProductList : ModelsIncludeToAll
    {
        /// <summary> Product code </summary>
        public string ProductCode { get; private set; }

        /// <summary> Product name </summary>
        public string Name { get; private set; }

        /// <summary> Product price </summary>
        public float Price { get; private set; }

        /// <summary> Minimum quantity to apply promotional price </summary>
        public long PromotionalQuantity { get; private set; }

        /// <summary> Price if buy promotional quantity or more </summary>
        public float PromotionalPrice { get; private set; }

        /// <summary> Register new product </summary>
        /// <param name="productParams"> Product params </param>
        public ProductList(in ProductParams productParams)
            : base(false)
        {
            var status = new ValidationStatusHandler();
            status.Message = "Product created";
            status.CombineErrors(ValidateProperties(in productParams));
            if (status.HasErrors) throw new ArgumentException(status.GetAllErrors());

            this.ProductCode = productParams.ProductCode;
            this.Name = productParams.Name;
            this.Price = productParams.Price.HasValue ? productParams.Price.Value : 0;
            this.PromotionalQuantity = productParams.PromotionalQuantity.HasValue ? productParams.PromotionalQuantity.Value : 0;
            this.PromotionalPrice = productParams.PromotionalPrice.HasValue ? productParams.PromotionalPrice.Value : 0;
        }

        private ProductList()
        {
        }

        /// <summary> Validate input properties </summary>
        /// <param name="productParams"> Product params </param>
        /// <returns> Validation status </returns>
        public static IValidationStatus ValidateProperties(in ProductParams productParams)
        {
            var status = new ValidationStatusHandler();
            status.CombineErrors(ValidateProperties(productParams.IsDeleted));

            if (string.IsNullOrEmpty(productParams.ProductCode))
                status.AddError("Product code should be set");

            return status;
        }

        /// <summary> Immutable struct (pass it for performance using in, out, ref) </summary>
        public readonly struct ProductParams
        {
            /// <summary> Immutable struct (pass it for performance using in, out, ref) </summary>
            /// <param name="id"> <see cref="Id"/> </param>
            /// <param name="productCode"> <see cref="ProductCode"/> </param>
            /// <param name="name"> <see cref="Name"/> </param>
            /// <param name="price"> <see cref="Price"/> </param>
            /// <param name="promotionalQuantity"> <see cref="PromotionalQuantity"/> </param>
            /// <param name="promotionalPrice"> <see cref="PromotionalPrice"/> </param>
            /// <param name="isDeleted"> <see cref="IsDeleted"/> </param>
            public ProductParams(long? id, string productCode, string name, float? price, long? promotionalQuantity, float? promotionalPrice, bool? isDeleted)
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
}
