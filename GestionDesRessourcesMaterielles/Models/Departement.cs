using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDesRessourcesMaterielles.Models
{
    public class Departement
    {
        [Key]
        public int DepartmentId { get; set; }
        public float Budget { get; set; }
        [ForeignKey("ChefDepartementID")]
        public ChefDepartement ChefDepartement { get; set; }


    }
}
