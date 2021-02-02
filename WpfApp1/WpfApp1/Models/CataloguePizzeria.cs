using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    class CataloguePizzeria
    {
        private List<Pizzeria> catalogue;

        public static double PrixMaximal()
        {
            List<Pizzeria> l = new List<Pizzeria>();
            using (StreamReader r = new StreamReader("dataPizza.txt"))
            {
                string json = r.ReadToEnd();
                l = JsonConvert.DeserializeObject<List<Pizzeria>>(json);

                double prixInf = 0;
                foreach (Pizzeria p in l)
                {
                    foreach (Pizza pizza in p.LPizza)
                    {
                        foreach (PrixDetaille prix in pizza.Prix)
                        {
                            if (prix.Prix > prixInf)
                            {
                                prixInf = prix.Prix;
                            }
                        }
                    }

                    foreach (Dessert dessert in p.LDessert)
                    {
                        if (dessert.Prix > prixInf)
                        {
                            prixInf = dessert.Prix;
                        }
                    }
                }
                return prixInf;
            }
        }

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
