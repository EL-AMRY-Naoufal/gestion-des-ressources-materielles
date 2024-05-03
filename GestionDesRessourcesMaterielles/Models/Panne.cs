using System.ComponentModel.DataAnnotations;

namespace GestionDesRessourcesMaterielles.Models
{
    public class Panne
    {
        [Key]
        public int PanneId {  get; set; }
        public string Explication { get; set; }
        public DateTime DateApparition { get; set; }
        public Frequence frequence { get; set; }
        public ClassificationPanne ClassificationPanne { get; set; }
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
