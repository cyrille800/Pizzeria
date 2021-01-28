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


        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        [ComVisible(true)]
        public class ObjectForScriptingHelper
        {

            public bool chargement = true;
            public void invokewpfsincjavascript(string message)
            {
                MessageBox.Show(message);
            }

            public void AddPanier(String id)
            {
                Pizza p = new Pizza();
                foreach(Pizzeria pizzzeriia in Cata.Catalogue)
                {
                    foreach (Pizza pizza in pizzzeriia.LPizza)
                    {
                        if(pizza.Id == Convert.ToInt32(id))
                        {
                            p = pizza;
                        }
                    }
                }

                // permet d'utiliser les notification systeme Install - Package Microsoft.Toolkit.Uwp.Notifications - Version 6.1.1
                MessageBox.Show(p.Nom);
            }

            public void startLoadPagePizza()
            {
                chargement = true;
            }
        }

        private ObjectForScriptingHelper helper;

        private async void check()
        {

            bool t = await Task.Run(() =>
            {
                while (helper.chargement)
                {

                }
                return false;
            }
            );
            if (!t)
            {
                MessageBox.Show("oui");
            }

        }

        public PizzaPage()
        {
            helper = new ObjectForScriptingHelper();

            Cata = new CataloguePizzeria();
            InitializeComponent();
            BordureboutPanier.Visibility = Visibility.Hidden;
            wbMain.ObjectForScripting = helper;
            check();
        }

        private void Webbrowser1_Navigated(object sender, NavigationEventArgs e)
        {
            //wbMain.InvokeScript("WriteFromExternals", "v");
        }

        private void loadCompletePagePizza(object sender, NavigationEventArgs e)
        {
            List<Pizzeria> lP = null;

            using (StreamReader r = new StreamReader("dataPizza.txt"))
            {
                string json = r.ReadToEnd();
                lP = JsonConvert.DeserializeObject<List<Pizzeria>>(json);
                foreach(Pizzeria p in lP)
                {
                    Cata.AjouterPizzeria(p);
                }
                if (lP != null)
                {
                    //MessageBox.Show("c");
                    wbMain.InvokeScript("WriteFromExternals", json);
                    //
                }
                else
                {
                    MessageBox.Show("click on the update button of the main interface to update the program");
                }

            }
        }

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
        

        private void pageLcik(object sender, RoutedEventArgs e)
        {

            BordureboutPanier.Visibility = Visibility.Hidden;
            
        }

        private void panier_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = new SolidColorBrush(Color.FromArgb(25, 25, 36, 255));
        }

        private void panier_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = Brushes.Transparent;
        }

    }
}
