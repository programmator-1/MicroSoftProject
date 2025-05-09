﻿using MicroSoftContract.DataModels;

namespace MicroSoftContract.StorageContracts
{
    public interface IManufacturerStorageContract
    {
        List<ManufacturerDataModel> GetList();

        ManufacturerDataModel? GetElementById(string id);

        ManufacturerDataModel? GetElementByName(string name);

        ManufacturerDataModel? GetElementByOldName(string name);

        void AddElement(ManufacturerDataModel manufacturerDataModel);

        void UpdElement(ManufacturerDataModel manufacturerDataModel);

        void DelElement(string id);
    }
}
