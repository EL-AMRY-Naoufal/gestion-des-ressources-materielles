using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDesRessourcesMaterielles.Models
{
    public class Imprimante : Ressource
    {
        public int Vitesseimpression { get; set; }
        public string Resolution { get; set; }
        public string Marque { get; set; }
    }
}
