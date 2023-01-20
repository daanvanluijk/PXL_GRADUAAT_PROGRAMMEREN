using CSWeb1PE.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSWeb1PE.Models
{
    public class Inschrijving
    {
        public int InschrijvingId { get; set; }
        [Required]
        public Student Student { get; set; }
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        [Required]
        public VakLector VakLector { get; set; }
        [ForeignKey("VakLector")]
        public int VakLectorId { get; set; }
        public AcademieJaar AcademieJaar { get; set; }
        [ForeignKey("AcademieJaar")]
        public int AcademieJaarId { get; set; }
    }

}
