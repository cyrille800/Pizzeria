using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    class CataloguePizzeria
    {
        private List<Pizzeria> catalogue;

        public List<Pizzeria> Catalogue
        {
            get { return catalogue; }
            set { catalogue = value; }
        }

        public CataloguePizzeria()
        {
            Catalogue = new List<Pizzeria>();
        }

        public delegate Pizzeria WebSiteScrapping(int pizzaActif);

        public Pizzeria Scrapping(WebSiteScrapping w, int pizzaActif)
        {
            return w(pizzaActif);
        }

        public void AjouterPizzeria(Pizzeria p)
        {
            catalogue.Add(p);
        }
    }
}
