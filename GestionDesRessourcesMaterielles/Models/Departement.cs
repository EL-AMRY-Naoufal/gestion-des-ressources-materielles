using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDesRessourcesMaterielles.Models
{
    public class Departement
    {
        private int DepartmentId { get; set; }
        private float Budget { get; set; }
        [ForeignKey("ChefDepartementID")]
        private ChefDepartement ChefDepartement { get; set; }


    }
}
