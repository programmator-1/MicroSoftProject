
using MicroSoftContract.Exceptions;
using MicroSoftContract.Extensions;
using MicroSoftContract.Infrastructure;

namespace MicroSoftContract.DataModels
{
    public class InstallDataModel(string id, string productId, string workerId, double pPrice, double iPrice) : IValidation
    {
        public string Id { get; private set; } = id;

        public string ProductId { get; private set; } = productId;

        public string WorkerId { get; private set; } = workerId;

        public DateTime InstallDate { get; private set; } = DateTime.UtcNow;

        public double ProductPrice { get; private set; } = pPrice;

        public double InstallPrice { get; private set; } = iPrice;        

        public void Validate()
        {
            if (Id.IsEmpty())
                throw new ValidationException("Field Id is empty");

            if (!Id.IsGuid())
                throw new ValidationException("The value in the field Id is not a unique identifier");

            if (ProductId.IsEmpty())
                throw new ValidationException("Field ProductId is empty");

            if (!ProductId.IsGuid())
                throw new ValidationException("The value in the field ProductId is not a unique identifier");

            if (WorkerId.IsEmpty())
                throw new ValidationException("Field WorkerId is empty");

            if (!WorkerId.IsGuid())
                throw new ValidationException("The value in the field WorkerId is not a unique identifier");

            if (ProductPrice <= 0)
                throw new ValidationException("Field Price is less than or equal to 0");

            if (InstallPrice <= 0)
                throw new ValidationException("Field InstallPrice is less than or equal to 0");
        }
    }
}
