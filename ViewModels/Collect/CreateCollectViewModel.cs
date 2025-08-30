using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CollectApp.Attributes;
using CollectApp.Models;

namespace CollectApp.ViewModels
{
    public class CreateCollectViewModel
    {
        [Required]
        public int SupplierId { get; set; }

        [DisplayName("Fornecedor")]
        // [SupplierValidation]
        [StringLength(20, ErrorMessage = "Empresa para coleta dever ter no máximo 20 caracteres")]
        public Supplier? Supplier { get; set; }

        [DisplayName("Data coleta")]
        [Required(ErrorMessage = "Campo data de coleta é obrigatório")]
        [DataType(DataType.Date, ErrorMessage = "Insira uma data válida")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CollectAt { get; set; }

        [Required]
        public int ProductId { get; set; }

        [DisplayName("Produto")]
        public Product? Product { get; set; }

        [Range(1, 1000, ErrorMessage = "Volume deve ser entre 1 e 1000")]
        public int? Volume { get; set; }

        [DisplayName("Peso")]
        [Range(1, 20000, ErrorMessage = "Peso deve ser entre 1 e 20000")]
        public int? Weight { get; set; }

        [DisplayName("Loja")]
        [Required(ErrorMessage = "Campo Loja é obrigatório")]
        [FilialValidation]
        public string? Filial { get; set; }

        public List<Product>? ProductsList { get; set; }
        public List<Supplier>? SuppliersList { get; set; }
    }
}