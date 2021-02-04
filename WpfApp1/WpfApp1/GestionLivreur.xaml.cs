using System;
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
    /// Logique d'interaction pour GestionLivreur.xaml
    /// </summary>
    public partial class GestionLivreur : Page
    {

        // sortedlist est une collection generique qui me permet d'avoir une list triée
        public static SortedList<Livreur, String> mySL;

        public Livreur livreurSelectionner=null;

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
            List<Livreur> l = Livreur.getListeLivreur();
            if (l == null)
            {
                mySL = null;
            }
            else
            {
                mySL = new SortedList<Livreur, String>();

                l.ForEach(x =>
                {
                    mySL.Add(x, "id liveur : " + x.IdPersonne);
                }); 

            }
        }


        public static void ChargerLaListeDecroissantId()
        {
            List<Livreur> l = Livreur.getListeLivreur();
            if (l == null)
            {
                mySL = null;
            }
            else
            {
                Comparer<Livreur> comparer = Comparer<Livreur>.Create((x, y) => y.IdPersonne.CompareTo(x.IdPersonne));
                mySL = new SortedList<Livreur, String>(comparer);

                l.ForEach(x =>
                {
                    mySL.Add(x, "id liveur : " + x.IdPersonne);
                });

            }
        }

        public static void ChargerLaListeCroissantNom()
        {
            List<Livreur> l = Livreur.getListeLivreur();
            if (l == null)
            {
                mySL = null;
            }
            else
            {
                Comparer<Livreur> comparer = Comparer<Livreur>.Create((x, y) => x.Nom.CompareTo(y.Nom));
                mySL = new SortedList<Livreur, String>(comparer);

                l.ForEach(x =>
                {
                    mySL.Add(x, "id liveur : " + x.IdPersonne);
                });

            }
        }

        public static void ChargerLaListeDecroissantNom()
        {
            List<Livreur> l = Livreur.getListeLivreur();
            if (l == null)
            {
                mySL = null;
            }
            else
            {
                Comparer<Livreur> comparer = Comparer<Livreur>.Create((x, y) => y.Nom.CompareTo(x.Nom));
                mySL = new SortedList<Livreur, String>(comparer);

                l.ForEach(x =>
                {
                    mySL.Add(x, "id liveur : " + x.IdPersonne);
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
                foreach (KeyValuePair<Livreur, string> k in mySL)
                {
                    data[i] = k.Key.ToString();
                    i = i + 1;
                }
            }
        }

        // mon contructeur par defautt
        public GestionLivreur()
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
            List<Livreur> l = mySL.Keys.ToList();

            #region ce bout de code me permet de récupere l'ID de la commande selectionner 
            String[] chaine = affichage.SelectedItem.ToString().Replace(" ", "").Split(':');
            String[] chaine2 = chaine[1].Split(',');
            // chaine2[0] va contenir Id commande
            #endregion

            // me permet de trouvr la commande avec son ID
            Livreur m = l.Find(x => x.IdPersonne == Convert.ToInt32(chaine2[0]));
            if (m != null)
            {
                Nom.Text = m.Nom;
                Prenom.Text = m.Prenom;
                Numero.Text = Convert.ToString(m.Numero);
                TypedeVehicule.Text = m.TypeVehicule;
                bouttonLivreur.Content = "Modifier Livreur";
                livreurSelectionner = m;

            }
        }

        // me permet de savoir si on a cliqué sur le boutton rechercher
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String chaine = numero.Text.Replace(" ", "");
            if (chaine == "")
            {
                MessageBox.Show("saisir une information");
            }
            else
            {
                try
                {
                    int numero = Convert.ToInt32(chaine);
                    Livreur search = Livreur.GetLivreur(numero);
                    if (search == null)
                    {
                        MessageBox.Show("cet identifiant n'existe pas");
                    }
                    else
                    {
                        String detail = search.ToString();

                        MessageBox.Show(detail, "", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception es)
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
            Trier(ChargerLaListeCroissantNom);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Trier(ChargerLaListeDecroissantNom);
        }

        #endregion

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if(livreurSelectionner != null)
            {
                Livreur.ModifierLivreur(livreurSelectionner.IdPersonne,Nom.Text,Prenom.Text,Convert.ToInt64(Numero.Text), TypedeVehicule.Text);
                MessageBox.Show("opération terminer");
                bouttonLivreur.Content = "Créer Livreur";
                livreurSelectionner = null;
                gestion.pagePrinicipal.Content = new GestionLivreur();
            }
            else
            {
                Livreur.AjouterLivreur(new Livreur(Livreur.getLastIdLivreur() + 1, Nom.Text, Prenom.Text, Convert.ToInt64(Numero.Text), "libre" ,TypedeVehicule.Text));
                gestion.pagePrinicipal.Content = new GestionLivreur();
            }
        }
    }
}
