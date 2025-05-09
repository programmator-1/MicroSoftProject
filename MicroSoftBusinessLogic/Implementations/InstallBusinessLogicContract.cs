using Microsoft.Extensions.Logging;
using MicroSoftContract.BusinessLogicsContracts;
using MicroSoftContract.DataModels;
using MicroSoftContract.Exceptions;
using MicroSoftContract.Extensions;
using MicroSoftContract.StorageContracts;
using System.Text.Json;

namespace MicroSoftBusinessLogic.Implementations
{
    internal class InstallBusinessLogicContract(IInstallStorageContract installStorageContract, ILogger logger) : IInstallBusinessLogicContract
    {
        private readonly ILogger _logger = logger;
        private readonly IInstallStorageContract _installStorageContract = installStorageContract;

        public List<InstallDataModel> GetAllInstallsByPeriod(DateTime fromDate, DateTime toDate)
        {
            _logger.LogInformation("GetAllInstallsByPeriod params: {fromDate}, {toDate}", fromDate, toDate);
            if (fromDate.IsDateNotOlder(toDate))
            {
                throw new IncorrectDatesException(fromDate, toDate);
            }
            return _installStorageContract.GetList(fromDate, toDate) ?? throw new NullListException();
        }

        public List<InstallDataModel> GetAllInstallsByWorkerByPeriod(string workerId, DateTime fromDate, DateTime toDate)
        {
            _logger.LogInformation("GetAllInstallsByWorkerByPeriod params: {workerId}, {fromDate}, { toDate}", workerId, fromDate, toDate); 
            if (fromDate.IsDateNotOlder(toDate))
            {
                throw new IncorrectDatesException(fromDate, toDate);
            }
            if (workerId.IsEmpty())
            {
                throw new ArgumentNullException(nameof(workerId));
            }
            if (!workerId.IsGuid())
            {
                throw new ValidationException("The value in the field workerId is not a unique identifier."); 
            }
            return _installStorageContract.GetList(fromDate, toDate, workerId: workerId) ?? throw new NullListException();
        }

        public List<InstallDataModel> GetAllInstallsByProductByPeriod(string productId, DateTime fromDate, DateTime toDate)
        {
            _logger.LogInformation("GetAllInstallsByProductByPeriod params: {productId}, {fromDate}, { toDate}", productId, fromDate, toDate); 
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
            return _installStorageContract.GetList(fromDate, toDate, productId: productId) ?? throw new NullListException();
        }

        public InstallDataModel GetInstallByData(string data)
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
            return _installStorageContract.GetElementById(data) ?? throw new ElementNotFoundException(data);
        }

        public void InsertInstall(InstallDataModel installDataModel)
        {
            _logger.LogInformation("New data: {json}", JsonSerializer.Serialize(installDataModel));
            ArgumentNullException.ThrowIfNull(installDataModel);
            installDataModel.Validate();
            _installStorageContract.AddElement(installDataModel);
        }       
    }
}
