
using MicroSoftContract.Exceptions;
using MicroSoftContract.Extensions;
using MicroSoftContract.Infrastructure;

namespace MicroSoftContract.DataModels
{
    public class RequestDataModel: IValidation
    {
        public string Id { get; private set; }        

        public DateTime RequestDate { get; private set; } = DateTime.UtcNow;

        public double Sum { get; private set; }

        public bool IsCancel { get; private set; }

        public bool IsCompleted { get; private set; } = false;

        public List<RequestProductDataModel>? Products { get; private set; }

        public RequestDataModel(string id, bool isCancel, List<RequestProductDataModel> products)
        {
            Id = id;            
            IsCancel = isCancel;
            Products = products;
            Sum = Products?.Sum(x => x.ProductPrice + x.InstallPrice) ?? 0;
        }

        public void Validate()
        {
            if (Id.IsEmpty())
                throw new ValidationException("Field Id is empty");

            if (!Id.IsGuid())
                throw new ValidationException("The value in the field Id is not a unique identifier");

            if ((Products?.Count ?? 0) == 0)
                throw new ValidationException("The request must include products");
        }
    }
}
