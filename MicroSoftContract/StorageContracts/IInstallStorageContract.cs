using MicroSoftContract.DataModels;

namespace MicroSoftContract.StorageContracts
{
    public interface IInstallStorageContract
    {
        List<InstallDataModel> GetList(DateTime? startDate = null, DateTime? endDate = null, string? workerId = null, string? productId = null);

        InstallDataModel? GetElementById(string id);

        void AddElement(InstallDataModel installDataModel);

        void DelElement(string id);
    }
}
