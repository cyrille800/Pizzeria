using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    class PizzaCommande : Pizza
    {
        private int qte;

        public int Qte
        {
            get { return qte; }
            set { qte = value; }
        }

        public Pizza Pizza
        {
            get { return new Pizza(this.nom, this.type, this.id, this.image,this.ingrediant,this.prix); }
        }


        public PizzaCommande()
        {

        }

        public PizzaCommande(String nom, String type, int id, String image, List<Ingredient> ingrediant, List<PrixDetaille> prix,int qte) : base(nom,type,id,image,ingrediant,prix)
        {
            this.qte = qte;
        }

        public PizzaCommande(Pizza p, int qte) : base(p.Nom, p.Type, p.Id, p.Image, p.Ingredient, p.Prix)
        {
            this.qte = qte;
        }
    }
}
