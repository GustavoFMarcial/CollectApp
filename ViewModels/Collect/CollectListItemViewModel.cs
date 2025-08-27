using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CollectApp.ViewModels
{
    public class CollectListItemViewModel
    {
        public int Id { get; set; }

        [DisplayName("Criação coleta")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DisplayName("Fornecedor")]
        public string? Supplier { get; set; }

        [DisplayName("Data coleta")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CollectAt { get; set; }

        public int? Volume { get; set; }

        [DisplayName("Peso")]
        public int? Weigth { get; set; }

        [DisplayName("Loja")]
        public string? Filial { get; set; }

        public string? Status { get; set; } = "A coletar";

         public ChangeStatusCollectViewModel ChangeStatus { get; set; } = new();
    }
}