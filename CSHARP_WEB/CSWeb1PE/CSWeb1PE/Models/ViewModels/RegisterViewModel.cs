using Microsoft.AspNetCore.Identity;

namespace CSWeb1PE.Models.ViewModels
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Naam { get; set; }
        public string Voornaam { get; set; }
        public string Paswoord { get; set; }
        public string ConfirmPaswoord { get; set; }
        public string Role { get; set; }
    }
}
