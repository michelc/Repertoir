using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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

    public class ReplaceTag
    {
        public int Tag_ID { get; set; }

        [Display(Name = "Tag actuel")]
        public string Caption { get; set; }

        [Display(Name = "A remplacer par")]
        public int Other_ID { get; set; }

        public SelectList Tags { get; set; }
    }
}