using MicroSoftContract.DataModels;

namespace MicroSoftContract.StorageContracts
{
    public interface ISalaryStorageContract
    {
        List<SalaryDataModel> GetList(DateTime startDate, DateTime endDate, string? workerId = null);

        void AddElement(SalaryDataModel salaryDataModel);
    }
}
