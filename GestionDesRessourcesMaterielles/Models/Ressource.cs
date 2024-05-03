using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDesRessourcesMaterielles.Models
{
    public class Ressource
    {
        [Key]
        public int NumeroInventaire { get; set; }
        public DateTime DateLivraison { get; set; }
        public int FournisseurId { get; set; }
        [ForeignKey("PersonneDepartementID")]
        public PersonneDepartement? PersonneDepartement { get; set; }
        [ForeignKey("DepartementID")]
        public Departement? Departement { get; set; }
    }
}
