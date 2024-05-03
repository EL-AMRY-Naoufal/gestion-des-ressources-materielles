using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDesRessourcesMaterielles.Models
{
    public class Ressource
    {
        private int NumeroInventaire { get; set; }
        private DateTime DateLivraison { get; set; }
        private int FournisseurId { get; set; }
        [ForeignKey("PersonneDepartementID")]
        private PersonneDepartement? PersonneDepartement { get; set; }
        [ForeignKey("DepartementID")]
        private Departement? Departement { get; set; }
    }
}
