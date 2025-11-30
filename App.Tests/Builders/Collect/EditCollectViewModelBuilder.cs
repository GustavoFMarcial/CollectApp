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