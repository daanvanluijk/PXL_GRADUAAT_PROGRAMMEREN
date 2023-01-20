using System.ComponentModel.DataAnnotations.Schema;

namespace CSWeb1PE.Models
{
    public class VakLector
    {
        public int VakLectorId { get; set; }
        public Lector Lector { get; set; }
        [ForeignKey("Lector")]
        public int LectorId { get; set; }
        public Vak Vak { get; set; }
        [ForeignKey("Vak")]
        public int VakId { get; set; }
    }
}
