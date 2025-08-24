using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;

namespace CollectApp.Models
{
    public class Collect
    {
        public int Id { get; set; }

        [DisplayName("Coleta criada")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DisplayName("Origem coleta")]
        public string? Company { get; set; }

        [DisplayName("Data coleta")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CollectAt { get; set; }

        public int? Volume { get; set; }

        [DisplayName("Peso")]
        public int? Weigth { get; set; }

        [DisplayName("Loja")]
        public string? Filial { get; set; }

        public string? Status { get; set; } = "A coletar";
    }
}