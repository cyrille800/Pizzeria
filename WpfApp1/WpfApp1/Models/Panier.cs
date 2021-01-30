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

        public static void AjouterDessertCommande(DessertComande p)
        {
            List<DessertComande> Lp = getPanierDessertUser();
            if (Lp == null)
            {
                Lp = new List<DessertComande>();
            }
            Lp.Add(p);

            string json = JsonConvert.SerializeObject(Lp.ToArray());
            System.IO.File.WriteAllText(@"panierDessert.txt", json);
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

        public static DessertComande RechercherDesserPanier(int id)
        {
            List<DessertComande> LC = getPanierDessertUser();
            if (LC != null)
            {
                foreach (DessertComande dessert in LC)
                {
                    if (dessert.Id == id)
                    {
                        return dessert;
                    }
                }
            }

            return null;
        }

        public static int getNombrePanier()
        {
            List<PizzaCommande> LC = getPanierUser();
            List<DessertComande> LD = getPanierDessertUser();

            int d = 0;
            if (LC != null)
            {
                d += LC.Count;
            }
            if(LD != null){
                d += LD.Count;
                }

            return d;
        }

        public static double getPrixPanier()
        {
            List<PizzaCommande> LC = getPanierUser();
            List<DessertComande> LD = getPanierDessertUser();
            double d = 0;
            if (LC != null)
            {
                d+= LC.ConvertAll(a => a.Prix.First().Prix*a.Qte).Sum();
            }
            if (LD != null)
            {
                d += LD.ConvertAll(a => a.Prix * a.Qte).Sum();
            }

            return d;
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

                    public static void ModifierpanierDessert(int id, int qte)
        {
            List<DessertComande> LC = getPanierDessertUser();

            if (qte != 0)
            {
                foreach (DessertComande dessert in LC)
                {
                    if (dessert.Id == id)
                    {
                        dessert.Qte = Convert.ToInt32(qte);
                    }
                }
            }
            else
            {
                List<DessertComande> LCC = new List<DessertComande>();
                foreach (DessertComande dessert in LC)
                {
                    if (dessert.Id != id)
                    {
                        LCC.Add(dessert);
                    }
                }

                LC = LCC;
            }

            string json = JsonConvert.SerializeObject(LC.ToArray());
            System.IO.File.WriteAllText(@"panierDessert.txt", json);

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

        public static List<DessertComande> getPanierDessertUser()
        {
            List<DessertComande> Lp = null;
            string curFile = @"panierDessert.txt";
            if (File.Exists(curFile) == true)
            {
                using (StreamReader r = new StreamReader("panierDessert.txt"))
                {
                    string json = r.ReadToEnd();
                    Lp = JsonConvert.DeserializeObject<List<DessertComande>>(json);
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
