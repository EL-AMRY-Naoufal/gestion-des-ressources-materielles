namespace GestionDesRessourcesMaterielles.Models
{
    public class Ressource
    {
        private int NumeroInventaire { get; set; }
        private DateTime DateLivraison { get; set; }
        private int FournisseurId { get; set; }
        private int? PersonneDepartement { get; set; }
        private int? Departement { get; set; }
    }
}
