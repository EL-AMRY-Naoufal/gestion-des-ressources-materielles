using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestionDesRessourcesMaterielles.Models
{
    public class Besoin
    {
        [Key]
        public int BesoinId { get; set; }
        [ForeignKey("PersonnedepartementId")]
        public PersonneDepartement PersonneDepartementId { get; set; }
        [ForeignKey("RessourceCatalogId")]
        public RessourceCatalog RessourceCatalogteId { get; set; }
        public int NumberOfRessource { get; set; }
        public DateTime DateRequested { get; set; }
        public bool? IsSentByChefDepartement { get; set; }
        public bool? IsApprovedByResponsableRessource { get; set; }
        [JsonIgnore]
        public AppelOffre? AppelOffre { get; set; }
    }
}
