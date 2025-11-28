using CollectApp.ViewModels;

namespace CollectAppTests.Builders;

public class CreateCollectViewModelBuilder
{
    private int _supplierId = 1;
    private string? _supplier = "Fornecedor ABC LTDA";
    private DateTime _collectAt = new DateTime(2025, 12, 25);
    private int _productId = 1;
    private string? _product = "Papel Reciclável";
    private int _volume = 100;
    private int _weight = 50;
    private int _filialId = 1;
    private string? _filial = "Filial SP Centro";

    public CreateCollectViewModelBuilder WithSupplierId(int supplierId)
    {
        _supplierId = supplierId;
        return this;
    }

    public CreateCollectViewModelBuilder WithSupplier(string supplier)
    {
        _supplier = supplier;
        return this;
    }

    public CreateCollectViewModelBuilder WithCollectAt(DateTime collectAt)
    {
        _collectAt = collectAt;
        return this;
    }

    public CreateCollectViewModelBuilder WithProductId(int productId)
    {
        _productId = productId;
        return this;
    }

    public CreateCollectViewModelBuilder WithProduct(string product)
    {
        _product = product;
        return this;
    }

    public CreateCollectViewModelBuilder WithVolume(int volume)
    {
        _volume = volume;
        return this;
    }

    public CreateCollectViewModelBuilder WithWeight(int weight)
    {
        _weight = weight;
        return this;
    }

    public CreateCollectViewModelBuilder WithFilialId(int filialId)
    {
        _filialId = filialId;
        return this;
    }

    public CreateCollectViewModelBuilder WithFilial(string filial)
    {
        _filial = filial;
        return this;
    }

    public CreateCollectViewModel Build()
    {
        return new CreateCollectViewModel
        {
            SupplierId = _supplierId,
            Supplier = _supplier,
            CollectAt = _collectAt,
            ProductId = _productId,
            Product = _product,
            Volume = _volume,
            Weight = _weight,
            FilialId = _filialId,
            Filial = _filial,
        };
    }
}