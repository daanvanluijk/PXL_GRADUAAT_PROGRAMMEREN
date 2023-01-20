using System.ComponentModel.DataAnnotations.Schema;

namespace CSWeb1PE.Models
{
    public class Lector
    {
        public int LectorId { get; set; }
        public Gebruiker Gebruiker { get; set; }
        [ForeignKey("Gebruiker")]
        public int GebruikerId { get; set; }
    }
}
