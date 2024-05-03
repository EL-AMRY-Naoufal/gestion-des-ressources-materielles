using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace GestionDesRessourcesMaterielles.Models
{
    public class AppelOffre
    {
        private int AppelOffreID { get; set; }
        [ForeignKey("DepartementId")]
        private Departement Departement { get; set; }
        private List<Besoin> Besoins { get; set; }
    }
}
