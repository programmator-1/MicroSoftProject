using Microsoft.Extensions.Logging;
using MicroSoftContract.BusinessLogicsContracts;
using MicroSoftContract.DataModels;
using MicroSoftContract.Exceptions;
using MicroSoftContract.Extensions;
using MicroSoftContract.StorageContracts;
using System.Text.Json;

namespace MicroSoftBusinessLogic.Implementations
{
    internal class RequestBusinessLogicContract(IRequestStorageContract requestStorageContract, ILogger logger) : IRequestBusinessLogicContract
    {
        private readonly ILogger _logger = logger;
        private readonly IRequestStorageContract _requestStorageContract = requestStorageContract;        

        public List<RequestDataModel> GetAllRequestsByPeriod(DateTime fromDate, DateTime toDate)
        {
            _logger.LogInformation("GetAllRequestsByPeriod params: {fromDate}, {toDate}", fromDate, toDate);
            if (fromDate.IsDateNotOlder(toDate))
            {
                throw new IncorrectDatesException(fromDate, toDate);
            }
            return _requestStorageContract.GetList(fromDate, toDate) ?? throw new NullListException();
        }

        public List<RequestDataModel> GetAllRequestsByProductByPeriod(string productId, DateTime fromDate, DateTime toDate)
        {
            _logger.LogInformation("GetAllRequestsByProductByPeriod params: {productId}, {fromDate}, { toDate}", productId, fromDate, toDate);
            if (fromDate.IsDateNotOlder(toDate))
            {
                throw new IncorrectDatesException(fromDate, toDate);
            }
            if (productId.IsEmpty())
            {
                throw new ArgumentNullException(nameof(productId));
            }
            if (!productId.IsGuid())
            {
                throw new ValidationException("The value in the field productId is not a unique identifier."); 
            }
            return _requestStorageContract.GetList(fromDate, toDate, productId: productId) ?? throw new NullListException();
        }

        public RequestDataModel GetRequestByData(string data)
        {
            _logger.LogInformation("Get element by data: {data}", data);
            if (data.IsEmpty())
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (!data.IsGuid())
            {
                throw new ValidationException("Id is not a unique identifier"); 
            }
            return _requestStorageContract.GetElementById(data) ?? throw new ElementNotFoundException(data);
        }

        public void InsertRequest(RequestDataModel requestDataModel)
        {
            _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(requestDataModel));
            ArgumentNullException.ThrowIfNull(requestDataModel);
            requestDataModel.Validate();
            _requestStorageContract.AddElement(requestDataModel);
        }

        public void CancelRequest(string id)
        {
            _logger.LogInformation("Cancel by id: {id}", id);
            if (id.IsEmpty())
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (!id.IsGuid())
            {
                throw new ValidationException("Id is not a unique identifier"); 
            }
            _requestStorageContract.DelElement(id);
        }
    }
}
