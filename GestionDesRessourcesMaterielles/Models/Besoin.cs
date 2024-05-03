using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDesRessourcesMaterielles.Models
{
    public class Besoin
    {
        [Key]
        public int BesoinId { get; set; }
        public int Quantite { get; set; }
        [ForeignKey("RessourceId")]
        public Ressource Ressource { get; set; }
    }
}
