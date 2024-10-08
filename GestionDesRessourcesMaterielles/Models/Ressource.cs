﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDesRessourcesMaterielles.Models
{
    public abstract class Ressource
    {
        [Key]
        public int RessourceId { get; set; }
        public DateTime DateLivraison { get; set; }
        [ForeignKey("FournisseurID")]
        public Fournisseur FournisseurId { get; set; }
        [ForeignKey("PersonneDepartementID")]
        public PersonneDepartement? PersonneDepartement { get; set; }
        [ForeignKey("DepartementID")]
        public Departement? Departement { get; set; }
        public bool? Livred { get; set; }
    }
}
