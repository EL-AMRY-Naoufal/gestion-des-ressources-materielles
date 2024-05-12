using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDesRessourcesMaterielles.Models
{
    public class ChefDepartement : User
    {
        public string Name { get; set; }
        [ForeignKey("DepartementID")]
        public Departement Departement { get; set; }
    }
}
