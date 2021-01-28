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
        private ObjectForScriptingHelper helper = new ObjectForScriptingHelper();

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
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }

        }

        public PizzaPage()
        {
            InitializeComponent();
            wbMain.ObjectForScripting = helper;
            check();
        }

        private void loadCompletePagePizza(object sender, NavigationEventArgs e)
        {
            List<Pizzeria> lP = null;

            using (StreamReader r = new StreamReader("dataPizza.txt"))
            {
                string json = r.ReadToEnd();
                lP = JsonConvert.DeserializeObject<List<Pizzeria>>(json);
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
    }
}
