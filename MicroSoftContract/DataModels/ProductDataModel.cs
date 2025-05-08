

using MicroSoftContract.Enums;
using MicroSoftContract.Exceptions;
using MicroSoftContract.Extensions;
using MicroSoftContract.Infrastructure;

namespace MicroSoftContract.DataModels
{
    public class ProductDataModel(string id, string productName, ProductType productType, string manufacturerId, double pPrice, double iPrice, bool isDeleted) : IValidation
    {
        public string Id { get; private set; } = id;

        public string ProductName { get; private set; } = productName;

        public ProductType ProductType { get; private set; } = productType;

        public string ManufacturerId { get; private set; } = manufacturerId;

        public double ProductPrice { get; private set; } = pPrice;

        public double InstallPrice { get; private set; } = iPrice;

        public bool IsDeleted { get; private set; } = isDeleted;

        public void Validate()
        {
            if (Id.IsEmpty())
                throw new ValidationException("Field Id is empty");

            if (!Id.IsGuid())
                throw new ValidationException("The value in the field Id is not a unique identifier"); 
      

            if (ProductName.IsEmpty())
                throw new ValidationException("Field ProductName is empty");

            if (ProductType == ProductType.None)
                throw new ValidationException("Field ProductType is empty");

            if (ManufacturerId.IsEmpty())
                throw new ValidationException("Field ManufacturerId is empty"); 
      

            if (!ManufacturerId.IsGuid())
                throw new ValidationException("The value in the field ManufacturerId is not a unique identifier");      

            if (ProductPrice <= 0)
                throw new ValidationException("Field ProductPrice is less than or equal to 0");

            if (InstallPrice <= 0)
                throw new ValidationException("Field InstallPrice is less than or equal to 0");
        }
    }
}
