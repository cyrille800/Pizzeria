using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Commande
    {
        int idCommande;
        int idLivreur;
        DateTime dateAjout;
        DateTime dateFin;
        List<PizzaCommande> panierPizza;
        List<DessertComande> panierDessert;
        Livreur livreur;
        Client client;
        String statut;

        public Commande(int idCommande, int idLivreur, DateTime dateAjout, String statut, DateTime dateFin, List<PizzaCommande> panierPizza, List<DessertComande> panierDessert, Livreur livreur, Client client)
        {
            this.idCommande = idCommande;
            this.idLivreur = idLivreur;
            this.dateAjout = dateAjout;
            this.dateFin = dateFin;
            this.panierPizza = panierPizza;
            this.panierDessert = panierDessert;
            this.livreur = livreur;
            this.client = client;
            this.statut = statut;
        }

        public int IdCommande { get => idCommande; set => idCommande = value; }
        public int IdLivreur { get => idLivreur; set => idLivreur = value; }
        public DateTime DateAjout { get => dateAjout; set => dateAjout = value; }
        public DateTime DateFin { get => dateFin; set => dateFin = value; }
        public Livreur Livreur { get => livreur; set => livreur = value; }
        public Client Client { get => client; set => client = value; }
        public String Statut { get => statut; set => statut = value; }
        public List<PizzaCommande> PanierPizza { get => panierPizza; set => panierPizza = value; }
        public List<DessertComande> PanierDessert { get => panierDessert; set => panierDessert = value; }

        public static void AjouterCommande(Commande l)
        {
            List<Commande> Lp = getListeCommande();
            if (Lp == null)
            {

                Lp = new List<Commande>();
            }
            Lp.Add(l);
            string json = JsonConvert.SerializeObject(Lp.ToArray());
            System.IO.File.WriteAllText(@"commande.txt", json);
        }

        public static List<Commande> getListeCommande()
        {
            List<Commande> Lp = null;
            string curFile = @"commande.txt";
            if (File.Exists(curFile) == true)
            {
                using (StreamReader r = new StreamReader("commande.txt"))
                {
                    string json = r.ReadToEnd();
                    Lp = JsonConvert.DeserializeObject<List<Commande>>(json);
                    return Lp;

                }
            }
            else
            {
                return null;
            }
        }


        public static int getLastIdCommande()
        {
            List<Commande> Lp = getListeCommande();
            if (Lp == null)
            {
                return 0;
            }
            else
            {
                return Lp.Last().IdCommande;
            }
        }

        public static void ModifierStatutCommande(int idCommande, String statut)
        {
            List<Commande> Lp = getListeCommande();
            if (Lp != null)
            {
                Lp.ForEach(x =>
                {
                    if (x.IdCommande == idCommande)
                    {
                        x.Statut = statut;
                    }
                });

                string json = JsonConvert.SerializeObject(Lp.ToArray());
                System.IO.File.WriteAllText(@"commande.txt", json);
            }
        }
    }
}
