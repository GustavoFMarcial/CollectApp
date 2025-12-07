using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectAppTests.Builders;

public class ProductListViewModelBuilder
{
    private int _id;
    private string _name = string.Empty;

    public ProductListViewModelBuilder FromProduct(Product p)
    {
        _id = p.Id;
        _name = p.Name;
        return this;
    }

    public ProductListViewModel Build()
    {
        return new ProductListViewModel
        {
            Id = _id,
            Name = _name
        };
    }
}