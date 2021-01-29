using System.Windows;

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

        public MainWindow()
        {// il faut installé ca Install-Package WpfAnimatedGif -Version 2.0.0 dans package manager

            // Mon programme commence ici

            InitializeComponent();
            fenetrePrincipal = this;

            // j'affiche la page home;
            this.Content = new PageHome();

        }



    }
}
