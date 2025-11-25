using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectAppTests.Builders;

public class CollectListViewModelBuilder
{
    private int _id = 1;
    private DateTime _createdAt = new DateTime(2024, 1, 15);
    private string _userId = "user123";
    private string _fullName = "João Silva";
    private string _supplierName = "Fornecedor ABC Ltda";
    private DateTime _collectAt = new DateTime(2024, 1, 20);
    private string _productDescription = "Papel Reciclável";
    private int _volume = 100;
    private int _weigth = 50;
    private string _filial = "Filial SP Centro";
    private CollectStatus _status = CollectStatus.PendenteAprovar;
    private ChangeCollectViewModel _changeCollect = new ChangeCollectViewModelBuilder().Build();

    public CollectListViewModelBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public CollectListViewModelBuilder WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    public CollectListViewModelBuilder WithUserId(string userId)
    {
        _userId = userId;
        return this;
    }

    public CollectListViewModelBuilder WithFullName(string fullName)
    {
        _fullName = fullName;
        return this;
    }

    public CollectListViewModelBuilder WithSupplierName(string supplierName)
    {
        _supplierName = supplierName;
        return this;
    }

    public CollectListViewModelBuilder WithCollectAt(DateTime collectAt)
    {
        _collectAt = collectAt;
        return this;
    }

    public CollectListViewModelBuilder WithProductDescription(string productDescription)
    {
        _productDescription = productDescription;
        return this;
    }

    public CollectListViewModelBuilder WithVolume(int volume)
    {
        _volume = volume;
        return this;
    }

    public CollectListViewModelBuilder WithWeight(int weight)
    {
        _weigth = weight;
        return this;
    }

    public CollectListViewModelBuilder WithFilial(string filial)
    {
        _filial = filial;
        return this;
    }

    public CollectListViewModelBuilder WithStatus(CollectStatus status)
    {
        _status = status;
        return this;
    }

    public CollectListViewModelBuilder WithChangeCollect(Action<ChangeCollectViewModelBuilder> configure)
    {
        ChangeCollectViewModelBuilder ccvmb = new ChangeCollectViewModelBuilder();
        configure(ccvmb);
        _changeCollect = ccvmb.Build();
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