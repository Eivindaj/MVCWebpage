using System.ComponentModel.DataAnnotations;

namespace WebappTest.Models
{
    public class Ansatt
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Navn kan ikke være tom")]
        public string Navn { get; set; }

    }
}
