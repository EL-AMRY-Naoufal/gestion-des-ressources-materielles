using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace GestionDesRessourcesMaterielles.Models
{
    public class AppelOffre
    {
        [Key]
        public int AppelOffreID { get; set; }
        [ForeignKey("DepartementId")]
        public Departement Departement { get; set; }
        public List<Besoin> Besoins { get; set; }
    }
}
