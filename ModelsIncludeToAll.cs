using System;
using ValidationStatus;

namespace TestWunderMobilityCheckout
{
    /// <summary> Include to all models (general params) except Auth agregate </summary>
    public class ModelsIncludeToAll
    {
        /// <summary> Unique id </summary>
        public long Id { get; private set; }

        /// <summary> Soft delete attribute </summary>
        public bool IsDeleted { get; private set; }

        /// <summary> Initializes a new instance of the <see cref="ModelsIncludeToAll"/> class. Parameterless constructor </summary>
        protected ModelsIncludeToAll()
        {
        }

        /// <summary> Constructor </summary>
        /// <param name="isDeleted"> Soft delete mark </param>
        protected ModelsIncludeToAll(bool isDeleted)
        {
            var status = new ValidationStatusHandler();
            status.CombineErrors(ValidateProperties(isDeleted));
            if (status.HasErrors) throw new ArgumentException(status.GetAllErrors());
        }

        /// <summary> Properties validator </summary>
        /// <param name="isDeleted"> Soft delete parameter </param>
        /// <returns> Validation status </returns>
        public static IValidationStatus ValidateProperties(bool? isDeleted)
        {
            var status = new ValidationStatusHandler();
            if (!isDeleted.HasValue)
            {
                status.AddError("IsDeleted param has to be false or true");
            }

            return status;
        }

        /// <summary>
        /// Delete element softly (mark as deleted)
        /// </summary>
        public void SoftDelete()
        {
            this.IsDeleted = true;
        }

        /// <summary>
        /// Undelete element softly (unmark as deleted)
        /// </summary>
        public void SoftUnDelete()
        {
            this.IsDeleted = false;
        }
    }
}
