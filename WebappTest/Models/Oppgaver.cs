using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebappTest.Models
{
    public class Oppgaver
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Navn { get; set; }
        [Required]
        [ForeignKey("Ansatt_id")]
        public int Ansatt_id { get; set; }
        [Required]
        public DateTime Dato { get; set; }
    }
}
