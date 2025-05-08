
using MicroSoftContract.Exceptions;
using MicroSoftContract.Extensions;
using MicroSoftContract.Infrastructure;

namespace MicroSoftContract.DataModels
{
    public class RequestProductDataModel(string requestId, string productId, double pPrice, double iPrice) : IValidation
    {
        public string RequestId { get; private set; } = requestId;

        public string ProductId { get; private set; } = productId;

        public double ProductPrice { get; private set; } = pPrice;

        public double InstallPrice { get; private set; } = iPrice;

        public string? InstallId { get; private set; }

        public void Validate()
        {
            if (RequestId.IsEmpty())
                throw new ValidationException("Field RequestId is empty");

            if (!RequestId.IsGuid())
                throw new ValidationException("The value in the field RequestId is not a unique identifier");

            if (ProductId.IsEmpty())
                throw new ValidationException("Field ProductId is empty");

            if (!ProductId.IsGuid())
                throw new ValidationException("The value in the field ProductId is not a unique identifier");

            if (ProductPrice <= 0)
                throw new ValidationException("Field Price is less than or equal to 0");

            if (InstallPrice <= 0)
                throw new ValidationException("Field InstallPrice is less than or equal to 0");
        }
    }
}
