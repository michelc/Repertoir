using System.ComponentModel.DataAnnotations;

namespace Repertoir.Models
{
    public class ViewTag
    {
        public int Tag_ID { get; set; }

        [Required]
        [Display(Name = "Libellé tag")]
        [StringLength(50)]
        public string Caption { get; set; }
    }
}