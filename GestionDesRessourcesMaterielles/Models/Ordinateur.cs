using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDesRessourcesMaterielles.Models
{
    public class Ordinateur
    {
        [Key]
        public string OdrinateurID { get; set; }
        public string Marque { get; set; }
        public string Cpu { get; set; }
        public string Ram { get; set; }
        public string DisqueDur { get; set; }
        public string Ecran { get; set; }
        [ForeignKey("RessourceId")]
        public Ressource Ressource { get; set; }
    }
}
