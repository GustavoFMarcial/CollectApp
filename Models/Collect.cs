using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CollectApp.Models
{
    public class Collect
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public DateTime CollectAt { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int? Volume { get; set; }
        public int? Weigth { get; set; }
        public int FilialId { get; set; }
        public Filial? Filial { get; set; }
        // public string? Filial { get; set; } 
        public string? Status { get; set; } = "A coletar";
    }
}