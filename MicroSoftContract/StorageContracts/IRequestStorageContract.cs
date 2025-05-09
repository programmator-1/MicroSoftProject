using MicroSoftContract.DataModels;

namespace MicroSoftContract.StorageContracts
{    
    public interface IRequestStorageContract
    {
        List<RequestDataModel> GetList(DateTime? startDate = null, DateTime? endDate = null, string? productId = null);

        RequestDataModel? GetElementById(string id);

        void AddElement(RequestDataModel requestDataModel);

        void DelElement(string id);
    }
}
