using MicroSoftContract.DataModels;
using MicroSoftContract.Enums;
using MicroSoftContract.Exceptions;

namespace MicroSoftTests.DataModelsTests;

[TestFixture]
internal class ProductDataModelTests
{
    [Test]
    public void IdIsNullOrEmptyTest()
    {
        var product = CreateDataModel(null, "name", ProductType.OS, Guid.NewGuid().ToString(), 100, 10, false);
        Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
        product = CreateDataModel(string.Empty, "name", ProductType.OS, Guid.NewGuid().ToString(), 100, 10, false);
        Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void IdIsNotGuidTest()
    {
        var product = CreateDataModel("id", "name", ProductType.OS, Guid.NewGuid().ToString(), 100, 10, false);
        Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void ProductNameIsEmptyTest()
    {
        var product = CreateDataModel(Guid.NewGuid().ToString(), null, ProductType.OS, Guid.NewGuid().ToString(), 100, 10, false);
        Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
        product = CreateDataModel(Guid.NewGuid().ToString(), string.Empty, ProductType.OS, Guid.NewGuid().ToString(), 100, 10, false);
        Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void ProductTypeIsNoneTest()
    {
        var product = CreateDataModel(Guid.NewGuid().ToString(), null, ProductType.None, Guid.NewGuid().ToString(), 100, 10, false);
        Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void ManufacturerIdIsNullOrEmptyTest()
    {
        var product = CreateDataModel(Guid.NewGuid().ToString(), "name", ProductType.OS, null, 100, 10, false);
        Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
        product = CreateDataModel(Guid.NewGuid().ToString(), "name", ProductType.OS, string.Empty, 100, 10, false);
        Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void ManufacturerIdIsNotGuidTest()
    {
        var product = CreateDataModel(Guid.NewGuid().ToString(), "name", ProductType.OS, "manufacturerId", 100, 10, false);
        Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void ProductPriceIsLessOrZeroTest()
    {
        var product = CreateDataModel(Guid.NewGuid().ToString(), "name", ProductType.OS, Guid.NewGuid().ToString(), 0, 10, false);
        Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
        product = CreateDataModel(Guid.NewGuid().ToString(), "name", ProductType.OS, Guid.NewGuid().ToString(), -10, 10, false);
        Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void InstallPriceIsLessOrZeroTest()
    {
        var product = CreateDataModel(Guid.NewGuid().ToString(), "name", ProductType.OS, Guid.NewGuid().ToString(), 100, 0, false);
        Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
        product = CreateDataModel(Guid.NewGuid().ToString(), "name", ProductType.OS, Guid.NewGuid().ToString(), 100, -10, false);
        Assert.That(() => product.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void AllFieldsIsCorrectTest()
    {
        var productId = Guid.NewGuid().ToString();
        var productName = "name";
        var productType = ProductType.OS;
        var productManufacturerId = Guid.NewGuid().ToString();
        var productPrice = 100;
        var installPrice = 10;
        var productIsDelete = false;
        var product = CreateDataModel(productId, productName, productType, productManufacturerId, productPrice, installPrice, productIsDelete);
        Assert.That(() => product.Validate(), Throws.Nothing);
        Assert.Multiple(() =>
        {
            Assert.That(product.Id, Is.EqualTo(productId));
            Assert.That(product.ProductName, Is.EqualTo(productName));
            Assert.That(product.ProductType, Is.EqualTo(productType));
            Assert.That(product.ManufacturerId, Is.EqualTo(productManufacturerId));
            Assert.That(product.ProductPrice, Is.EqualTo(productPrice));
            Assert.That(product.InstallPrice, Is.EqualTo(installPrice));
            Assert.That(product.IsDeleted, Is.EqualTo(productIsDelete));
        });
    }

    private static ProductDataModel CreateDataModel(string? id, string? productName, ProductType productType, string? manufacturerId, double pPrice, double iPrice, bool isDeleted)
        => new(id, productName, productType, manufacturerId, pPrice, iPrice, isDeleted);
}
