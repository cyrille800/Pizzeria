using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Commande: Iprix,IComparable<Commande>
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

        #region Contructeur
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

        #endregion

        #region get & set
        public int IdCommande { get => idCommande; set => idCommande = value; }
        public int IdLivreur { get => idLivreur; set => idLivreur = value; }
        public DateTime DateAjout { get => dateAjout; set => dateAjout = value; }
        public DateTime DateFin { get => dateFin; set => dateFin = value; }
        public Livreur Livreur { get => livreur; set => livreur = value; }
        public Client Client { get => client; set => client = value; }
        public String Statut { get => statut; set => statut = value; }
        public List<PizzaCommande> PanierPizza { get => panierPizza; set => panierPizza = value; }
        public List<DessertComande> PanierDessert { get => panierDessert; set => panierDessert = value; }
        #endregion

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

        public static Commande GetCommande(int id)
        {
            List<Commande> Lp = getListeCommande();
            if (Lp == null)
            {
                return null;
            }
            else
            {
                return Lp.Find(x => x.IdCommande == id);
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

        public double CalculprixTotal()
        {
            double prix = 0.0;
            if (panierPizza != null)
            {
                prix = panierPizza.ConvertAll(x => x.CalculprixTotal()).Sum();
            }

            if (panierDessert != null)
            {
                prix += panierDessert.ConvertAll(x => x.CalculprixTotal()).Sum();
            }
            return prix;
        }


        #region generate code barre
        public static String generateCodeBarCommande(Commande c)
        {
            String lien = "https://qrcode.tec-it.com/en/Calendar";
            String titre = "commande#002" + Convert.ToString(c.IdCommande);
            String description = "";
            description += " Nom du client : " + c.Client.Nom + ",";
            description += " Prenom : " + c.Client.Prenom + ",";
            description += " Telephone : " + c.Client.Numero + ",";
            description += " Adresse  : " + c.Client.Adresse + ",";
            description += " liste des pizzas  :  ";
            c.PanierPizza.ForEach(x =>
            {
                description += ", " + x.Nom.ToUpper() + " "+x.Prix.First().Nom+" x" + x.Qte;
            });

            description += ", liste des dessert  : ";
            c.PanierDessert.ForEach(x =>
            {
                description += ", " + x.Nom.ToUpper() + " x" + x.Qte;
            });
            description += ", Total à payer  :  "+c.CalculprixTotal()+ "€";

            String date_debut = c.DateAjout.ToString("MM/dd/yyyy");
            String heure_debut = c.DateAjout.ToString("HH:mm:ss");

            String date_fin = c.DateFin.ToString("MM/dd/yyyy");
            String heure_fin = c.DateFin.ToString("HH:mm:ss");

            #region configuration du navigateur 
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("no-sandbox");
            options.AddArgument("--window-position=-32000,-32000");
            options.AddArgument("--headless");
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            ChromeDriver driver = new ChromeDriver(service, options, TimeSpan.FromMinutes(10));
            driver.Manage().Timeouts().PageLoad.Add(System.TimeSpan.FromSeconds(30));
            #endregion

            // me permet d'accéder à la page
            driver.Navigate().GoToUrl(lien);

            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;

            // je rempli les diffrents champs
            jse.ExecuteScript("document.getElementById('Data_Summary').setAttribute('value', '" + titre + "')");
            jse.ExecuteScript("document.getElementById('Data_DateStart').setAttribute('value', '" + date_debut + "')");
            jse.ExecuteScript("document.getElementById('Data_TimeStart').setAttribute('value', '" + heure_debut + "')");
            jse.ExecuteScript("document.getElementById('Data_DateEnd').setAttribute('value', '" + date_fin + "')");
            jse.ExecuteScript("document.getElementById('Data_TimeEnd').setAttribute('value', '" + heure_fin + "')");


            IWebElement eltc = driver.FindElement(By.XPath("//*[@id='Data_Description']"));
            eltc.SendKeys(" " + description);

            IWebElement elt = driver.FindElement(By.XPath("//*[@alt='QR Code']"));
            String chainedebut = elt.GetAttribute("src");

            // je génère le codebar
            driver.FindElement(By.XPath("//*[@title='Refresh Barcode']")).Click();
            // me permet de trouver un élément

            new WebDriverWait(driver, new TimeSpan(0, 0, 5))
            .Until(d => d.FindElement(By.XPath("//*[@alt='QR Code']")).GetAttribute("src") != chainedebut);
            // me permet de trouver un élément
            IWebElement elts = driver.FindElement(By.XPath("//*[@alt='QR Code']"));
            String chaine = elts.GetAttribute("src");
            return chaine;
        }

        // permet de faire un trie croissant sur l'id de la commande
        public int CompareTo(Commande other)
        {
            return idCommande.CompareTo(other.IdCommande);
        }

        #endregion
    }
}
