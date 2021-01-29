using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using WpfApp1.Models;
using Newtonsoft.Json;

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour PizzaPage.xaml
    /// </summary>
    /// 

    public partial class PizzaPage : Page
    {
        private int clickPanier = 0;
        private static CataloguePizzeria Cata;
        public static bool fileChage=false;


        #region me permet de faire une communication javascript vers WPF de mon compasant webBrowser
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        [ComVisible(true)]
        public class ObjectForScriptingHelper
        {

            public bool chargement = true;
            public void invokewpfsincjavascript(string message)
            {
                MessageBox.Show(message);
            }

            public void AddPanier(String id,String qte,String taille,String prix)
            {
                PrixDetaille pd = new PrixDetaille(taille, Convert.ToDouble(prix.Replace(".",",")));
                Pizza p = new Pizza();
                foreach (Pizzeria pizzzeriia in Cata.Catalogue)
                {
                    foreach (Pizza pizza in pizzzeriia.LPizza)
                    {
                        if (pizza.Id == Convert.ToInt32(id))
                        {
                            p = pizza;
                            p.Prix.RemoveAll(x => true);
                            p.Prix.Add(pd);
                        }
                    }
                }

                PizzaCommande pc = new PizzaCommande(p, Convert.ToInt32(qte));
                Panier.AjouterPizzaCommande(pc);
            }

            public void AddPanierModifierValeur(String id, String qte)
            {
                Panier.Modifierpanier(Convert.ToInt32(id), Convert.ToInt32(qte));
            }

            public void startLoadPagePizza()
            {
                chargement = true;
            }
        }

        #endregion

        // utilisé pour établir la connexion entre WPF vers javascript
        private ObjectForScriptingHelper helper;

        #region fonction asynchrone utilisée pour verifier si le panier est modifier pour faire une mise à jour de l'affichage
        private async void check()
        {
            int nbrPizzaPanier = Panier.getNombreTypePizzaPanier();
            if (nbrPizzaPanier != 0)
            {
                qte.Text = Convert.ToString(nbrPizzaPanier);
                prixPanierLabel.Content = "$ " + Convert.ToString(Panier.getPrixPanier());

                qte.Visibility = Visibility.Visible;
                prixPanierLabel.Visibility = Visibility.Visible;
            }
            else
            {
                qte.Visibility = Visibility.Hidden;
                prixPanierLabel.Visibility = Visibility.Hidden;
            }
            bool t = await Task.Run(() =>
            {
                while (!fileChage)
                {

                }
                return true;
            }
            );
            if (t)
            {
                fileChage = false;
                check();
            }

        }

        #endregion

        public PizzaPage()
        {
            helper = new ObjectForScriptingHelper();

            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();

            string[] args = Environment.GetCommandLineArgs();

            fileSystemWatcher.Path = args[0].Replace("WpfApp1.exe", "");
            fileSystemWatcher.Filter = "panier.txt";

            fileSystemWatcher.Changed += delegate (object sender, FileSystemEventArgs e) { fileChage = true; };


            fileSystemWatcher.EnableRaisingEvents = true;

            Cata = new CataloguePizzeria();
            InitializeComponent();
            BordureboutPanier.Visibility = Visibility.Hidden;
            wbMain.ObjectForScripting = helper;
            check();
        }

        // cette événement me permet de savoir la page est chargé en mémoire, ensuite j'envoie mes données
        private void loadCompletePagePizza(object sender, NavigationEventArgs e)
        {
            List<Pizzeria> lP = null;

            using (StreamReader r = new StreamReader("dataPizza.txt"))
            {
                string json = r.ReadToEnd();
                lP = JsonConvert.DeserializeObject<List<Pizzeria>>(json);
                List<Pizzeria> lPC = new List<Pizzeria>();

                //permet de verifier ceux qui ont déja été choisi
                foreach (Pizzeria p in lP)
                {
                    int i = 0;
                    foreach (Pizza pizza in p.LPizza)
                    {
                        PizzaCommande pizzaCommande = Panier.RechercherPizzaPanier(pizza.Id);
                        if (pizzaCommande != null)
                        {
                            if (pizzaCommande != null)
                            {
                                pizza.Nom = p.LPizza[pizza.Id].Nom + "||" + pizzaCommande.Qte + "||" + pizzaCommande.Prix[0].Nom;
                            }
                        }

                        i++;
                    }
                    lPC.Add(p);
                }

                json=JsonConvert.SerializeObject(lPC.ToArray());


                foreach (Pizzeria p in lP)
                {
                    Cata.AjouterPizzeria(p);
                }
                if (lP != null)
                {
                    // cette fonction permet d'invoqué une méthode javascript appelé WriteFromExternals avec mon catalogue de pizzeria qui sera traité la bas
                    wbMain.InvokeScript("WriteFromExternals", json);
                    //
                }
                else
                {
                    MessageBox.Show("click on the update button of the main interface to update the program");
                }

            }
        }

        // me permet d'afficher le panier
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (clickPanier % 2 == 0)
            {
                BordureboutPanier.Visibility = Visibility.Visible;
            }
            else
            {
                BordureboutPanier.Visibility = Visibility.Hidden;
            }
            clickPanier++;
        }


        // me permet de savoir si on clickqué sur la pge pour fermer la fênettre panier
        private void pageLcik(object sender, RoutedEventArgs e)
        {

            BordureboutPanier.Visibility = Visibility.Hidden;

        }

        #region permet de faire des animation sur le boutton Panier
        private void panier_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = new SolidColorBrush(Color.FromArgb(25, 25, 36, 255));
        }

        private void panier_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = Brushes.Transparent;
        }
        #endregion

        #region permet de faire des animation sur le boutton Home
        private void HomeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = new SolidColorBrush(Color.FromArgb(25, 25, 36, 255));
        }

        private void HomeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = Brushes.Transparent;
        }
        #endregion

        // permet de retourner à la page principale
        private void Button_ClickHome(object sender, RoutedEventArgs e)
        {
            PageHome p = new PageHome();
            MainWindow.fenetrePrincipal.Content = p;
        }
    }
}
