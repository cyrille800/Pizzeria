using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Livreur: Personne, IComparable<Livreur>
    {
        String statut;
        String typeVehicule;

        public string Statut { get => statut; set => statut = value; }
        public string TypeVehicule { get => typeVehicule; set => typeVehicule = value; }

        public Livreur(int idPersonne, string nom, string prenom, long numero,String statut, String typeVehicule)
        {
            this.idPersonne = idPersonne;
            this.nom = nom;
            this.prenom = prenom;
            this.numero = numero;
            this.statut = statut;
            this.typeVehicule = typeVehicule;
        }

        public static void AjouterLivreur(Livreur l)
        {
            List<Livreur> Lp = getListeLivreur();
            if (Lp == null)
            {

                Lp = new List<Livreur>();
            }
            Lp.Add(l);
            string json = JsonConvert.SerializeObject(Lp.ToArray());
            System.IO.File.WriteAllText(@"livreur.txt", json);
        }

        public static void ModifierStatutLivreur(int idLivreur,String statut)
        {
            List<Livreur> Lp = getListeLivreur();
            if (Lp != null)
            {
                Lp.ForEach(x =>
                {
                    if (x.IdPersonne == idLivreur)
                    {
                        x.statut = statut;
                    }
                });

                string json = JsonConvert.SerializeObject(Lp.ToArray());
                System.IO.File.WriteAllText(@"livreur.txt", json);
            }
        }

        public static void ModifierLivreur(int idLivreur, String nom,String prenom,long numero,String typeVelicule)
        {
            List<Livreur> Lp = getListeLivreur();
            if (Lp != null)
            {
                Lp.ForEach(x =>
                {
                    if (x.IdPersonne == idLivreur)
                    {
                        x.Nom = nom;
                        x.Prenom = prenom;
                        x.Numero = numero;
                        x.TypeVehicule = typeVelicule;
                    }
                });

                string json = JsonConvert.SerializeObject(Lp.ToArray());
                System.IO.File.WriteAllText(@"livreur.txt", json);
            }
        }

        public static List<Livreur> getListeLivreur()
        {
            List<Livreur> Lp = null;
            string curFile = @"livreur.txt";
            if (File.Exists(curFile) == true)
            {
                using (StreamReader r = new StreamReader("livreur.txt"))
                {
                    string json = r.ReadToEnd();
                    Lp = JsonConvert.DeserializeObject<List<Livreur>>(json);
                    return Lp;

                }
            }
            else
            {
                return null;
            }
        }

        public static int getLastIdLivreur()
        {
            List<Livreur> Lp = getListeLivreur();
            if (Lp == null)
            {
                return 0;
            }
            else
            {
                return Lp.Last().IdPersonne;
            }
        }

        public static Livreur getLivreurLibre()
        {
            List<Livreur> Lp = getListeLivreur();
            if (Lp == null)
            {
                return null;
            }
            else
            {
                Lp.Reverse();
                return Lp.Find( a => a.statut == "libre");
            }
        }

        public static Livreur GetLivreur(int id)
        {
            List<Livreur> Lp = getListeLivreur();
            if (Lp == null)
            {
                return null;
            }
            else
            {
                return Lp.Find(x => x.IdPersonne == id);
            }
        }

        public override bool Equals(object obj)
        {
            var livreur = obj as Livreur;
            return livreur != null &&
                   numero == livreur.numero;
        }

        public int CompareTo(Livreur other)
        {
            return idPersonne.CompareTo(other.idPersonne);
        }

        public string ToString()
        {
            return "IdPersonne : " + IdPersonne +", Nom du Livreur : " + Nom + " Prénom du Livreur : " + Prenom + " Numéro du Livreur : " + Numero + " Statut : "+ Statut + " Type de véhicule : " + typeVehicule;
        }
    }
}
