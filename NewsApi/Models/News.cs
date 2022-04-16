using System.ComponentModel.DataAnnotations;

namespace NewsApi.Models
{
    public class News
    {
        [Key]
        public int NewsId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Article { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

    }
}
