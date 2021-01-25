


using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows;

namespace WpfApp1
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public class ObjectForScriptingHelper
    {

        public bool chargement = true;
        public void invokewpfsincjavascript(string message)
        {
            MessageBox.Show(message);
        }

        public void endLoadPagePizza()
        {
            chargement = false;
        }

        public void startLoadPagePizza()
        {
            chargement = true;
        }
    }
}