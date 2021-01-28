using DotNetBrowser.Engine;
using Microsoft.Toolkit.Uwp.Notifications;
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
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        LoadPage loadPage;
        
        static readonly CancellationTokenSource s_cts = new CancellationTokenSource();


        public MainWindow()
        {

            // Register COM server and activator type
            DesktopNotificationManagerCompat.RegisterActivator<MyNotificationActivator>();

            // il faut installé ca Install-Package WpfAnimatedGif -Version 2.0.0 dans package manager
            InitializeComponent();
            loadPage = null;
            List<Pizzeria> lP = null;

            using (StreamReader r = new StreamReader("dataPizza.txt"))
            {
                string json = r.ReadToEnd();
                lP = JsonConvert.DeserializeObject<List<Pizzeria>>(json);
                if (lP == null)
                {
                    MessageBox.Show("click on the update button of the main interface to update the program");
                }
            }

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            loadPage = new LoadPage();
            loadPage.Show();
        }


        private void Update_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button) sender).Background = new SolidColorBrush(Color.FromArgb(36, 160, 36, 255));
        }

        private void Update_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = Brushes.Transparent;
        }


        private void pizzaButton_Click(object sender, RoutedEventArgs e)
        {
            PizzaPage p = new PizzaPage();
            this.Content = p;
        }


        private void pizzaButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = new SolidColorBrush(Color.FromArgb(25, 25, 36, 255));
        }

        private void pizzaButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = Brushes.Transparent;
        }


    }
}
