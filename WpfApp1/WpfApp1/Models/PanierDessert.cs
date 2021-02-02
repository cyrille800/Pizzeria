using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class PanierDessert : panierInterface
    {

        public void AjouterDessertPanier(DessertComande p)
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

        public DessertComande RechercherDesserPanier(int id)
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


        public void ModifierpanierDessert(int id, int qte)
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

        public List<DessertComande> getPanierDessertUser()
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


        public int getNombreElementDansPanier()
        {
            List<DessertComande> LD = getPanierDessertUser();

            int d = 0;
            if (LD != null)
            {
                d += LD.Count;
            }

            return d;
        }

        public double getPrixPanier()
        {
            List<DessertComande> LD = getPanierDessertUser();
            double d = 0;
            if (LD != null)
            {
                d += LD.ConvertAll(a => a.Prix * a.Qte).Sum();
            }

            return d;
        }
        


    }
}
