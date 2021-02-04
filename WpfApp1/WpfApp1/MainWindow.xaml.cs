using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Windows;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        // ma fenêtre principal
        public static MainWindow fenetrePrincipal;


        #region me permet de faire le test de la connexion internet

        public class InternetCS
        {
            //Creating the extern function...  
            [DllImport("wininet.dll")]
            private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
            //Creating a function that uses the API function...  
            public static bool IsConnectedToInternet()
            {
                int Desc;
                return InternetGetConnectedState(out Desc, 0);
            }
        }

        #endregion

        public MainWindow()
        {
           /*Livreur.AjouterLivreur(new Livreur(Livreur.getLastIdLivreur()+1, "fdsq", "jfdsi", 699875176, "libre", "velo"));
            Livreur.AjouterLivreur(new Livreur(Livreur.getLastIdLivreur()+1, "ipoiq", "zaeoi", 752272593, "libre", "velo"));
            Livreur.AjouterLivreur(new Livreur(Livreur.getLastIdLivreur()+1, "roro", "nana", 694228592, "libre", "voiture"));
            Livreur.AjouterLivreur(new Livreur(Livreur.getLastIdLivreur()+1, "popo", "dgdgd", 672748596, "congé", "moto"));
            Livreur.AjouterLivreur(new Livreur(Livreur.getLastIdLivreur() + 1, "onana", "dg", 672748587, "libre", "bus"));*/


            // il faut installé ca Install-Package WpfAnimatedGif -Version 2.0.0 dans package manager

            // Mon programme commence ici

            // permet de verfier si la connexion fonctionne
            bool networkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if(InternetCS.IsConnectedToInternet() == true)
            {
                InitializeComponent();
                fenetrePrincipal = this;

                // j'affiche la page home;
                this.Content = new PageHome();
            }
            else
            {
                MessageBox.Show("enable network");
                this.Close();
            }

        }



    }
}
