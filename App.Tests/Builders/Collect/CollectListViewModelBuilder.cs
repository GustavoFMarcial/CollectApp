using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectAppTests.Builders;

public class CollectListViewModelBuilder
{
    private int _id;
    private DateTime _createdAt;
    private string _userId = string.Empty;
    private string? _fullName;
    private string? _supplierName;
    private DateTime _collectAt;
    private string? _productDescription;
    private int? _volume;
    private int? _weigth;
    private string? _filial;
    private CollectStatus _status;
    private ChangeCollectViewModel _changeCollect = new();

    public CollectListViewModelBuilder FromCollect(Collect c)
    {
        _id = c.Id;
        _createdAt = c.CreatedAt;
        _userId = c.UserId;
        _fullName = c.User.FullName;
        _supplierName = c.Supplier.Name;
        _collectAt = c.CollectAt;
        _productDescription = c.Product.Name;
        _volume = c.Volume;
        _weigth = c.Weight;
        _filial = c.Filial.Name;
        _status = c.Status;
        _changeCollect = new ChangeCollectViewModelBuilder().FromCollect(c).Build();
        return this;
    }

    public CollectListViewModel Build()
    {
        return new CollectListViewModel
        {
            Id = _id,
            CreatedAt = _createdAt,
            UserId = _userId,
            FullName = _fullName,
            SupplierName = _supplierName,
            CollectAt = _collectAt,
            ProductDescription = _productDescription,
            Volume = _volume,
            Weigth = _weigth,
            Filial = _filial,
            Status = _status,
            ChangeCollect = _changeCollect,
        };
    }
}