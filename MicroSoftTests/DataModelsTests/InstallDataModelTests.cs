using MicroSoftContract.DataModels;
using MicroSoftContract.Exceptions;

namespace MicroSoftTests.DataModelsTests;

[TestFixture]
internal class InstallDataModelTests
{
    [Test]
    public void IdIsNullOrEmptyTest()
    {
        var sale = CreateDataModel(null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 100, 10);
        Assert.That(() => sale.Validate(), Throws.TypeOf<ValidationException>());
        sale = CreateDataModel(string.Empty, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 100, 10);
        Assert.That(() => sale.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void IdIsNotGuidTest()
    {
        var sale = CreateDataModel("id", Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 100, 10);
        Assert.That(() => sale.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void ProductIdIsNullOrEmptyTest()
    {
        var sale = CreateDataModel(Guid.NewGuid().ToString(), null, Guid.NewGuid().ToString(), 100, 10);
        Assert.That(() => sale.Validate(), Throws.TypeOf<ValidationException>());
        sale = CreateDataModel(Guid.NewGuid().ToString(), string.Empty, Guid.NewGuid().ToString(), 100, 10);
        Assert.That(() => sale.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void ProductIdIsNotGuidTest()
    {
        var sale = CreateDataModel(Guid.NewGuid().ToString(), "id", Guid.NewGuid().ToString(), 100, 10);
        Assert.That(() => sale.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void WorkerIdIsNullOrEmptyTest()
    {
        var sale = CreateDataModel(Guid.NewGuid().ToString(),Guid.NewGuid().ToString(),  null, 100, 10);
        Assert.That(() => sale.Validate(), Throws.TypeOf<ValidationException>());
        sale = CreateDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), string.Empty, 100, 10);
        Assert.That(() => sale.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void WorkerIdIsNotGuidTest()
    {
        var sale = CreateDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "id", 100, 10);
        Assert.That(() => sale.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void ProductPriceIsLessOrZeroTest()
    {
        var sale = CreateDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 0, 10);
        Assert.That(() => sale.Validate(), Throws.TypeOf<ValidationException>());
        sale = CreateDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), -100, 10);
        Assert.That(() => sale.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void InstallPriceIsLessOrZeroTest()
    {
        var sale = CreateDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 100, 0);
        Assert.That(() => sale.Validate(), Throws.TypeOf<ValidationException>());
        sale = CreateDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 100, -10);
        Assert.That(() => sale.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void AllFieldsIsCorrectTest()
    {
        var saleId = Guid.NewGuid().ToString();
        var productId = Guid.NewGuid().ToString();
        var workerId = Guid.NewGuid().ToString();
        double pPrice = 100;
        double iPrice = 10;
        var sale = CreateDataModel(saleId, productId, workerId, pPrice, iPrice);
        Assert.That(() => sale.Validate(), Throws.Nothing);
        Assert.Multiple(() =>
        {
            Assert.That(sale.Id, Is.EqualTo(saleId));
            Assert.That(sale.ProductId, Is.EqualTo(productId));
            Assert.That(sale.WorkerId, Is.EqualTo(workerId));
            Assert.That(sale.ProductPrice, Is.EqualTo(pPrice));
            Assert.That(sale.InstallPrice, Is.EqualTo(iPrice));
        });
    }


    private static InstallDataModel CreateDataModel(string? id, string? productId, string? workerId, double pPrice, double iPrice) =>
        new InstallDataModel(id, productId, workerId, pPrice, iPrice);
}
