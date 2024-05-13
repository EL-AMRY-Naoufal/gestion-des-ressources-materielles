using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDesRessourcesMaterielles.Models
{
    [Table("OrdinateurCatalog")]
    public class OrdinateurCatalog : RessourceCatalog
    {
        public string Marque { get; set; }
        public string Cpu { get; set; }
        public string Ram { get; set; }
        public string DisqueDur { get; set; }
        public string Ecran { get; set; }
        
    }
}
