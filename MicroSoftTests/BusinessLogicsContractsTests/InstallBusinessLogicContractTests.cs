using Microsoft.Extensions.Logging;
using MicroSoftBusinessLogic.Implementations;
using MicroSoftContract.DataModels;
using MicroSoftContract.Exceptions;
using MicroSoftContract.StorageContracts;
using Moq;

namespace MicroSoftTests;

[TestFixture]
internal class InstallBusinessLogicContractTests
{
    private InstallBusinessLogicContract _installBusinessLogicContract;
    private Mock<IInstallStorageContract> _installStorageContract;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _installStorageContract = new Mock<IInstallStorageContract>();
        _installBusinessLogicContract = new InstallBusinessLogicContract(_installStorageContract.Object, new Mock<ILogger>().Object);
    }

    [SetUp]
    public void SetUp()
    {
        _installStorageContract.Reset();
    }

    [Test]
    public void GetAllSalesByPeriod_ReturnListOfRecords_Test()
    {
        //Arrange 
        var date = DateTime.UtcNow;
        var listOriginal = new List<InstallDataModel>() {
            new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 100, 10),
            new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 100, 10),
            new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 100, 10),
        };
        _installStorageContract.Setup(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>())).Returns(listOriginal);
        //Act 
        var list = _installBusinessLogicContract.GetAllInstallsByPeriod(date, date.AddDays(1));
        //Assert 
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Is.EquivalentTo(listOriginal));
        _installStorageContract.Verify(x => x.GetList(date, date.AddDays(1), null, null), Times.Once);
    }

    [Test]
    public void GetAllSalesByPeriod_ReturnEmptyList_Test()
    {
        //Arrange 
        _installStorageContract.Setup(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>())).Returns([]);
        //Act 
        var list = _installBusinessLogicContract.GetAllInstallsByPeriod(DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
        //Assert 
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Has.Count.EqualTo(0));
        _installStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetAllSalesByPeriod_IncorrectDates_ThrowException_Test()
    {
        //Arrange 
        var date = DateTime.UtcNow;
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.GetAllInstallsByPeriod(date, date), Throws.TypeOf<IncorrectDatesException>());
        Assert.That(() => _installBusinessLogicContract.GetAllInstallsByPeriod(date, date.AddSeconds(-1)), Throws.TypeOf<IncorrectDatesException>());
        _installStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetAllSalesByPeriod_ReturnNull_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.GetAllInstallsByPeriod(DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), Throws.TypeOf<NullListException>());
        _installStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetAllSalesByPeriod_StorageThrowError_ThrowException_Test()
    {
        //Arrange 
        _installStorageContract.Setup(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new StorageException(new InvalidOperationException()));
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.GetAllInstallsByPeriod(DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), Throws.TypeOf<StorageException>());
        _installStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetAllSalesByWorkerByPeriod_ReturnListOfRecords_Test()
    {
        //Arrange 
        var date = DateTime.UtcNow;
        var workerId = Guid.NewGuid().ToString();
        var listOriginal = new List<InstallDataModel>() {
            new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 50, 12),
            new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 10, 12),
            new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 10, 12),
        };
        _installStorageContract.Setup(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>())).Returns(listOriginal);
        //Act 
        var list = _installBusinessLogicContract.GetAllInstallsByWorkerByPeriod(workerId, date, date.AddDays(1));
        //Assert 
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Is.EquivalentTo(listOriginal));
        _installStorageContract.Verify(x => x.GetList(date, date.AddDays(1), workerId, null), Times.Once);
    }

    [Test]
    public void GetAllSalesByWorkerByPeriod_ReturnEmptyList_Test()
    {
        //Arrange 
        _installStorageContract.Setup(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>())).Returns([]);
        //Act 
        var list = _installBusinessLogicContract.GetAllInstallsByWorkerByPeriod(Guid.NewGuid().ToString(), DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
        //Assert 
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Has.Count.EqualTo(0));
        _installStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetAllSalesByWorkerByPeriod_IncorrectDates_ThrowException_Test()
    {
        //Arrange 
        var date = DateTime.UtcNow;
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.GetAllInstallsByWorkerByPeriod(Guid.NewGuid().ToString(), date, date), Throws.TypeOf<IncorrectDatesException>());
        Assert.That(() => _installBusinessLogicContract.GetAllInstallsByWorkerByPeriod(Guid.NewGuid().ToString(), date, date.AddSeconds(-1)), Throws.TypeOf<IncorrectDatesException>());
        _installStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetAllSalesByWorkerByPeriod_WorkerIdIsNullOrEmpty_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.GetAllInstallsByWorkerByPeriod(null, DateTime.UtcNow,  DateTime.UtcNow.AddDays(1)), Throws.TypeOf<ArgumentNullException>());
        Assert.That(() => _installBusinessLogicContract.GetAllInstallsByWorkerByPeriod(string.Empty, DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), Throws.TypeOf<ArgumentNullException>());
        _installStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetAllSalesByWorkerByPeriod_WorkerIdIsNotGuid_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.GetAllInstallsByWorkerByPeriod("workerId",  DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), Throws.TypeOf<ValidationException>());
        _installStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetAllSalesByWorkerByPeriod_ReturnNull_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.GetAllInstallsByWorkerByPeriod(Guid.NewGuid().ToString(), DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), Throws.TypeOf<NullListException>());
        _installStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetAllSalesByWorkerByPeriod_StorageThrowError_ThrowException_Test()
    {
        //Arrange 
        _installStorageContract.Setup(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new StorageException(new InvalidOperationException()));
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.GetAllInstallsByWorkerByPeriod(Guid.NewGuid().ToString(), DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), Throws.TypeOf<StorageException>());
        _installStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }    

    [Test]
    public void GetAllSalesByProductByPeriod_ReturnListOfRecords_Test()
    {
        //Arrange 
        var date = DateTime.UtcNow;
        var productId = Guid.NewGuid().ToString();
        var listOriginal = new List<InstallDataModel>() {
            new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 50, 12),
            new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 50, 12),
            new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 50, 12),
        };
        _installStorageContract.Setup(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>())).Returns(listOriginal);
        //Act 
        var list = _installBusinessLogicContract.GetAllInstallsByProductByPeriod(productId, date, date.AddDays(1));
        //Assert 
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Is.EquivalentTo(listOriginal));
        _installStorageContract.Verify(x => x.GetList(date, date.AddDays(1), null, productId), Times.Once);
    }

    [Test]
    public void GetAllSalesByProductByPeriod_ReturnEmptyList_Test()
    {
        //Arrange 
        _installStorageContract.Setup(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>())).Returns([]);
        //Act 
        var list = _installBusinessLogicContract.GetAllInstallsByProductByPeriod(Guid.NewGuid().ToString(), DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
        //Assert 
        Assert.That(list, Is.Not.Null);
        Assert.That(list, Has.Count.EqualTo(0));
        _installStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetAllSalesByProductByPeriod_IncorrectDates_ThrowException_Test()
    {
        //Arrange 
        var date = DateTime.UtcNow;
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.GetAllInstallsByProductByPeriod(Guid.NewGuid().ToString(), date, date), Throws.TypeOf<IncorrectDatesException>());
        Assert.That(() => _installBusinessLogicContract.GetAllInstallsByProductByPeriod(Guid.NewGuid().ToString(), date, date.AddSeconds(-1)), Throws.TypeOf<IncorrectDatesException>());
        _installStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetAllSalesByProductByPeriod_ProductIdIsNullOrEmpty_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.GetAllInstallsByProductByPeriod(null, DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), Throws.TypeOf<ArgumentNullException>());
        Assert.That(() => _installBusinessLogicContract.GetAllInstallsByProductByPeriod(string.Empty, DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), Throws.TypeOf<ArgumentNullException>());
        _installStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetAllSalesByProductByPeriod_ProductIdIsNotGuid_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.GetAllInstallsByProductByPeriod("productId", DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), Throws.TypeOf<ValidationException>());
        _installStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetAllSalesByProductByPeriod_ReturnNull_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.GetAllInstallsByProductByPeriod(Guid.NewGuid().ToString(), DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), Throws.TypeOf<NullListException>());
        _installStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetAllSalesByProductByPeriod_StorageThrowError_ThrowException_Test()
    {
        //Arrange 
        _installStorageContract.Setup(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new StorageException(new InvalidOperationException()));
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.GetAllInstallsByProductByPeriod(Guid.NewGuid().ToString(), DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), Throws.TypeOf<StorageException>());
        _installStorageContract.Verify(x => x.GetList(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetSaleByData_GetById_ReturnRecord_Test()
    {
        //Arrange 
        var id = Guid.NewGuid().ToString();
        var record = new InstallDataModel(id, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 50, 12);
        _installStorageContract.Setup(x => x.GetElementById(id)).Returns(record);
        //Act 
        var element = _installBusinessLogicContract.GetInstallByData(id);
        //Assert 
        Assert.That(element, Is.Not.Null);
        Assert.That(element.Id, Is.EqualTo(id));
        _installStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetSaleByData_EmptyData_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.GetInstallByData(null), Throws.TypeOf<ArgumentNullException>());
        Assert.That(() => _installBusinessLogicContract.GetInstallByData(string.Empty), Throws.TypeOf<ArgumentNullException>());
        _installStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetSaleByData_IdIsNotGuid_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.GetInstallByData("saleId"), Throws.TypeOf<ValidationException>());
        _installStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void GetSaleByData_GetById_NotFoundRecord_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.GetInstallByData(Guid.NewGuid().ToString()), Throws.TypeOf<ElementNotFoundException>());
        _installStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void GetSaleByData_StorageThrowError_ThrowException_Test()
    {
        //Arrange 
        _installStorageContract.Setup(x => x.GetElementById(It.IsAny<string>())).Throws(new StorageException(new InvalidOperationException()));
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.GetInstallByData(Guid.NewGuid().ToString()), Throws.TypeOf<StorageException>());
        _installStorageContract.Verify(x => x.GetElementById(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void InsertSale_CorrectRecord_Test()
    {
        //Arrange 
        var flag = false;
        var record = new InstallDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 50, 12);
        _installStorageContract.Setup(x => x.AddElement(It.IsAny<InstallDataModel>())).Callback((InstallDataModel x) => {
            flag = x.Id == record.Id && x.ProductId == record.ProductId && x.WorkerId == record.WorkerId && x.InstallDate == record.InstallDate
            && x.ProductPrice == record.ProductPrice && x.InstallPrice == record.InstallPrice;
         });
        //Act 
        _installBusinessLogicContract.InsertInstall(record);
        //Assert 
        _installStorageContract.Verify(x => x.AddElement(It.IsAny<InstallDataModel>()), Times.Once);
        Assert.That(flag);
    }

    [Test]
    public void InsertSale_RecordWithExistsData_ThrowException_Test()
    {
        //Arrange 
        _installStorageContract.Setup(x => x.AddElement(It.IsAny<InstallDataModel>())).Throws(new ElementExistsException("Data", "Data"));
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.InsertInstall(new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 50, 12)), Throws.TypeOf<ElementExistsException>());
        _installStorageContract.Verify(x => x.AddElement(It.IsAny<InstallDataModel>()), Times.Once);
    }

    [Test]
    public void InsertSale_NullRecord_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.InsertInstall(null), Throws.TypeOf<ArgumentNullException>());
        _installStorageContract.Verify(x => x.AddElement(It.IsAny<InstallDataModel>()), Times.Never);
    }

    [Test]
    public void InsertSale_InvalidRecord_ThrowException_Test()
    {
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.InsertInstall(new InstallDataModel("id", Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),100, 10)), Throws.TypeOf<ValidationException>());
        _installStorageContract.Verify(x => x.AddElement(It.IsAny<InstallDataModel>()), Times.Never);
    }

    [Test]
    public void InsertSale_StorageThrowError_ThrowException_Test()
    {
        //Arrange 
        _installStorageContract.Setup(x => x.AddElement(It.IsAny<InstallDataModel>())).Throws(new StorageException(new InvalidOperationException()));
        //Act&Assert 
        Assert.That(() => _installBusinessLogicContract.InsertInstall(new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 5, 12)), Throws.TypeOf<StorageException>());
        _installStorageContract.Verify(x => x.AddElement(It.IsAny<InstallDataModel>()), Times.Once);
    }    
}