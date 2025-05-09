using MicroSoftContract.DataModels;

namespace MicroSoftContract.BusinessLogicsContracts
{
    public interface IRequestBusinessLogicContract
    {
        List<RequestDataModel> GetAllRequestsByPeriod(DateTime fromDate, DateTime toDate);

        List<RequestDataModel> GetAllRequestsByProductByPeriod(string productId, DateTime fromDate, DateTime toDate);

        RequestDataModel GetRequestByData(string data);

        void InsertRequest(RequestDataModel requestDataModel);

        void CancelRequest(string id);
    }
}
