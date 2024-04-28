namespace GestionDesRessourcesMaterielles.Models
{
    public class PersonneDepartement
    {
        private int Id { get; set; }
        private string Name { get; set; }
        private Role Role { get; set; }
        

    }

    public enum Role
    {
        enseignant,
        administratif
    }
}
