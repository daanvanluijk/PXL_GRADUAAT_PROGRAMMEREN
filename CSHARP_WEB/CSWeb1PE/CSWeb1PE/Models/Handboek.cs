using CSWeb1PE.ModelValidation;
using System.ComponentModel.DataAnnotations;

namespace CSWeb1PE.Models
{
    public class Handboek
    {
        public int HandboekId { get; set; }
        public string Titel { get; set; }
        public int Kostprijs { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [MinimumDate(1980, 1, 1)]
        [MaximumDateCurrentYear(1, 1)]
        public DateTime UitgifteDatum { get; set; }
        public string Afbeelding { get; set; }
    }
}
