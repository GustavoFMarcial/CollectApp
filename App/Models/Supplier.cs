namespace CollectApp.Models;

public class Supplier
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string CNPJ { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Street { get; set; }
    public string? Neighborhood { get; set; }
    public string? Number { get; set; }
    public string City { get; set; } = string.Empty;
    public string? State { get; set; }
    public string? ZipCode { get; set; }
}