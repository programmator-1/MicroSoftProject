using MicroSoftContract.DataModels;

namespace MicroSoftContract.StorageContracts
{
    public interface IProductStorageContract
    {
        List<ProductDataModel> GetList(bool onlyActive = true, string? manufacturerId = null);

        List<ProductHistoryDataModel> GetHistoryByProductId(string productId);

        ProductDataModel? GetElementById(string id);

        ProductDataModel? GetElementByName(string name);

        void AddElement(ProductDataModel productDataModel);

        void UpdElement(ProductDataModel productDataModel);

        void DelElement(string id);
    }
}
