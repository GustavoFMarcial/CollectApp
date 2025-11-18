namespace CollectApp.Helpers;

public static class LogFieldTranslations
{
    private static readonly Dictionary<string, Dictionary<string, string>> _map = new()
    {
        ["Collect"] = new Dictionary<string, string>
        {
            ["Supplier"] = "Fornecedor",
            ["CollectAt"] = "Data coleta",
            ["Product"] = "Produto",
            ["Weigth"] = "Peso",
            ["Filial"] = "Loja",
        },
        ["Supplier"] = new Dictionary<string, string>
        {
            ["Name"] = "Nome",
            ["Street"] = "Rua",
            ["Neighborhood"] = "Bairro",
            ["Number"] = "Número",
            ["City"] = "Cidade",
            ["State"] = "Estado",
            ["ZipCode"] = "CEP",
        },
        ["Product"] = new Dictionary<string, string>
        {
            ["Description"] = "Descrição",
        },
        ["Filial"] = new Dictionary<string, string>
        {
            ["Name"] = "Nome",
        },
        ["ApplicationUser"] = new Dictionary<string, string>
        {
            ["FullName"] = "Nome completo",
            ["Role"] = "Cargo",
        },
    };

    public static string Translate(string entityName, string field)
    {
        if (_map.TryGetValue(entityName, out var entityFields)
            && entityFields.TryGetValue(field, out var translated))
        {
            return translated;
        }

        return field;
    }
}
