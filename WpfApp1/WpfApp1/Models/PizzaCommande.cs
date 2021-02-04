using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class PizzaCommande : Pizza, Iprix
    {
        private int qte;

        public int Qte
        {
            get { return qte; }
            set { qte = value; }
        }

        public Pizza Pizza
        {
            get { return new Pizza(this.nom, this.type, this.id, this.image, this.ingrediant, this.prix); }
        }


        public PizzaCommande()
        {

        }

        public PizzaCommande(String nom, String type, int id, String image, List<Ingredient> ingrediant, List<PrixDetaille> prix, int qte) : base(nom, type, id, image, ingrediant, prix)
        {
            this.qte = qte;
        }

        public PizzaCommande(Pizza p, int qte) : base(p.Nom, p.Type, p.Id, p.Image, p.Ingredient, p.Prix)
        {
            this.qte = qte;
        }

        public PizzaCommande(String nom,String image, double prix, int qte)
        {
            this.Nom = nom;
            this.Image = image;
            List<PrixDetaille> lprix = new List<PrixDetaille>();
            lprix.Add(new PrixDetaille("vide", prix));
            this.Prix = lprix;
            this.qte = qte;
        }

        public double CalculprixTotal()
        {
            return prix.First().Prix * qte;
        }
    }
}
