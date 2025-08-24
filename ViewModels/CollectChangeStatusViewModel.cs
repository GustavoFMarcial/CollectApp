using System.ComponentModel.DataAnnotations;
using CollectApp.Attributes;

namespace CollectApp.ViewModels
{
    public class CollectChangeStatusViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Status]
        public string? Status { get; set; }
    }
}