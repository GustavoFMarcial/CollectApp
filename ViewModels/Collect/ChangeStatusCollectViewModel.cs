using System.ComponentModel.DataAnnotations;

namespace CollectApp.ViewModels
{
    public class ChangeStatusCollectViewModel
    {
        [Required]
        public int Id { get; set; }
    }
}