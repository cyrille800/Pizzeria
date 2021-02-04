using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class PanierPizza : IpanierInterface
    {


        public void AjouterPizzaPanier(PizzaCommande p)
        {
            List<PizzaCommande> Lp = getPanierPiizaUser();
            if (Lp == null)
            {
                Lp = new List<PizzaCommande>();
            }
            Lp.Add(p);

            string json = JsonConvert.SerializeObject(Lp.ToArray());
            System.IO.File.WriteAllText(@"panier.txt", json);
        }



        public void ModifierpanierPizza(int id, int qte)
        {
            List<PizzaCommande> LC = getPanierPiizaUser();

            if (qte != 0)
            {
                foreach (PizzaCommande pizza in LC)
                {
                    if (pizza.Id == id)
                    {
                        pizza.Qte = Convert.ToInt32(qte);
                    }
                }
            }
            else
            {
                List<PizzaCommande> LCC = new List<PizzaCommande>();
                foreach (PizzaCommande pizza in LC)
                {
                    if (pizza.Id != id)
                    {
                        LCC.Add(pizza);
                    }
                }

                LC = LCC;
            }

            string json = JsonConvert.SerializeObject(LC.ToArray());
            System.IO.File.WriteAllText(@"panier.txt", json);

        }

        public List<PizzaCommande> getPanierPiizaUser()
        {
            List<PizzaCommande> Lp = null;
            string curFile = @"panier.txt";
            if (File.Exists(curFile) == true)
            {
                using (StreamReader r = new StreamReader("panier.txt"))
                {
                    string json = r.ReadToEnd();
                    Lp = JsonConvert.DeserializeObject<List<PizzaCommande>>(json);
                    return Lp;

                }
            }
            else
            {
                return null;
            }
        }


        public int getNombreElementDansPanier()
        {
            List<PizzaCommande> LC = getPanierPiizaUser();

            int d = 0;
            if (LC != null)
            {
                d += LC.Count;
            }

            return d;
        }

        public double getPrixPanier()
        {
            List<PizzaCommande> LC = getPanierPiizaUser();

            double d = 0;
            if (LC != null)
            {
                d += LC.ConvertAll(a => a.Prix.First().Prix * a.Qte).Sum();
            }

            return d;
        }

        public PizzaCommande RechercherPizzaPanier(int id)
        {
            List<PizzaCommande> LC = getPanierPiizaUser();
            if (LC != null)
            {
                foreach (PizzaCommande pizza in LC)
                {
                    if (pizza.Id == id)
                    {
                        return pizza;
                    }
                }
            }

            return null;
        }

        public void viderpanier()
        {
            List<PizzaCommande> Lp = new List<PizzaCommande>();
            string json = JsonConvert.SerializeObject(Lp.ToArray());
            System.IO.File.WriteAllText(@"panier.txt", json);
        }

    }
}
