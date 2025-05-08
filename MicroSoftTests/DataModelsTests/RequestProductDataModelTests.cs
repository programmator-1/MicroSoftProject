using MicroSoftContract.DataModels;
using MicroSoftContract.Exceptions;

namespace MicroSoftTests.DataModelsTests;

[TestFixture]
internal class RequestProductDataModelTests
{
    [Test]
    public void RequestIdIsNullOrEmptyTest()
    {
        var item = CreateDataModel(null, Guid.NewGuid().ToString(), 100, 10);
        Assert.That(() => item.Validate(), Throws.TypeOf<ValidationException>());
        item = CreateDataModel(string.Empty, Guid.NewGuid().ToString(), 100, 10);
        Assert.That(() => item.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void RequestIdIsNotGuidTest()
    {
        var item = CreateDataModel("id", Guid.NewGuid().ToString(), 100, 10);
        Assert.That(() => item.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void ProductIdIsNullOrEmptyTest()
    {
        var item = CreateDataModel(Guid.NewGuid().ToString(), null, 100, 10);
        Assert.That(() => item.Validate(), Throws.TypeOf<ValidationException>());
        item = CreateDataModel(Guid.NewGuid().ToString(), string.Empty, 100, 10);
        Assert.That(() => item.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void ProductIdIsNotGuidTest()
    {
        var item = CreateDataModel(Guid.NewGuid().ToString(), "id", 100, 10);
        Assert.That(() => item.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void ProductPriceIsLessOrZeroTest()
    {
        var item = CreateDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 0, 10);
        Assert.That(() => item.Validate(), Throws.TypeOf<ValidationException>());
        item = CreateDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), -100, 10);
        Assert.That(() => item.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void InstallPriceIsLessOrZeroTest()
    {
        var item = CreateDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 100, 0);
        Assert.That(() => item.Validate(), Throws.TypeOf<ValidationException>());
        item = CreateDataModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 100, -10);
        Assert.That(() => item.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void AllFieldsIsCorrectTest()
    {
        var requestId = Guid.NewGuid().ToString();
        var productId = Guid.NewGuid().ToString();        
        double pPrice = 100;
        double iPrice = 10;
        var item = CreateDataModel(requestId, productId, pPrice, iPrice);
        Assert.That(() => item.Validate(), Throws.Nothing);
        Assert.Multiple(() =>
        {
            Assert.That(item.RequestId, Is.EqualTo(requestId));
            Assert.That(item.ProductId, Is.EqualTo(productId));            
            Assert.That(item.ProductPrice, Is.EqualTo(pPrice));
            Assert.That(item.InstallPrice, Is.EqualTo(iPrice));
        });
    }


    private static RequestProductDataModel CreateDataModel(string? requestId, string? productId, double pPrice, double iPrice) =>
        new RequestProductDataModel(requestId, productId, pPrice, iPrice);
}
