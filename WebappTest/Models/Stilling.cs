using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebappTest.Models
{
    public class Stilling
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Navn { get; set; }
        [Required]
        [ForeignKey("Ansatt_id")]
        public int Ansatt_id { get; set; }
        [Required]
        public DateTime Periode_Start { get; set; }
        [Required]
        public DateTime Periode_Slutt { get; set; }
    }
}
