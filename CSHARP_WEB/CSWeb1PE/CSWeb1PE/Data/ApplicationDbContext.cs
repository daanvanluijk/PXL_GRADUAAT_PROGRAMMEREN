using CSWeb1PE.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSWeb1PE.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Gebruiker> Gebruikers { get; set; }
        public DbSet<Vak> Vakken { get; set; }
        public DbSet<Handboek> Handboeken { get; set; }
        public DbSet<Lector> Lectoren { get; set; }
        public DbSet<VakLector> VakLectoren { get; set; }
        public DbSet<Student> Studenten { get; set; }
        public DbSet<Inschrijving> Inschrijvingen { get; set; }
        public DbSet<AcademieJaar> AcademieJaren { get; set; }
    }
}
