using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour LoadPage.xaml
    /// </summary>


    public partial class LoadPage : Window
    {

        private static LoadPage currentPage;
        private System.Threading.Timer timerValidation = null;
        private TimerCallback timerDelegate = new TimerCallback(Timer_Callback);
        public static bool fini = false;

        private static void Timer_Callback(object obj)
        {
            System.Threading.Timer timer = (System.Threading.Timer)obj;
            validationFinale();
            fini = true;

        }

        private static void validationFinale()
        {
            CataloguePizzeria C = new CataloguePizzeria();

            C.AjouterPizzeria(C.Scrapping(Pizzeria.ItalianoPizza, 0));
            C.AjouterPizzeria(C.Scrapping(Pizzeria.AlloPizza, C.Catalogue.First().LPizza.Count));


            string json = JsonConvert.SerializeObject(C.Catalogue.ToArray());
            System.IO.File.WriteAllText(@"dataPizza.txt", json);
        }

        private async void check()
        {

            bool t = await Task.Run(() =>
            {
                while (!fini)
                {

                }
                return true;
            }
            );
            if (t)
            {
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }

        }

        public LoadPage()
        {
            InitializeComponent();
            currentPage = this;
            timerValidation = new System.Threading.Timer(timerDelegate, timerValidation, 500, Timeout.Infinite);
            check();
        }
    }
}
