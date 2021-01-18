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
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {

            String c = "";
            foreach(Pizza p in Pizzeria.AlloPizza().LPizza)
            {
                if (p.Ingredient != null)
                {
                    foreach (KeyValuePair<string, int> kvp in p.Ingredient)
                    {
                        c += kvp.Key + "->" + kvp.Value + "\n";
                    }
                }
                
            }
            MessageBox.Show(c);

            /*string curFile = @"test.txt";
            if (File.Exists(curFile))
            {
                MessageBox.Show("oui");
            }
            else
            {
                MessageBox.Show("nn");
            }*/
        }

        private void Update_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button) sender).Background = new SolidColorBrush(Color.FromArgb(77, 147, 58, 10));
        }

        private void Update_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = Brushes.Transparent;
        }
    }
}
