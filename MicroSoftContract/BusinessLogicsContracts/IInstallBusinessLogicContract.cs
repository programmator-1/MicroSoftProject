using MicroSoftContract.DataModels;

namespace MicroSoftContract.BusinessLogicsContracts
{
    public interface IInstallBusinessLogicContract
    {
        List<InstallDataModel> GetAllInstallsByPeriod(DateTime fromDate, DateTime toDate);

        List<InstallDataModel> GetAllInstallsByWorkerByPeriod(string workerId, DateTime fromDate, DateTime toDate);        

        List<InstallDataModel> GetAllInstallsByProductByPeriod(string productId, DateTime fromDate, DateTime toDate);

        InstallDataModel GetInstallByData(string data);

        void InsertInstall(InstallDataModel installDataModel);
    }
}
