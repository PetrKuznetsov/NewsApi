using System.ComponentModel.DataAnnotations;

namespace NewsApi.Models.Dto
{
    public class NewsViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Article { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
