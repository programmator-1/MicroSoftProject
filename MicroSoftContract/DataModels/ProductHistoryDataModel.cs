
using MicroSoftContract.Exceptions;
using MicroSoftContract.Extensions;
using MicroSoftContract.Infrastructure;

namespace MicroSoftContract.DataModels
{
    public class ProductHistoryDataModel(string productId, double oldProdPrice, double oldInstPrice) :
IValidation
    {
        public string ProductId { get; private set; } = productId;

        public double OldProductPrice { get; private set; } = oldProdPrice;

        public double OldInstallPrice { get; private set; } = oldInstPrice;

        public DateTime ChangeDate { get; private set; } = DateTime.UtcNow;

        public void Validate()
        {
            if (ProductId.IsEmpty())
                throw new ValidationException("Field ProductId is empty");

            if (!ProductId.IsGuid())
                throw new ValidationException("The value in the field ProductId is not a unique identifier");      

            if (OldProductPrice <= 0)
                throw new ValidationException("Field OldProductPrice is less than or equal to 0");

            if (OldInstallPrice <= 0)
                throw new ValidationException("Field OldInstallPrice is less than or equal to 0");
        }
    }
}
