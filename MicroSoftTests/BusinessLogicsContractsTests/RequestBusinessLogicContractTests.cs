using Microsoft.Extensions.Logging;
using MicroSoftBusinessLogic.Implementations;
using MicroSoftContract.DataModels;
using MicroSoftContract.Exceptions;
using MicroSoftContract.StorageContracts;
using Moq;

namespace MicroSoftTests;

[TestFixture]
internal class RequestBusinessLogicContractTests
{
    private RequestBusinessLogicContract _requestBusinessLogicContract;
    private Mock<IRequestStorageContract> _requestStorageContract;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _requestStorageContract = new Mock<IRequestStorageContract>();
        _requestBusinessLogicContract = new RequestBusinessLogicContract(_requestStorageContract.Object, new Mock<ILogger>().Object);
    }

    [TearDown]
    public void TearDown()
    {
        _requestStorageContract.Reset();
    }

    [Test]
    public void GetAllSalesByPeriod_ReturnListOfRecords_Test()
    {
        //Arrange 
        var date = DateTime.UtcNow;
        var listOriginal = new List<RequestDataModel>() {
            new(Guid.NewGuid().ToString(), false, [new RequestProductDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 100, 10)]), 
            new(Guid.NewGuid().ToString(), false, []), 
            new(Guid.NewGuid().ToString(),false, []),
        };
        _requestStorageContract.Setup(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>())).Returns(listOriginal);
        //Act 
        var list = _requestBusinessLogicContract.GetAllRequestsByPeriod(date, date.AddDays(1));
        //Assert 
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Is.EquivalentTo(listOriginal));
        _requestStorageContract.Verify(x => x.GetList(date, date.AddDays(1), null), Times.Once);
    }

    [Test]
    public void GetAllSalesByPeriod_ReturnEmptyList_Test()
    {
        //Arrange 
        _requestStorageContract.Setup(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>())).Returns([]);
        //Act 
        var list = _requestBusinessLogicContract.GetAllRequestsByPeriod(DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
        //Assert 
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Has.Count.EqualTo(0));
        _requestStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetAllSalesByPeriod_IncorrectDates_ThrowException_Test()
    {
        //Arrange 
        var date = DateTime.UtcNow;
        //Act&Assert 
        Assert.That(() => _requestBusinessLogicContract.GetAllRequestsByPeriod(date, date), Throws.TypeOf<IncorrectDatesException>());
        Assert.That(() => _requestBusinessLogicContract.GetAllRequestsByPeriod(date, date.AddSeconds(-1)), Throws.TypeOf<IncorrectDatesException>());
        _requestStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetAllSalesByPeriod_ReturnNull_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _requestBusinessLogicContract.GetAllRequestsByPeriod(DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), Throws.TypeOf<NullListException>());
        _requestStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetAllSalesByPeriod_StorageThrowError_ThrowException_Test()
    {
        //Arrange 
        _requestStorageContract.Setup(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>())).Throws(new StorageException(new InvalidOperationException()));
        //Act&Assert 
        Assert.That(() => _requestBusinessLogicContract.GetAllRequestsByPeriod(DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), Throws.TypeOf<StorageException>());
        _requestStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>()), Times.Once);
    }    

    [Test]
    public void GetAllSalesByProductByPeriod_ReturnListOfRecords_Test()
    {
        //Arrange 
        var date = DateTime.UtcNow;
        var productId = Guid.NewGuid().ToString();
        var listOriginal = new List<RequestDataModel>() {
            new(Guid.NewGuid().ToString(), false,[new RequestProductDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 100, 10)]),
            new(Guid.NewGuid().ToString(), false,[]),
            new(Guid.NewGuid().ToString(), false,[]),
        };
        _requestStorageContract.Setup(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>())).Returns(listOriginal);
        //Act 
        var list = _requestBusinessLogicContract.GetAllRequestsByProductByPeriod(productId, date, date.AddDays(1));
        //Assert 
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Is.EquivalentTo(listOriginal));
        _requestStorageContract.Verify(x => x.GetList(date, date.AddDays(1), productId), Times.Once);
    }

    [Test]
    public void GetAllSalesByProductByPeriod_ReturnEmptyList_Test()
    {
        //Arrange 
        _requestStorageContract.Setup(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>())).Returns([]);
        //Act 
        var list = _requestBusinessLogicContract.GetAllRequestsByProductByPeriod(Guid.NewGuid().ToString(), DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
        //Assert 
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Has.Count.EqualTo(0));
        _requestStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void
  GetAllSalesByProductByPeriod_IncorrectDates_ThrowException_Test()
    {
        //Arrange 
        var date = DateTime.UtcNow;
        //Act&Assert 
        Assert.That(() => _requestBusinessLogicContract.GetAllRequestsByProductByPeriod(Guid.NewGuid().ToString(), date, date), Throws.TypeOf<IncorrectDatesException>());
        Assert.That(() => _requestBusinessLogicContract.GetAllRequestsByProductByPeriod(Guid.NewGuid().ToString(), date, date.AddSeconds(-1)), Throws.TypeOf<IncorrectDatesException>());
        _requestStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetAllSalesByProductByPeriod_ProductIdIsNullOrEmpty_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _requestBusinessLogicContract.GetAllRequestsByProductByPeriod(null, DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), Throws.TypeOf<ArgumentNullException>());
        Assert.That(() => _requestBusinessLogicContract.GetAllRequestsByProductByPeriod(string.Empty, DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), Throws.TypeOf<ArgumentNullException>());
        _requestStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetAllSalesByProductByPeriod_ProductIdIsNotGuid_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _requestBusinessLogicContract.GetAllRequestsByProductByPeriod("productId", DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), Throws.TypeOf<ValidationException>());
        _requestStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetAllSalesByProductByPeriod_ReturnNull_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _requestBusinessLogicContract.GetAllRequestsByProductByPeriod(Guid.NewGuid().ToString(), DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), Throws.TypeOf<NullListException>());
        _requestStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetAllSalesByProductByPeriod_StorageThrowError_ThrowException_Test()
    {
        //Arrange 
        _requestStorageContract.Setup(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>())).Throws(new StorageException(new InvalidOperationException()));
        //Act&Assert 
        Assert.That(() => _requestBusinessLogicContract.GetAllRequestsByProductByPeriod(Guid.NewGuid().ToString(), DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), Throws.TypeOf<StorageException>());
        _requestStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetSaleByData_GetById_ReturnRecord_Test()
    {
        //Arrange 
        var id = Guid.NewGuid().ToString();
        var record = new RequestDataModel(id, false,  [new RequestProductDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 100, 10)]);
        _requestStorageContract.Setup(x => x.GetElementById(id)).Returns(record);
        //Act 
        var element = _requestBusinessLogicContract.GetRequestByData(id);
        //Assert 
        Assert.That(element, Is.Not.Null);
        Assert.That(element.Id, Is.EqualTo(id));
        _requestStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetSaleByData_EmptyData_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _requestBusinessLogicContract.GetRequestByData(null), Throws.TypeOf<ArgumentNullException>());
        Assert.That(() => _requestBusinessLogicContract.GetRequestByData(string.Empty), Throws.TypeOf<ArgumentNullException>());
        _requestStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetSaleByData_IdIsNotGuid_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _requestBusinessLogicContract.GetRequestByData("saleId"), Throws.TypeOf<ValidationException>());
        _requestStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetSaleByData_GetById_NotFoundRecord_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _requestBusinessLogicContract.GetRequestByData(Guid.NewGuid().ToString()), Throws.TypeOf<ElementNotFoundException>());
        _requestStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetSaleByData_StorageThrowError_ThrowException_Test()
    {
        //Arrange 
        _requestStorageContract.Setup(x => x.GetElementById(It.IsAny<string>())).Throws(new StorageException(new InvalidOperationException()));
        //Act&Assert 
        Assert.That(() => _requestBusinessLogicContract.GetRequestByData(Guid.NewGuid().ToString()), Throws.TypeOf<StorageException>());
        _requestStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void InsertSale_CorrectRecord_Test()
    {
        //Arrange 
        var flag = false;
        var record = new RequestDataModel(Guid.NewGuid().ToString(), false, [new RequestProductDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 50, 10)]);
        _requestStorageContract.Setup(x => x.AddElement(It.IsAny<RequestDataModel>())).Callback((RequestDataModel x) => {
            flag = x.Id == record.Id && x.RequestDate == record.RequestDate && x.Sum ==  record.Sum && x.IsCancel == record.IsCancel && 
            x.Products?.Count == record.Products?.Count && x.Products?.First().ProductId == record.Products?.First().ProductId &&
            x.Products?.First().RequestId == record.Products?.First().RequestId && x.Products?.First().ProductPrice == record.Products?.First().ProductPrice;
          });
        //Act 
        _requestBusinessLogicContract.InsertRequest(record);
        //Assert 
        _requestStorageContract.Verify(x => x.AddElement(It.IsAny<RequestDataModel>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void InsertSale_RecordWithExistsData_ThrowException_Test()
    {
        //Arrange 
        _requestStorageContract.Setup(x => x.AddElement(It.IsAny<RequestDataModel>())).Throws(new ElementExistsException("Data", "Data"));
        //Act&Assert 
        Assert.That(() => _requestBusinessLogicContract.InsertRequest(new(Guid.NewGuid().ToString(), false, [new RequestProductDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 50, 12)])), Throws.TypeOf<ElementExistsException>());
        _requestStorageContract.Verify(x => x.AddElement(It.IsAny<RequestDataModel>()), Times.Once);
    }

    [Test]
    public void InsertSale_NullRecord_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _requestBusinessLogicContract.InsertRequest(null), Throws.TypeOf<ArgumentNullException>());
        _requestStorageContract.Verify(x => x.AddElement(It.IsAny<RequestDataModel>()), Times.Never);
    }

    [Test]
    public void InsertSale_InvalidRecord_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _requestBusinessLogicContract.InsertRequest(new RequestDataModel("id", false, [])), Throws.TypeOf<ValidationException>());
        _requestStorageContract.Verify(x => x.AddElement(It.IsAny<RequestDataModel>()), Times.Never);
    }

    [Test]
    public void InsertSale_StorageThrowError_ThrowException_Test()
    {
        //Arrange 
        _requestStorageContract.Setup(x => x.AddElement(It.IsAny<RequestDataModel>())).Throws(new StorageException(new InvalidOperationException()));
        //Act&Assert 
        Assert.That(() => _requestBusinessLogicContract.InsertRequest(new(Guid.NewGuid().ToString(), false, [new RequestProductDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 50, 12)])), Throws.TypeOf<StorageException>());
        _requestStorageContract.Verify(x => x.AddElement(It.IsAny<RequestDataModel>()), Times.Once);
    }

    [Test]
    public void CancelSale_CorrectRecord_Test()
    {
        //Arrange 
        var id = Guid.NewGuid().ToString();
        var flag = false;
        _requestStorageContract.Setup(x => x.DelElement(It.Is((string x) => x == id))).Callback(() => { flag = true; });
        //Act 
        _requestBusinessLogicContract.CancelRequest(id);
        //Assert 
        _requestStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void CancelSale_RecordWithIncorrectId_ThrowException_Test()
    {
        //Arrange 
        var id = Guid.NewGuid().ToString();
        _requestStorageContract.Setup(x => x.DelElement(It.IsAny<string>())).Throws(new ElementNotFoundException(id));
        //Act&Assert 
        Assert.That(() => _requestBusinessLogicContract.CancelRequest(Guid.NewGuid().ToString()), Throws.TypeOf<ElementNotFoundException>());
        _requestStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void CancelSale_IdIsNullOrEmpty_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _requestBusinessLogicContract.CancelRequest(null), Throws.TypeOf<ArgumentNullException>());
        Assert.That(() => _requestBusinessLogicContract.CancelRequest(string.Empty), Throws.TypeOf<ArgumentNullException>());
        _requestStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void CancelSale_IdIsNotGuid_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _requestBusinessLogicContract.CancelRequest("id"), Throws.TypeOf<ValidationException>());
        _requestStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void CancelSale_StorageThrowError_ThrowException_Test()
    {
        //Arrange 
        _requestStorageContract.Setup(x => x.DelElement(It.IsAny<string>())).Throws(new StorageException(new InvalidOperationException()));
        //Act&Assert 
        Assert.That(() => _requestBusinessLogicContract.CancelRequest(Guid.NewGuid().ToString()), Throws.TypeOf<StorageException>());
        _requestStorageContract.Verify(x => x.DelElement(It.IsAny<string>()), Times.Once);
    }
}