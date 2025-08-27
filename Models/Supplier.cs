namespace CollectApp.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? CNPJ { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
    }
}