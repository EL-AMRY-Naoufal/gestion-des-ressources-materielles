using System.ComponentModel.DataAnnotations;

namespace GestionDesRessourcesMaterielles.Models
{
    public abstract class RessourceCatalog
    {
        [Key]
        public string RessourceCatalogID { get; set; }
    }
}
