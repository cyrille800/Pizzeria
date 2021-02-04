using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace WpfApp1.Models
{
    class Pizzeria
    {
        private String nom;
        private double[] emplacement;
        private List<Pizza> Lpizza;
        private String siteWeb;
        private List<Dessert>  Ldessert;

        public double[] Emplacement
        {
            get { return emplacement; }
            set { emplacement = value; }
        }

        public String Nom
        {
            get { return nom; }
            set { nom = value; }
        }
        public String SiteWeb
        {
            get { return siteWeb; }
            set { siteWeb = value; }
        }

        public List<Pizza> LPizza
        {
            get { return Lpizza; }
            set { Lpizza = value; }
        }

        public List<Dessert> LDessert
        {
            get { return Ldessert; }
            set { Ldessert = value; }
        }

        public Pizzeria(String nom, String siteweb)
        {
            this.emplacement = new double[2];
            this.siteWeb = siteweb;
            this.Lpizza = new List<Pizza>();
            this.Ldessert = new List<Dessert>();
            this.nom = nom;
        }

        public Pizzeria(String nom, String siteweb, List<Pizza> l)
        {
            this.emplacement = new double[2];
            this.siteWeb = siteweb;
            this.Lpizza = l;
            this.nom = nom;
        }

        public Pizzeria()
        {
            this.Lpizza = new List<Pizza>();
            this.emplacement = new double[2];
            this.Ldessert = new List<Dessert>();
        }


        public void AjouterPizza(Pizza p)
        {
            Lpizza.Add(p);
        }

        public void AjouterDessert(Dessert p)
        {
            LDessert.Add(p);
        }

        public Boolean CheckPizza(Pizza p)
        {

            foreach (Pizza d in Lpizza)
            {
                if (d.Nom == p.Nom)
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean CheckDessert(Dessert p)
        {

            foreach (Dessert d in LDessert)
            {
                if (d.Nom == p.Nom)
                {
                    return true;
                }
            }
            return false;
        }


        // permet de scrapper les informations du site https://www.italiano-pizza.fr/index
        public static Pizzeria ItalianoPizza(int pizzaActif)
        {
            Pizzeria p = new Pizzeria();
            p.Nom = "Italiano pizza";
            p.siteWeb = "https://www.italiano-pizza.fr/index";
            double[] ki = new double[2] { 48.792860, 2.399710 };
            p.Emplacement = ki;

            // me permet de masquer l'affichage du navigateur qui va me permettre de faire le scrapping
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

            List<String> villes = new List<string>();

            // me permet d'accéder à la page
            driver.Navigate().GoToUrl(p.siteWeb);
            // me permet de trouver un élément
            IWebElement elt = driver.FindElement(By.Name("choix_ville"));
            String c = "";
            int i = 0;

            // ici je parcours l'ensemble des zones proposé par le site
            foreach (IWebElement element in elt.FindElements(By.TagName("option")))
            {
                if (i != 0)
                {
                    villes.Add(element.Text);
                    c += element.Text + "\n";
                }
                i++;
            }
            i = 0;
            // & pour chaque zone je fais ca ville = zone
            foreach (String tmp in villes)
            {

                // je vais utiliser le javascript en console, pour pouvoir exécuter des scripts
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                driver.Url = "https://www.italiano-pizza.fr/index";
                driver.Navigate();
                IWebElement eltc = driver.FindElement(By.Name("choix_ville"));
                // me permet de choisir l'option qui coorespond à la zone tmp
                eltc.SendKeys(tmp);

                // me permet de cliquer sur le boutton go
                driver.FindElement(By.CssSelector("div[class='BtnShadow btnEffects']")).FindElement(By.TagName("button")).Click();

                String[] listePage =
{
                    "https://www.italiano-pizza.fr/pizzas_creme_fraiche",
                    "https://www.italiano-pizza.fr/pizzas_tomate"
                };

                #region récupérer les pizzas
                foreach (String lien in listePage)
                {
                    // une fois que j'ai été redirigé par le go sur une autre page, j'accède à cette adresse
                    driver.Url = lien;
                    String[] l = lien.Split('/');
                    String type = l[l.Length - 1].Replace("_", " ") ;
                    driver.Navigate();

                    // je récupère l'ensemble des pizzas de la page de type pizza tomage
                    int b = Convert.ToInt32(js.ExecuteScript("return document.getElementsByClassName('papersheet').length"));
                    
                    for (int vc = 0; vc < b; vc++)
                    {
                        js.ExecuteScript("return document.getElementsByClassName('papersheet')[" + vc + "].className+='opened'");
                        Pizza piz = new Pizza();
                        piz.Id = pizzaActif;
                        piz.Nom = Convert.ToString(js.ExecuteScript("return document.getElementsByClassName('mainProd')[" + vc + "].getElementsByTagName('h2')[0].textContent"));
                        if (!p.CheckPizza(piz))
                        {

                            piz.Image = Convert.ToString(js.ExecuteScript("return document.getElementsByClassName('mainProd')[" + vc + "].getElementsByTagName('img')[0].src"));

                            String ingredient = Convert.ToString(js.ExecuteScript("return document.getElementsByClassName('mainProd')[" + vc + "].getElementsByTagName('p')[0].textContent"));

                            String[] Lingredient = ingredient.Split(',');
                            List<Ingredient> ingrediant = new List<Ingredient>();
                            String cs = "";
                            foreach (String hy in Lingredient)
                            {
                                cs += hy.Replace(".", "") + "-";
                                ingrediant.Add(new Ingredient(hy.Replace(".", ""), 1));
                            }
                            int nbrOption = Convert.ToInt32(js.ExecuteScript("return document.getElementsByClassName('mainProd')[" + vc + "].getElementsByTagName('option').length"));
                            List<PrixDetaille> prix = new List<PrixDetaille>();
                            for (int ij = 0; ij < nbrOption; ij++)
                            {
                                String px = Convert.ToString(js.ExecuteScript("return document.getElementsByClassName('mainProd')[" + vc + "].getElementsByTagName('option')[" + ij + "].textContent")).Replace("\n", "").Replace("\n", "").Replace(" ", "");
                                String[] tab = px.Split(':');
                                prix.Add(new PrixDetaille(tab[0], Convert.ToDouble(tab[1].Substring(0, tab[1].Length - 1).Replace(".", ","))));
                            }


                            piz.Ingredient = ingrediant;
                            piz.Prix = prix;
                            piz.Type = type;
                            p.AjouterPizza(piz);
                            pizzaActif++;
                        }
                    }
                }
                #endregion

                String[] listePageDessert =
                {
                    "https://www.italiano-pizza.fr/croques",
                    "https://www.italiano-pizza.fr/tex_mex",
                    "https://www.italiano-pizza.fr/salades",
                    "https://www.italiano-pizza.fr/desserts",
                    "https://www.italiano-pizza.fr/glaces",
                    "https://www.italiano-pizza.fr/boissons",
                };

                #region récupérer les desserts
                foreach (String lien in listePageDessert)
                {
                    // une fois que j'ai été redirigé par le go sur une autre page, j'accède à cette adresse
                    driver.Url = lien;
                    String[] l = lien.Split('/');
                    String type = l[l.Length - 1].Replace("_", " ");
                    driver.Navigate();

                    // je récupère l'ensemble des pizzas de la page de type pizza tomage
                    int b = Convert.ToInt32(js.ExecuteScript("return document.getElementsByClassName('papersheet').length"));

                    for (int vc = 0; vc < b; vc++)
                    {
                        js.ExecuteScript("return document.getElementsByClassName('papersheet')[" + vc + "].className+='opened'");
                        Dessert piz = new Dessert();
                        piz.Id = pizzaActif;
                        piz.Nom = Convert.ToString(js.ExecuteScript("return document.getElementsByClassName('mainProd')[" + vc + "].getElementsByTagName('h2')[0].textContent"));
                        if (!p.CheckDessert(piz))
                        {

                            piz.Image = Convert.ToString(js.ExecuteScript("return document.getElementsByClassName('mainProd')[" + vc + "].getElementsByTagName('img')[0].src"));
                            String Prix = Convert.ToString(js.ExecuteScript("return document.getElementsByClassName('mainProd')[" + vc + "].getElementsByTagName('a')[0].textContent"));
                            String[] ls = Prix.Replace("€", "").Replace(".", ",").Replace(" ", "").Split('|');
                            piz.Prix = Convert.ToDouble(ls[0]);
                            piz.Type = type;
                            p.AjouterDessert(piz);
                            pizzaActif++;
                        }
                    }
                }
                #endregion

            }

            driver.Quit();
            return p;
        }


        // permet de scrapper les informations du site https://www.allopizza94.com/index
        public static Pizzeria AlloPizza(int pizzaActif = 0)
        {
            Pizzeria p = new Pizzeria();
            p.Nom = "Allo pizza";
            p.siteWeb = "https://www.allopizza94.com/index";
            double[] ki = new double[2] { 48.789560, 2.449830 };
            p.Emplacement = ki;

            // me permet de masquer l'affichage du navigateur qui va me permettre de faire le scrapping
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

            List<String> villes = new List<string>();

            // me permet d'accéder à la page
            driver.Navigate().GoToUrl(p.siteWeb);
            // me permet de trouver un élément
            IWebElement elt = driver.FindElement(By.Name("choix_ville"));
            String c = "";
            int i = 0;

            // ici je parcours l'ensemble des zones proposé par le site
            foreach (IWebElement element in elt.FindElements(By.TagName("option")))
            {
                if (i != 0)
                {
                    villes.Add(element.Text);
                    c += element.Text + "\n";
                }
                i++;
            }
            i = 0;
            // & pour chaque zone je fais ca ville = zone
            foreach (String tmp in villes)
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                driver.Url = p.siteWeb;
                driver.Navigate();
                IWebElement eltc = driver.FindElement(By.Name("choix_ville"));
                // me permet de choisir l'option qui coorespond à la zone tmp
                eltc.SendKeys(tmp);

                // me permet de cliquer sur le boutton go
                driver.FindElement(By.CssSelector("div[class='BtnShadow btnEffects']")).FindElement(By.TagName("button")).Click();
                String[] listePage =
{
                    "https://www.allopizza94.com/pizzas_tomate.php",
                    "https://www.allopizza94.com/pizzas_creme_fraiche.php"
                };

                #region récupérer les pizzas
                foreach (String lien in listePage)
                {
                    // une fois que j'ai été redirigé par le go sur une autre page, j'accède à cette adresse
                    driver.Url = lien;
                    String[] l = lien.Split('/');
                    String type = l[l.Length - 1].Replace("_", " ");
                    driver.Navigate();

                    // je récupère l'ensemble des pizzas de la page de type pizza tomage
                    int b = Convert.ToInt32(js.ExecuteScript("return document.getElementsByClassName('papersheet').length"));

                    for (int vc = 0; vc < b; vc++)
                    {
                        js.ExecuteScript("return document.getElementsByClassName('papersheet')[" + vc + "].className+='opened'");
                        Pizza piz = new Pizza();
                        piz.Id = pizzaActif;
                        piz.Nom = Convert.ToString(js.ExecuteScript("return document.getElementsByClassName('mainProd')[" + vc + "].getElementsByTagName('h2')[0].textContent"));
                        if (!p.CheckPizza(piz))
                        {

                            piz.Image = Convert.ToString(js.ExecuteScript("return document.getElementsByClassName('mainProd')[" + vc + "].getElementsByTagName('img')[0].src"));

                            String ingredient = Convert.ToString(js.ExecuteScript("return document.getElementsByClassName('mainProd')[" + vc + "].getElementsByTagName('p')[0].textContent"));

                            String[] Lingredient = ingredient.Split(',');
                            List<Ingredient> ingrediant = new List<Ingredient>();
                            String cs = "";
                            foreach (String hy in Lingredient)
                            {
                                cs += hy.Replace(".", "") + "-";
                                ingrediant.Add(new Ingredient(hy.Replace(".", ""), 1));
                            }
                            int nbrOption = Convert.ToInt32(js.ExecuteScript("return document.getElementsByClassName('mainProd')[" + vc + "].getElementsByTagName('option').length"));
                            List<PrixDetaille> prix = new List<PrixDetaille>();
                            for (int ij = 0; ij < nbrOption; ij++)
                            {
                                String px = Convert.ToString(js.ExecuteScript("return document.getElementsByClassName('mainProd')[" + vc + "].getElementsByTagName('option')[" + ij + "].textContent")).Replace("\n", "").Replace("\n", "").Replace(" ", "");
                                String[] tab = px.Split(':');
                                prix.Add(new PrixDetaille(tab[0], Convert.ToDouble(tab[1].Substring(0, tab[1].Length - 1).Replace(".", ","))));
                            }


                            piz.Ingredient = ingrediant;
                            piz.Prix = prix;
                            piz.Type = type;
                            p.AjouterPizza(piz);
                            pizzaActif++;
                        }
                    }
                }
                #endregion

                String[] listePageDessert =
                {
                    "https://www.allopizza94.com/tex_mex",
                    "https://www.allopizza94.com/salades",
                    "https://www.italiano-pizza.fr/desserts",
                    "https://www.italiano-pizza.fr/glaces",
                    "https://www.allopizza94.com/desserts",
                };

                #region récupérer les desserts
                foreach (String lien in listePageDessert)
                {

                    // une fois que j'ai été redirigé par le go sur une autre page, j'accède à cette adresse
                    driver.Url = lien;
                    String[] l = lien.Split('/');
                    String type = l[l.Length - 1].Replace("_", " ");
                    driver.Navigate();

                    // je récupère l'ensemble des pizzas de la page de type pizza tomage
                    int b = Convert.ToInt32(js.ExecuteScript("return document.getElementsByClassName('papersheet').length"));

                    for (int vc = 0; vc < b; vc++)
                    {
                        js.ExecuteScript("return document.getElementsByClassName('papersheet')[" + vc + "].className+='opened'");
                        Dessert piz = new Dessert();
                        piz.Id = pizzaActif;
                        piz.Nom = Convert.ToString(js.ExecuteScript("return document.getElementsByClassName('mainProd')[" + vc + "].getElementsByTagName('h2')[0].textContent"));
                        if (!p.CheckDessert(piz))
                        {

                            piz.Image = Convert.ToString(js.ExecuteScript("return document.getElementsByClassName('mainProd')[" + vc + "].getElementsByTagName('img')[0].src"));
                            String Prix = Convert.ToString(js.ExecuteScript("return document.getElementsByClassName('mainProd')[" + vc + "].getElementsByTagName('a')[0].textContent"));
                            String[] ls = Prix.Replace("€", "").Replace(".", ",").Replace(" ", "").Split('|');
                            piz.Prix = Convert.ToDouble(ls[0]);
                            piz.Type = type;
                            p.AjouterDessert(piz);
                            pizzaActif++;
                        }
                    }
                }
                #endregion
            }

            driver.Quit();
            return p;
        }

    }
}
