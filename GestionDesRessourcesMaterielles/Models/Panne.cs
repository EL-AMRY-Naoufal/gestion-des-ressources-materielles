namespace GestionDesRessourcesMaterielles.Models
{
    public class Panne
    {
        private int PanneId {  get; set; }
        private string Explication { get; set; }
        private DateTime DateApparition { get; set; }
        private Frequence frequence { get; set; }
        private ClassificationPanne ClassificationPanne { get; set; }
    }

    public enum Frequence
    {
        rare,
        frequente,
        permanente
    }

    public enum ClassificationPanne
    {
        DefautSysteme,
        LogicielUtilitaire,
        Materiel
    }
}
