using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour Page1.xaml
    /// </summary>
    public partial class PageHome : Page
    {

        LoadPage loadPage;

        public PageHome()
        {
            InitializeComponent();
            loadPage = null;
            List<Pizzeria> lP = null;

            // permet d'initialiser les valeurs d'affichages du panier
            qte.Text = Convert.ToString(Panier.getNombreTypePizzaPanier());
            prixPanierLabel.Content = "$ " + Convert.ToString(Panier.getPrixPanier());

            // je stocke toutes mes informations sur les produits de type pizza dans un fichier appelé dataPizza.txt
            #region me permet de récupérer les pizzas que j'ai scrappé (Recolter)
            using (StreamReader r = new StreamReader("dataPizza.txt"))
            {
                string json = r.ReadToEnd();
                lP = JsonConvert.DeserializeObject<List<Pizzeria>>(json);
                if (lP == null)
                {
                    MessageBox.Show("click on the update button of the main interface to update the program");
                }
            }
            #endregion
        }



        private void Update_Click(object sender, RoutedEventArgs e)
        {
            // me permet d'appeler la page qui sera charger de faire la mise à jour
            loadPage = new LoadPage();
            loadPage.Show();
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


        private void pizzaButton_Click(object sender, RoutedEventArgs e)
        {
            // me permet d'affciher la page front des pizza
            PizzaPage p = new PizzaPage();
            MainWindow.fenetrePrincipal.Content = p;
        }

        #region permet de faire des animation sur le boutton pizza
        private void pizzaButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = new SolidColorBrush(Color.FromArgb(25, 25, 36, 255));
        }

        private void pizzaButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = Brushes.Transparent;
        }
        #endregion
    }
}
