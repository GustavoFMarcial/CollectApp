using System.ComponentModel.DataAnnotations;

namespace CollectApp.ViewModels
{
    public class CollectChangeStatusViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string? Status { get; set; }
    }
}