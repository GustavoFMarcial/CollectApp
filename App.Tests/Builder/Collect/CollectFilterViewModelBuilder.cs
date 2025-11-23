using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectAppTests.Builders;

public class CollectFilterViewModelBuilder
{
    private int _id = 1;
    private DateTime _startCreation = new DateTime(2024, 1, 15);
    private DateTime _endCreation = new DateTime(2024, 1, 15);
    private string _user = "João Silva";
    private string _supplier = "Fornecedor ABC Ltda";
    private DateTime _startCollect = new DateTime(2024, 1, 20);
    private DateTime _endCollect = new DateTime(2024, 1, 20);
    private string _product = "Papel Reciclável";
    private CollectStatus _status = CollectStatus.PendenteAprovar;
    private int _volume = 100;
    private int _weight = 50;
    private string _filial = "Filial SP Centro";

    public CollectFilterViewModelBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public CollectFilterViewModelBuilder WithStartCreation(DateTime startCreation)
    {
        _startCreation = startCreation;
        return this;
    }

    public CollectFilterViewModelBuilder WithEndCreation(DateTime endCreation)
    {
        _endCreation = endCreation;
        return this;
    }

    public CollectFilterViewModelBuilder WithUser(string user)
    {
        _user = user;
        return this;
    }

    public CollectFilterViewModelBuilder WithSupplier(string supplier)
    {
        _supplier = supplier;
        return this;
    }

    public CollectFilterViewModelBuilder WithStartCollect(DateTime startCollect)
    {
        _startCollect = startCollect;
        return this;
    }

    public CollectFilterViewModelBuilder WithEndCollect(DateTime endCollect)
    {
        _endCollect = endCollect;
        return this;
    }

    public CollectFilterViewModelBuilder WithProduct(string product)
    {
        _product = product;
        return this;
    }

    public CollectFilterViewModelBuilder WithCollectStatus(CollectStatus status)
    {
        _status = status;
        return this;
    }

    public CollectFilterViewModelBuilder WithVolume(int volume)
    {
        _volume = volume;
        return this;
    }

    public CollectFilterViewModelBuilder WithWeight(int weight)
    {
        _weight = weight;
        return this;
    }

    public CollectFilterViewModelBuilder WithFilial(string filial)
    {
        _filial = filial;
        return this;
    }

    public CollectFilterViewModel Build()
    {
        return new CollectFilterViewModel
        {
            Id = _id,
            StartCreation = _startCreation,
            EndCreation = _endCreation,
            User = _user,
            Supplier = _supplier,
            StartCollect = _startCollect,
            EndCollect = _endCollect,
            Product = _product,
            Status = _status,
            Volume = _volume,
            Weight = _weight,
            Filial = _filial,
        };
    }
}