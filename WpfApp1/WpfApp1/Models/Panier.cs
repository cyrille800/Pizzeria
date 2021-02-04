using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Panier : panierInterface
    {
        PanierPizza panierpizza;
        PanierDessert panierdessert;

        public PanierPizza Panierpizza
        {
            get { return panierpizza; }
            set { panierpizza = value;  }
        }


        public PanierDessert Panierdessert
        {
            get { return panierdessert; }
            set { panierdessert = value; }
        }

        public Panier()
        {
            panierpizza = new PanierPizza();
            panierdessert = new PanierDessert();
        }

        public int getNombreElementDansPanier()
        {
            return panierpizza.getNombreElementDansPanier() + panierdessert.getNombreElementDansPanier();
        }

        public double getPrixPanier()
        {
            return panierpizza.getPrixPanier() + panierdessert.getPrixPanier();
        }

        public void viderpanier()
        {
        }
    }
}
