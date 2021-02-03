using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public abstract class Personne
    {
       protected int idPersonne;
       protected   String nom;
       protected  String prenom;
       protected  long numero;

        public int IdPersonne { get => idPersonne; set => idPersonne = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get => prenom; set => prenom = value; }
        public long Numero { get => numero; set => numero = value; }

        public Personne(int idPersonne, string nom, string prenom, long numero)
        {
            this.idPersonne = idPersonne;
            this.nom = nom;
            this.prenom = prenom;
            this.numero = numero;
        }

        public Personne()
        {

        }
    }
}
