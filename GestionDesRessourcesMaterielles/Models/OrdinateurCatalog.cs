using System.ComponentModel.DataAnnotations;

namespace GestionDesRessourcesMaterielles.Models
{
    public class OrdinateurCatalog
    {
        [Key]
        public string ImprimanteID { get; set; }
        public string Marque { get; set; }
        public string Cpu { get; set; }
        public string Ram { get; set; }
        public string DisqueDur { get; set; }
        public string Ecran { get; set; }
    }
}
