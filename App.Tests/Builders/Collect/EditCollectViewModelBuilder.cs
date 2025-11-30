using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectAppTests.Builders;

public class EditCollectViewModelBuilder
{
    private int _id;
    private int _supplierId;
    private string? _supplier;
    private DateTime _collectAt;
    private int _productId;
    private string? _product;
    private int? _volume;
    private int? _weight;
    private int _filialId;
    private string? _filial;

    public EditCollectViewModelBuilder FromCollect(Collect c)
    {
        _id = c.Id;
        _supplierId = c.SupplierId;
        _supplier = c.Supplier.Name;
        _collectAt = c.CollectAt;
        _productId = c.ProductId;
        _product = c.Product.Name;
        _volume = c.Volume;
        _weight = c.Weight;
        _filialId = c.FilialId;
        _filial = c.Filial.Name;
        return this;
    }

    public EditCollectViewModelBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public EditCollectViewModelBuilder WithSupplierId(int supplierId)
    {
        _supplierId = supplierId;
        return this;
    }

    public EditCollectViewModelBuilder WithSupplier(string supplier)
    {
        _supplier = supplier;
        return this;
    }

    public EditCollectViewModelBuilder WithCollectAt(DateTime collectAt)
    {
        _collectAt = collectAt;
        return this;
    }

    public EditCollectViewModelBuilder WithProductId(int productId)
    {
        _productId = productId;
        return this;
    }

    public EditCollectViewModelBuilder WithProduct(string product)
    {
        _product = product;
        return this;
    }

    public EditCollectViewModelBuilder WithVolume(int volume)
    {
        _volume = volume;
        return this;
    }

    public EditCollectViewModelBuilder WithWeight(int weight)
    {
        _weight = weight;
        return this;
    }

    public EditCollectViewModelBuilder WithFilialId(int filialId)
    {
        _filialId = filialId;
        return this;
    }

    public EditCollectViewModelBuilder WithFilial(string filial)
    {
        _filial = filial;
        return this;
    }

    public EditCollectViewModel Build()
    {
        return new EditCollectViewModel
        {
            Id = _id,
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