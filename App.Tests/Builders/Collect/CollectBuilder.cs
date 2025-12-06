using CollectApp.Models;

namespace CollectAppTests.Builders;

public class CollectBuilder
{
    private string _userId = "1";
    private ApplicationUser _user = new UserBuilder().Build();
    private int _supplierId = 1;
    private Supplier _supplier = new SupplierBuilder().Build();
    private DateTime _collectAt = new DateTime(2025, 12, 25);
    private int _productId = 1;
    private Product _product = new ProductBuilder().Build();
    private int _volume = 100;
    private int _weight = 50;
    private int _filialId = 1;
    private Filial _filial = new FilialBuilder().Build();
    private CollectStatus _status = CollectStatus.PendenteAprovar;

    public CollectBuilder WithUserId(string userId)
    {
        _userId = userId;
        return this;
    }

    public CollectBuilder WithUser(Action<UserBuilder> configure)
    {
        UserBuilder ub = new UserBuilder();
        configure(ub);
        _user = ub.Build();
        return this;
    }

    public CollectBuilder WithSupplierId(int supplierId)
    {
        _supplierId = supplierId;
        return this;
    }

    public CollectBuilder WithSupplier(Action<SupplierBuilder> configure)
    {
        SupplierBuilder sb = new SupplierBuilder();
        configure(sb);
        _supplier = sb.Build();
        return this;
    }

    public CollectBuilder WithCollectAt(DateTime collectAt)
    {
        _collectAt = collectAt;
        return this;
    }

    public CollectBuilder WithProductId(int productId)
    {
        _productId = productId;
        return this;
    }

    public CollectBuilder WithProduct(Action<ProductBuilder> configure)
    {
        ProductBuilder pb = new ProductBuilder();
        configure(pb);
        _product = pb.Build();
        return this;
    }

    public CollectBuilder WithVolume(int volume)
    {
        _volume = volume;
        return this;
    }

    public CollectBuilder WithWeight(int weight)
    {
        _weight = weight;
        return this;
    }

    public CollectBuilder WithFilialId(int filialId)
    {
        _filialId = filialId;
        return this;
    }

    public CollectBuilder WithFilial(Action<FilialBuilder> configure)
    {
        FilialBuilder fb = new FilialBuilder();
        configure(fb);
        _filial = fb.Build();
        return this;
    }

    public CollectBuilder WithStatus (CollectStatus status)
    {
        _status = status;
        return this;   
    }

    public Collect Build()
    {
        return new Collect
        {
            UserId = _userId,
            User = _user,
            SupplierId = _supplierId,
            Supplier = _supplier,
            CollectAt = _collectAt,
            ProductId = _productId,
            Product = _product,
            Volume = _volume,
            Weight = _weight,
            FilialId = _filialId,
            Filial = _filial,
            Status = _status,
        };
    }
};