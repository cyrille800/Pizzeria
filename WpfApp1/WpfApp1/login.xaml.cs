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
                            bi.StreamSource = new MemoryStream(System.Convert.FromBase64String(Commande.generateCodeBarCommande(c).Replace("data:image/png;base64,", "")));
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
                if (logins.Text == "admin")
                {
                    gestion page = new gestion();
                    this.Close();
                    page.Show();
                }
                else
                {
                    MessageBox.Show("vous devez saisir un numéro de telephone valide");
                }
            }

        }

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
