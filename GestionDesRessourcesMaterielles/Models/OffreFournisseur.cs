using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestionDesRessourcesMaterielles.Models
{
    public class OffreFournisseur
    {
        [Key]
        public int OffreFournisseurId { get; set; }

        [ForeignKey("FournisseurID")]
        public Fournisseur FournisseurId { get; set; }

        [ForeignKey("AppelOffreID")]
        public AppelOffre AppelOffreId { get; set; }
        public double Montant { get; set; }
        public DateTime DateSoumission { get; set; }
        public bool? IsAccepted { get; set; }
        public bool IsApproved { get; set; } = false;
    }
}
