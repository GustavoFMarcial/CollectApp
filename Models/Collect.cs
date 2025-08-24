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

        [DisplayName("Empresa para coleta")]
        public string? Company { get; set; }

        [DisplayName("Data da coleta")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CollectAt { get; set; }

        [Required]
        public string? Status { get; set; } = "A coletar";
    }
}