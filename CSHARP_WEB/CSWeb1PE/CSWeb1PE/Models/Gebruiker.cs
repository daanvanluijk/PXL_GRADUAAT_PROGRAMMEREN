using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CSWeb1PE.Models
{
    public class Gebruiker
    {
        public int GebruikerId { get; set; }
        [Required]
        public string Naam { get; set; }
        [Required]
        public string Voornaam { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public string? TijdelijkeRol { get; set; }
    }
}
