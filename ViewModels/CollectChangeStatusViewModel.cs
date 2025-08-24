using System.ComponentModel.DataAnnotations;
using CollectApp.Attributes;

namespace CollectApp.ViewModels
{
    public class ChangeStatusCollectViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StatusValidation]
        public string? Status { get; set; }
    }
}