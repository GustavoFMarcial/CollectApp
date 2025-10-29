namespace CollectApp.Models;

public class Product
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string Name { get; set; } = string.Empty;
}
