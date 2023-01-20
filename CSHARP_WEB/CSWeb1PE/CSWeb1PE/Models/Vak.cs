using CSWeb1PE.Data;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSWeb1PE.Models
{
    public class Vak
    {
        public int VakId { get; set; }
        public string VakNaam { get; set; }
        public int Studiepunten { get; set; }
        [Required]
        public Handboek Handboek { get; set; }
        [ForeignKey("Handboek")]
        public int HandboekId { get; set; }
    }
}
