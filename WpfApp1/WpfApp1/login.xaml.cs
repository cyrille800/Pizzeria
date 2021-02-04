using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour login.xaml
    /// </summary>
    public partial class login : Window
    {

        public static Commande c;
        public login()
        {
            InitializeComponent();
            buttonCommandeLivre.Visibility = Visibility.Hidden;
            c = null;
        }

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            MainWindow.fenetrePrincipal.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           try
            {
                long numero = Convert.ToInt64(logins.Text);
                List<Livreur> l = Livreur.getListeLivreur();

                Livreur livreur = l.Find(x => x.Numero == numero);
                if (livreur == null)
                {
                    MessageBox.Show("vous n'avez pas d'autorisation");
                }
                else
                {
                    List<Commande> ls = Commande.getListeCommande();

                    if (ls != null)
                    {
                        c = ls.Find(x => x.IdLivreur == livreur.IdPersonne && (x.Statut == "cuisine" || x.Statut == "livraison"));
                        if (c == null)
                        {
                            MessageBox.Show("vous n'avez aucune livraison à faire");
                        }
                        else
                        {
                            Commande.ModifierStatutCommande(c.IdCommande, "livraison");
                            connec.Visibility = Visibility.Hidden;
                            img.Visibility = Visibility.Hidden;
                            logins.Visibility = Visibility.Hidden;
                            //8465

                            BitmapImage bi = new BitmapImage();
                            bi.BeginInit();
                            bi.StreamSource = new MemoryStream(System.Convert.FromBase64String(login.generateCodeBarCommande(c).Replace("data:image/png;base64,", "")));
                            bi.EndInit();

                            Image imgs = new Image();
                            imgs.Source = bi;
                            imgs.Width = 200;

                            plan.Children.Add(imgs);
                            buttonCommandeLivre.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        MessageBox.Show("vous n'avez aucune livraison à faire");
                    }
                }
           }
            catch (Exception es)
            {
                MessageBox.Show("vous devez saisir un numéro de telephone valide");
            }

        }

        #region generate code bare
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
                description += ", " + x.Nom.ToUpper();
            });

            description += " liste des dessert  : ";
            c.PanierDessert.ForEach(x =>
            {
                description += ", " + x.Nom.ToUpper();
            });

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
            jse.ExecuteScript("document.getElementById('Data_Summary').setAttribute('value', '"+ titre + "')");
            jse.ExecuteScript("document.getElementById('Data_DateStart').setAttribute('value', '" + date_debut + "')");
            jse.ExecuteScript("document.getElementById('Data_TimeStart').setAttribute('value', '" + heure_debut + "')");
            jse.ExecuteScript("document.getElementById('Data_DateEnd').setAttribute('value', '" + date_fin + "')");
            jse.ExecuteScript("document.getElementById('Data_TimeEnd').setAttribute('value', '" + heure_fin + "')");


            IWebElement eltc = driver.FindElement(By.XPath("//*[@id='Data_Description']"));
            eltc.SendKeys(" "+ description);

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

        #endregion

        private void ButtonCommandeLivre_Click(object sender, RoutedEventArgs e)
        {
            if (c != null)
            {
                Commande.ModifierStatutCommande(c.IdCommande, "terminer");
                Livreur.ModifierStatutLivreur(c.IdLivreur, "libre");
                MessageBox.Show("Merci pour ton serieux ");
            }
        }
    }
}
