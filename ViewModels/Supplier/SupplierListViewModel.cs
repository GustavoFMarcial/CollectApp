using System.ComponentModel;

namespace CollectApp.ViewModels
{
    public class SupplierListViewModel
    {
        public int Id { get; set; }

        [DisplayName("Fornecedor")]
        public string? Name { get; set; }

        public string? CNPJ { get; set; }

        [DisplayName("Endereço")]
        public string? Address { get; set; }
    }
}