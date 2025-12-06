using CollectApp.Models;
using CollectApp.ViewModels;

namespace CollectAppTests.Builders;

public class FilialListViewModelBuilder
{
    private int _id;
    private string _name = string.Empty;

    public FilialListViewModelBuilder FromFilial(Filial f)
    {
        _id = f.Id;
        _name = f.Name;
        return this;
    }

    public FilialListViewModel Build()
    {
        return new FilialListViewModel
        {
            Id = _id,
            Name = _name,
        };
    }
}