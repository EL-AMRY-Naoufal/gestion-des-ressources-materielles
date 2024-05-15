using GestionDesRessourcesMaterielles.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace GestionDesRessourcesMaterielles.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<AppelOffre> AppelOffres { get; set; }
        public DbSet<Besoin> Besoins { get; set; }
        public DbSet<ChefDepartement> ChefDepartements { get; set; }
        public DbSet<Departement> Departements { get; set; }
        public DbSet<Fournisseur> Fournisseurs { get; set; }
        public DbSet<Imprimante> Imprimantes { get; set; }
        public DbSet<Ordinateur> Ordinateurs { get; set; }
        public DbSet<Panne> Pannes { get; set; }
        public DbSet<PersonneDepartement> PersonneDepartements { get; set; }
        public DbSet<ResponsableRessources> ResponsableRessources { get; set; }
        public DbSet<ServiceMaintenance> ServiceMaintenances { get; set; }
        public DbSet<ImprimanteCatalog> ImprimanteCatalog { get; set; }
        public DbSet<OrdinateurCatalog> OrdinateurCatalog { get; set;  }
        public DbSet<OffreFournisseur> offreFournisseurs { get; set; }
    }
}