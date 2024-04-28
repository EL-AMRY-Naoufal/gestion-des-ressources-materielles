namespace GestionDesRessourcesMaterielles.Models
{
    public class PersonneDepartement
    {
        private int Id { get; set; }
        private string Name { get; set; }
        private Role Role { get; set; }
        private string? Laboratoire { get; set; } // class ??
    }

    public enum Role
    {
        enseignant,
        administratif
    }
}
