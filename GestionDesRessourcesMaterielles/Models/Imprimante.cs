using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDesRessourcesMaterielles.Models
{
    public class Imprimante
    {
        [Key]
        public string ImprimanteID { get; set; }
        public int Vitesseimpression { get; set; }
        public string Resolution { get; set; }
        public string Marque { get; set; }
        [ForeignKey("RessourceId")]
        public Ressource Ressource { get; set; }
    }
}
