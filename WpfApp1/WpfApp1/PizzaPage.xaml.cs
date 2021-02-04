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
        public static bool fileChange=false;
        public static int pagePanier = 0;
        public static bool ReloadPizzaPage = false;
        public static bool ReloadDeesertPage = false;
        public static Panier panier;
        public double valeurMaximalDUnProduit;


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
                panier.Panierpizza.AjouterPizzaPanier(pc);
            }

            public void AddPanierDessert(String id)
            {
                Dessert p = new Dessert();
                foreach (Pizzeria pizzzeriia in Cata.Catalogue)
                {
                    foreach (Dessert dessert in pizzzeriia.LDessert)
                    {
                        if (dessert.Id == Convert.ToInt32(id))
                        {
                            p = dessert;
                        }
                    }
                }

                DessertComande pc = new DessertComande(p,1);
                panier.Panierdessert.AjouterDessertPanier(pc);
            }

            public void AddPanierModifierValeur(String id, String qte)
            {
                panier.Panierpizza.ModifierpanierPizza(Convert.ToInt32(id), Convert.ToInt32(qte));
            }

            public void AddPanierModifierValeurDessert(String id, String qte)
            {
                panier.Panierdessert.ModifierpanierDessert(Convert.ToInt32(id), Convert.ToInt32(qte));
            }

            public void showPizzaPage()
            {
                ReloadPizzaPage = true;
            }

            public void showDessertPage()
            {
                ReloadDeesertPage = true;
            }

            public void createCommane(String nom, String prenom, String adresse, String numero)
            {
                long n = Convert.ToInt64(numero);
                Livreur libre = Livreur.getLivreurLibre();
                if(libre == null)
                {
                    ReloadDeesertPage = true;
                    MessageBox.Show("il ya pas de livreur disponible, revenez apres");
                }
                else
                {
                    Client c = new Client(Client.getLastIdClient()+1, nom, prenom, n, adresse);
                    Client.AjouterClient(c);
                    Panier p = new Panier();
                    Commande com = new Commande(Commande.getLastIdCommande()+1,libre.IdPersonne,DateTime.Now,"cuisine",DateTime.Now.AddHours(1),p.Panierpizza.getPanierPiizaUser(),p.Panierdessert.getPanierDessertUser(), libre, c);
                    Commande.AjouterCommande(com);
                    Livreur.ModifierStatutLivreur(libre.IdPersonne, "occupe");

                    p.Panierdessert.viderpanier();
                    p.Panierpizza.viderpanier();
                }
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
            int nbrPizzaPanier = panier.getNombreElementDansPanier();
            if (nbrPizzaPanier != 0)
            {
                qte.Text = Convert.ToString(nbrPizzaPanier);
                prixPanierLabel.Content = "‎€ " + Convert.ToString(panier.getPrixPanier());
                prixPanierLabel_c.Content = "‎€ " + Convert.ToString(panier.getPrixPanier());

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
                while (!fileChange)
                {

                }
                return true;
            }
            );
            if (t)
            {
                fileChange = false;
                updateGraphiqueFonction(pagePanier);
                check();
            }

        }


        private async void checkReloadPagePizza()
        {
            /*int nbrPizzaPanier = Panier.getNombreTypePizzaPanier();
            if (nbrPizzaPanier != 0)
            {
                qte.Text = Convert.ToString(nbrPizzaPanier);
                prixPanierLabel.Content = "‎€ " + Convert.ToString(Panier.getPrixPanier());
                prixPanierLabel_c.Content = "‎€ " + Convert.ToString(Panier.getPrixPanier());

                qte.Visibility = Visibility.Visible;
                prixPanierLabel.Visibility = Visibility.Visible;
            }
            else
            {
                qte.Visibility = Visibility.Hidden;
                prixPanierLabel.Visibility = Visibility.Hidden;
            }*/
            bool t = await Task.Run(() =>
            {
                while (!ReloadPizzaPage && !ReloadDeesertPage)
                {

                }
                return true;
            }
            );
            if (t)
            {
                if (ReloadPizzaPage == true)
                {
                    ReloadPizzaPage = false;
                    wbMain.Source = new Uri("pack://siteoforigin:,,,/pizzapage.html");
                    checkReloadPagePizza();
                }

                if (ReloadDeesertPage == true)
                {
                    ReloadDeesertPage = false;
                    wbMain.Source = new Uri("pack://siteoforigin:,,,/dessertPage.html");
                    checkReloadPagePizza();
                }

            }

        }

        #endregion

        // me permet de mettre à jour le graphique de mon panier
        public void updateGraphiqueFonction(int i)
        {
            //ici je modifie graphique mon panier
            List<PizzaCommande> LC = panier.Panierpizza.getPanierPiizaUser();
            List<DessertComande> LD = panier.Panierdessert.getPanierDessertUser();

            if(LD != null){
                LD.ForEach(a =>
                {
                    PizzaCommande p = new PizzaCommande(a.Nom,a.Image, a.Prix, a.Qte);
                    LC.Add(p);
                });
            }
            if (LC != null)
            {
                LC.Reverse();
                int it = 0;
                int largeur = 70;
                int incrementationTotal = 0;
                int bases = it;
                if (LC != null && LC.Count != 0)
                {
                    boutPanier.Children.RemoveRange(0, boutPanier.Children.Count);
                    foreach (PizzaCommande p in LC)
                    {

                        if (incrementationTotal >= i)
                        {
                            it++;
                        }

                        if (it == 3)
                        {
                            break;
                        }
                        else if (it > 0)
                        {
                            Image finalImage = new Image();
                            finalImage.Width = 80;
                            BitmapImage logo = new BitmapImage();
                            logo.BeginInit();
                            logo.UriSource = new Uri(p.Image);

                            logo.EndInit();
                            finalImage.Source = logo;


                            Label label = new Label();
                            label.Height = 70;
                            label.Width = 100;
                            label.Content = p.Nom;
                            label.Background = Brushes.Transparent;

                            Label qte = new Label();
                            qte.Height = 70;
                            qte.Width = 100;
                            qte.Content = Convert.ToString(p.Qte) + " x ‎€" + Convert.ToString(p.Prix[0].Prix);
                            qte.Background = Brushes.Transparent;

                            Canvas.SetLeft(label, largeur);
                            Canvas.SetTop(label, 10);

                            Canvas.SetLeft(qte, largeur);
                            Canvas.SetTop(qte, 27);


                            largeur += 200;


                            Canvas.SetLeft(finalImage, (it - 1) * 200);
                            Canvas.SetTop(finalImage, 10);

                            label.FontWeight = FontWeights.Bold;

                            boutPanier.Children.Add(qte);
                            boutPanier.Children.Add(label);
                            boutPanier.Children.Add(finalImage);
                        }
                        incrementationTotal++;
                    }
                }
            }
        }

        public PizzaPage()
        {
            panier = new Panier();
            helper = new ObjectForScriptingHelper();

            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();

            string[] args = Environment.GetCommandLineArgs();

            fileSystemWatcher.Path = args[0].Replace("WpfApp1.exe", "");
            fileSystemWatcher.Filter = "*.txt";

            fileSystemWatcher.Changed += delegate (object sender, FileSystemEventArgs e) { fileChange = true; };


            fileSystemWatcher.EnableRaisingEvents = true;

            Cata = new CataloguePizzeria();
            InitializeComponent();
            slValue.Value = CataloguePizzeria.PrixMaximal();
            panierUp.Visibility = Visibility.Hidden;
            panierDown.Visibility = Visibility.Hidden;
            updateGraphiqueFonction(0);

            BordureboutPanier.Visibility = Visibility.Hidden;
            wbMain.ObjectForScripting = helper;
            check();
            checkReloadPagePizza();
        }

        // cette événement me permet de savoir si la page est chargé en mémoire, ensuite j'envoie mes données
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
                    // permet de verifier les pizzas déja choisi
                    foreach (Pizza pizza in p.LPizza)
                    {
                        PizzaCommande pizzaCommande = panier.Panierpizza.RechercherPizzaPanier(pizza.Id);
                        if (pizzaCommande != null)
                        {
                            pizza.Nom = pizza.Nom +
                                "||" + pizzaCommande.Qte +
                                "||" + pizzaCommande.Prix[0].Nom;
                        }
                    }

                    // permet de verifier les desserts déja choisi
                    foreach (Dessert dessert in p.LDessert)
                    {
                        DessertComande dessertCommande = panier.Panierdessert.RechercherDesserPanier(dessert.Id);
                        if (dessertCommande != null)
                        {
                            dessert.Nom = dessert.Nom + "||" + dessertCommande.Qte ;
                        }
                    }

                    lPC.Add(p);
                }

                json=JsonConvert.SerializeObject(lPC.ToArray());

                Cata = new CataloguePizzeria();
                foreach (Pizzeria p in lP)
                {
                    Cata.AjouterPizzeria(p);
                }
                if (lP != null)
                {
                    // cette fonction permet d'invoqué une méthode javascript appelé WriteFromExternals avec mon catalogue de pizzeria qui sera traité la bas
                    try
                    {
                        wbMain.InvokeScript("sendDataToJavascript", json);
                    }catch(Exception ex)
                    {

                    }
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
                panierUp.Visibility = Visibility.Visible;
                panierDown.Visibility = Visibility.Visible;
                BordureboutPanier.Visibility = Visibility.Visible;
            }
            else
            {
                panierUp.Visibility = Visibility.Hidden;
                panierDown.Visibility = Visibility.Hidden;
                BordureboutPanier.Visibility = Visibility.Hidden;
            }
            clickPanier++;
        }


        // me permet de savoir si on clickqué sur la pge pour fermer la fênettre panier
        private void pageLcik(object sender, RoutedEventArgs e)
        {

            //BordureboutPanier.Visibility = Visibility.Hidden;

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

        private void PanierUp_Click(object sender, RoutedEventArgs e)
        {
            if (pagePanier != 0)
            {
                pagePanier-=2;
                updateGraphiqueFonction(pagePanier);
            }
        }

        private void PanierDown_Click(object sender, RoutedEventArgs e)
        {
            List<PizzaCommande> LC = panier.Panierpizza.getPanierPiizaUser();
            List<DessertComande> LD = panier.Panierdessert.getPanierDessertUser();
            int o = 0;
            if (LC != null)
            {
                o += LC.Count;
            }

            if (LD != null)
            {
                o += LD.Count;
            }
            if (pagePanier+2 < o)
            {
                pagePanier += 2;
                updateGraphiqueFonction(pagePanier);
            }
        }

        private void checkOutButton_Click(object sender, RoutedEventArgs e)
        {
            // me permet d'appeler la page qui sera charger de faire la mise à jour
            wbMain.Source = new Uri("pack://siteoforigin:,,,/pageCheckOut.html");
        }


        #region me permet de faire des animations sur le boutton update
        private void Update_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = new SolidColorBrush(Color.FromArgb(36, 160, 36, 255));
        }

        private void Update_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = Brushes.Transparent;
        }
        #endregion

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //vide
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //vide
        }

        private void filtrageFunction()
        {
            double prixSelectionne = slValue.Value;
            String Recherche = "";
            Recherche += search.Text;
            List<Pizzeria> LFiltrer = new List<Pizzeria>();

            // je vais utilisez les expressions lambda pour faire le filtrage
            List<Pizzeria> LSC = Cata.Catalogue;

            

            #region elle me permet de faire le filtrage de ma liste
            LSC.ForEach(pizzeria =>
            {
                Pizzeria pTmp = new Pizzeria();
                pTmp.Emplacement = pizzeria.Emplacement;
                pTmp.Nom = pizzeria.Nom;
                pTmp.SiteWeb = pizzeria.SiteWeb;

                pizzeria.LPizza.ForEach(pizza =>
                {
                    // verifier si elle contient le prix pour les pizza
                    bool reponse = pizza.Prix.Exists(x => x.Prix <= prixSelectionne);
                    
                    if (pizza.Nom.Contains(Recherche.ToUpper()) == true && reponse == true)
                    {
                        pTmp.AjouterPizza(pizza);
                    }
                });

                pizzeria.LDessert.ForEach(dessert =>
                {
                    // verifier si elle contient le prix pour les dessert
                    if (dessert.Nom.Contains(Recherche.ToUpper()) == true && dessert.Prix<= prixSelectionne)
                    {
                        pTmp.AjouterDessert(dessert);
                    }
                });

                LFiltrer.Add(pTmp);
            });
            #endregion

            String json = JsonConvert.SerializeObject(LFiltrer.ToArray());

            // cette fonction permet d'invoqué une méthode javascript appelé WriteFromExternals avec mon catalogue de pizzeria qui sera traité la bas
            wbMain.InvokeScript("sendDataToJavascript", json);
                //
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            filtrageFunction();
        }
    }
}
