using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDesRessourcesMaterielles.Models
{
    public class Besoin
    {
        private int BesoinId { get; set; }
        private int Quantite { get; set; }
        [ForeignKey("RessourceId")]
        private Ressource Ressource { get; set; }
    }
}
