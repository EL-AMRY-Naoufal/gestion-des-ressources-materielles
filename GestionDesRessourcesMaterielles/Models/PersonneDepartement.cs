using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDesRessourcesMaterielles.Models
{
    public class PersonneDepartement : User
    {
        public string Name { get; set; }

        public Role Role { get; set; }
        public string? Laboratoire { get; set; } // class ??

        [ForeignKey("DepartementID")]
        public Departement Departement { get; set; }
    }

    public enum Role
    {
        enseignant,
        administratif
    }
}
