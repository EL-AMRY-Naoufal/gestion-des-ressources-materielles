using System.ComponentModel.DataAnnotations;

namespace GestionDesRessourcesMaterielles.Models
{
    public class ImprimanteCatalog
    {
        [Key]
        public string ImprimanteID { get; set; }
        public int Vitesseimpression { get; set; }
        public string Resolution { get; set; }
        public string Marque { get; set; }
    }
}
