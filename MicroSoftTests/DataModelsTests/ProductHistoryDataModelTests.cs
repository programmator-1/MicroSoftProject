using MicroSoftContract.DataModels;
using MicroSoftContract.Exceptions;

namespace MicroSoftTests.DataModelsTests;

[TestFixture]
internal class ProductHistoryDataModelTests
{
    [Test]
    public void ProductIdIsNullOrEmptyTest()
    {
        var product = CreateDataModel(null, 100, 10);
        Assert.That(() => product.Validate(),  Throws.TypeOf<ValidationException>());
        product = CreateDataModel(string.Empty, 100, 10);
        Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void ProductIdIsNotGuidTest()
    {
        var product = CreateDataModel("id", 100, 10);
        Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void OldProdPriceIsLessOrZeroTest()
    {
        var product = CreateDataModel(Guid.NewGuid().ToString(), 0, 10);
        Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
        product = CreateDataModel(Guid.NewGuid().ToString(), -10, 10);
        Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void OldInstPriceIsLessOrZeroTest()
    {
        var product = CreateDataModel(Guid.NewGuid().ToString(), 100, 0);
        Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
        product = CreateDataModel(Guid.NewGuid().ToString(), 100, -10);
        Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void AllFieldsIsCorrectTest()
    {
        var productId = Guid.NewGuid().ToString();
        var oldProdPrice = 100;
        var oldInstPrice = 10;
        var productHistory = CreateDataModel(productId, oldProdPrice, oldInstPrice);
        Assert.That(() => productHistory.Validate(), Throws.Nothing);
        Assert.Multiple(() =>
        {
            Assert.That(productHistory.ProductId, Is.EqualTo(productId));
            Assert.That(productHistory.OldProductPrice, Is.EqualTo(oldProdPrice));
            Assert.That(productHistory.OldInstallPrice, Is.EqualTo(oldInstPrice));
            Assert.That(productHistory.ChangeDate,  Is.LessThan(DateTime.UtcNow));
            Assert.That(productHistory.ChangeDate, Is.GreaterThan(DateTime.UtcNow.AddMinutes(-1)));
        });
    }

    private static ProductHistoryDataModel CreateDataModel(string? productId, double oldProdPrice, double oldInstPrice) =>
        new(productId, oldProdPrice, oldInstPrice);
}
