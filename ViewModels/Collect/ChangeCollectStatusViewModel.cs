using System.ComponentModel.DataAnnotations;
using CollectApp.Models;

namespace CollectApp.ViewModels
{
    public class ChangeCollectStatusViewModel
    {
        [Required]
        public int Id { get; set; }

        public CollectStatus Status { get; set; }
    }
}