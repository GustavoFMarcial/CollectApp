using CollectApp.Models;

namespace CollectAppTests.Builders;

public class FilialBuilder
{
    private int _id = 1;
    private string _name = "Filial SP Centro";

    public FilialBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public FilialBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public Filial Build()
    {
        return new Filial
        {
            Id = _id,
            Name = _name,
        };
    }
}