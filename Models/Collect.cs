namespace CollectApp.Models
{
    public class Collect
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;
        public DateTime CollectAt { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int? Volume { get; set; }
        public int? Weigth { get; set; }
        public int FilialId { get; set; }
        public Filial Filial { get; set; } = null!;
        public string? Status { get; set; } = "Pendente aprovação";
    }
}