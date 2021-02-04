using System;
using System.Collections;
using System.Collections.Generic;
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
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour GestionCommande.xaml
    /// </summary>
    public partial class GestionCommande : Page
    {

        // sortedlist est une collection generique qui me permet d'avoir une list triée
        public static SortedList<Commande,String> mySL;

        // elle va contenir l'ensemble des informations qui constitu mes commands
        String[] data;

        // va me permettre de faire des operations de trie
        public delegate void OperationDeTrie();
        public void Trier(OperationDeTrie o)
        {
            o();
            ChargerLaListeBox();
            affichage.ItemsSource = data;
        }


        public static void ChargerLaListeCroissantId()
        {
            List<Commande> l = Commande.getListeCommande();
            if (l == null)
            {
                mySL = null;
            }
            else
            {
                mySL = new SortedList<Commande, String>();

                l.ForEach(x =>
                {
                    mySL.Add(x, "id commande : " + x.IdCommande + ", Prix Commande : " + x.CalculprixTotal() + " Euro , date ajout " + x.DateAjout.ToShortDateString() + " : " + x.DateAjout.ToShortTimeString() + " heure de fin :" + x.DateFin.ToShortTimeString() + " statut :" + x.Statut);
                });

            }
        }


        public static void ChargerLaListeDecroissantId()
        {
            List<Commande> l = Commande.getListeCommande();
            if (l == null)
            {
                mySL = null;
            }
            else
            {
                var comparer = Comparer<Commande>.Create((x, y) => y.IdCommande.CompareTo(x.IdCommande));
                mySL = new SortedList<Commande, String>(comparer);

                l.ForEach(x =>
                {
                    mySL.Add(x, "id commande : " + x.IdCommande + ", Prix Commande : " + x.CalculprixTotal() + " Euro , date ajout " + x.DateAjout.ToShortDateString() + " : " + x.DateAjout.ToShortTimeString() + " heure de fin :" + x.DateFin.ToShortTimeString() + " statut :" + x.Statut);
                });

            }
        }

        public static void ChargerLaListeCroissantPrix()
        {
            List<Commande> l = Commande.getListeCommande();
            if (l == null)
            {
                mySL = null;
            }
            else
            {
                var comparer = Comparer<Commande>.Create((x, y) => x.CalculprixTotal().CompareTo(y.CalculprixTotal()));
                mySL = new SortedList<Commande, String>(comparer);

                l.ForEach(x =>
                {
                    mySL.Add(x, "id commande : " + x.IdCommande + ", Prix Commande : " + x.CalculprixTotal() + " Euro , date ajout " + x.DateAjout.ToShortDateString() + " : " + x.DateAjout.ToShortTimeString() + " heure de fin :" + x.DateFin.ToShortTimeString() + " statut :" + x.Statut);
                });

            }
        }

        public static void ChargerLaListeDecroissantPrix()
        {
            List<Commande> l = Commande.getListeCommande();
            if (l == null)
            {
                mySL = null;
            }
            else
            {
                var comparer = Comparer<Commande>.Create((x, y) => y.CalculprixTotal().CompareTo(x.CalculprixTotal()));
                mySL = new SortedList<Commande, String>(comparer);

                l.ForEach(x =>
                {
                    mySL.Add(x, "id commande : " + x.IdCommande + ", Prix Commande : " + x.CalculprixTotal() + " Euro , date ajout " + x.DateAjout.ToShortDateString() + " : " + x.DateAjout.ToShortTimeString() + " heure de fin :" + x.DateFin.ToShortTimeString() + " statut :" + x.Statut);
                });

            }
        }

        //me permet de modifier le tableau data qui va être affciher
        public void ChargerLaListeBox()
        {
            int i = 0;
            if (mySL != null)
            {
                data = new string[mySL.Count];
                foreach (KeyValuePair<Commande, string> k in mySL)
                {
                    data[i] = k.Value;
                    i=i+1;
                }
            }
        }

        // mon contructeur par defautt
        public GestionCommande()
        {
            ChargerLaListeCroissantId();
            ChargerLaListeBox();
            InitializeComponent();
            affichage.ItemsSource = data;
        }


        //me permet de savoir si l'utilisateur a selectionné une commande precise pour afficher ses détails
        private void Affichage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // je récupere la liste de mes commandes
            List<Commande> l = mySL.Keys.ToList();

            #region ce bout de code me permet de récupere l'ID de la commande selectionner 
            String[] chaine = affichage.SelectedItem.ToString().Replace(" ","").Split(':');
            String[] chaine2 = chaine[1].Split(',');
            // chaine2[0] va contenir Id commande
            #endregion

            // me permet de trouvr la commande avec son ID
            Commande m = l.Find(x => x.IdCommande == Convert.ToInt32(chaine2[0]));
            if (m != null)
            {
                String detail = "Numero livreur: " + m.Livreur.Numero + ",\n Numero client: " + m.Client.Numero + "\n details commande :\n";
                m.PanierPizza.ForEach(x =>
                {
                    detail += "\t " + x.Nom.ToUpper() + " " + x.Prix.First().Nom + " x" + x.Qte+"\n";
                });

                detail += "liste des dessert  : \n";
                m.PanierDessert.ForEach(x =>
                {
                    detail += "\t " + x.Nom.ToUpper() + " x" + x.Qte+"\n";
                });
                MessageBox.Show(detail, "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // me permet de savoir si on a cliqué sur le boutton rechercher
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String chaine = numero.Text.Replace(" ","");
            if (chaine == "")
            {
                MessageBox.Show("saisir une information");
            }
            else
            {
                try
                {
                    int numero = Convert.ToInt32(chaine);
                    Commande search = Commande.GetCommande(numero);
                    if (search == null)
                    {
                        MessageBox.Show("ce numéro n'existe pas");
                    }
                    else
                    {
                        String detail = "Numero livreur: " + search.Livreur.Numero + ",\n Numero client: " + search.Client.Numero + "\n details commande :\n";
                        search.PanierPizza.ForEach(x =>
                        {
                            detail += "\t " + x.Nom.ToUpper() + " " + x.Prix.First().Nom + " x" + x.Qte + "\n";
                        });

                        detail += "liste des dessert  : \n";
                        search.PanierDessert.ForEach(x =>
                        {
                            detail += "\t " + x.Nom.ToUpper() + " x" + x.Qte + "\n";
                        });

                        MessageBox.Show(detail, "", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }catch(Exception es)
                {
                    MessageBox.Show("Vous devez saisir un numéro");
                }
            }
        }



        #region evenement de la page
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            gestion.pagePrinicipal.Content = new GestionIndex();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Trier(ChargerLaListeCroissantId);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Trier(ChargerLaListeDecroissantId);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Trier(ChargerLaListeCroissantPrix);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Trier(ChargerLaListeDecroissantPrix);
        }

        #endregion
    }
}
