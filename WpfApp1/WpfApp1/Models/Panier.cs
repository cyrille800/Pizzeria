using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    class Panier
    {

        public static void AjouterPizzaCommande(PizzaCommande p)
        {
            List<PizzaCommande> Lp = getPanierUser();
            if (Lp == null)
            {
                Lp = new List<PizzaCommande>();
            }
            Lp.Add(p);

            string json = JsonConvert.SerializeObject(Lp.ToArray());
            System.IO.File.WriteAllText(@"panier.txt", json);
        }

        public static PizzaCommande RechercherPizzaPanier(int id)
        {
            List<PizzaCommande> LC = getPanierUser();
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

        public static int getNombreTypePizzaPanier()
        {
            List<PizzaCommande> LC = getPanierUser();
            if (LC != null)
            {
                return LC.Count(a => true);
            }

            return 0;
        }

        public static double getPrixPanier()
        {
            List<PizzaCommande> LC = getPanierUser();
            if (LC != null)
            {
                return LC.ConvertAll(a => a.Prix.First().Prix*a.Qte).Sum();
            }

            return 0;
        }

        public static void Modifierpanier(int id, int qte)
        {
            List<PizzaCommande> LC = getPanierUser();

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

        public static List<PizzaCommande> getPanierUser()
        {
            List<PizzaCommande> Lp = null;
            string curFile = @"panier.txt";
            if(File.Exists(curFile) == true)
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

    }
}
