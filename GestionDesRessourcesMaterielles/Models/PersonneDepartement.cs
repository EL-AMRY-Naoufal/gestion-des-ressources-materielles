using System.ComponentModel.DataAnnotations;

namespace GestionDesRessourcesMaterielles.Models
{
    public class PersonneDepartement : User
    {
        public string Name { get; set; }

        public Role Role { get; set; }
        public string? Laboratoire { get; set; } // class ??

    }

    public enum Role
    {
        enseignant,
        administratif
    }
}
