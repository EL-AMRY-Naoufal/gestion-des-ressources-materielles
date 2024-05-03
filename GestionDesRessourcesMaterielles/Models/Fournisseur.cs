using System.ComponentModel.DataAnnotations;

namespace GestionDesRessourcesMaterielles.Models
{
    public class Fournisseur : User
    {
        public string Name { get; set; }
        public string NomSociete { get; set; }
        public string Lieu { get; set; }
        public string Gerant { get; set; }
    }
}
