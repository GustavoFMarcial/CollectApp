using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CollectApp.Models
{
    public class Collect
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? Supplier { get; set; }
        public DateTime CollectAt { get; set; }
        public int? Volume { get; set; }
        public int? Weigth { get; set; }
        public string? Filial { get; set; } 
        public string? Status { get; set; } = "A coletar";
    }
}