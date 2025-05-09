using MicroSoftContract.DataModels;

namespace MicroSoftContract.BusinessLogicsContracts
{
    public interface IManufacturerBusinessLogicContract
    {
        List<ManufacturerDataModel> GetAllManufacturers();

        ManufacturerDataModel GetManufacturerByData(string data);

        void InsertManufacturer(ManufacturerDataModel manufacturerDataModel);

        void UpdateManufacturer(ManufacturerDataModel manufacturerDataModel);

        void DeleteManufacturer(string id);
    }
}
