using MicroSoftContract.DataModels;
using MicroSoftContract.Exceptions;

namespace MicroSoftTests.DataModelsTests;

[TestFixture]
internal class RequestDataModelTests
{
    [Test]
    public void IdIsNullOrEmptyTest()
    {
        var request = CreateDataModel(null, false, CreateSubDataModel());
        Assert.That(() => request.Validate(), Throws.TypeOf<ValidationException>());
        request = CreateDataModel(string.Empty, false, CreateSubDataModel());
        Assert.That(() => request.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void IdIsNotGuidTest()
    {
        var request = CreateDataModel("id", false, CreateSubDataModel());
        Assert.That(() => request.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void ProductsIsNullOrEmptyTest()
    {
        var request = CreateDataModel(Guid.NewGuid().ToString(), false, null);
        Assert.That(() => request.Validate(), Throws.TypeOf<ValidationException>());
        request = CreateDataModel(Guid.NewGuid().ToString(), false, []);
        Assert.That(() => request.Validate(), Throws.TypeOf<ValidationException>());
    }

    [Test]
    public void CalcSumTest()
    {
        var requestId = Guid.NewGuid().ToString();
        var workerId = Guid.NewGuid().ToString();
        var buyerId = Guid.NewGuid().ToString();
        var products = new List<RequestProductDataModel>() {
            new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 100, 10),
            new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 200, 15)
        };
        var isCancel = false;
        var totalSum = products.Sum(x => x.ProductPrice + x.InstallPrice);
        var request = CreateDataModel(requestId, isCancel, products);
        Assert.Multiple(() =>
        {
            Assert.That(request.Sum, Is.EqualTo(totalSum));            
        });
    }

    [Test]
    public void AllFieldsIsCorrectTest()
    {
        var requestId = Guid.NewGuid().ToString();        
        var isCancel = true;
        var products = CreateSubDataModel();
        var request = CreateDataModel(requestId, isCancel, products);
        Assert.That(() => request.Validate(), Throws.Nothing);
        Assert.Multiple(() =>
        {
            Assert.That(request.Id, Is.EqualTo(requestId));            
            Assert.That(request.IsCancel, Is.EqualTo(isCancel));
            Assert.That(request.Products, Is.EquivalentTo(products));
        });
    }

    private static RequestDataModel CreateDataModel(string? id, bool isCancel, List<RequestProductDataModel>? products) =>
        new(id, isCancel, products);

    private static List<RequestProductDataModel> CreateSubDataModel() => 
        [new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 100, 10)];
}
