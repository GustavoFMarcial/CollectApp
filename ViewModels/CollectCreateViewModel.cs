using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CollectApp.ViewModels
{
    public class CollectCreateEditViewModel
    {
        [DisplayName("Empresa para coleta")]
        [Required(ErrorMessage = "Campo empresa para coletar é obrigatório")]
        [StringLength(20, ErrorMessage = "Empresa para coleta dever ter no máximo 20 caracteres")]
        public string? Company { get; set; }

        [DisplayName("Data da coleta")]
        [Required(ErrorMessage = "Campo data de coleta é obrigatório")]
        [DataType(DataType.Date, ErrorMessage = "Insira uma data válida")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CollectAt { get; set; }

        [Range(1, 1000, ErrorMessage = "Volume deve ser entre 1 e 1000")]
        public int? Volume { get; set; }

        [DisplayName("Peso")]
        [Range(1, 20000, ErrorMessage = "Peso deve ser entre 1 e 20000")]
        public int? Weigth { get; set; }

        [DisplayName("Loja")]
        public string? Filial { get; set; }
    }
}