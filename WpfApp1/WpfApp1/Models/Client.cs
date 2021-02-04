using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Client : Personne
    {
        String adresse;

        public Client(int idPersonne, string nom, string prenom, long numero,String adresse)
        {
            this.idPersonne = idPersonne;
            this.nom = nom;
            this.prenom = prenom;
            this.numero = numero;
            this.adresse = adresse;
        }

        public string Adresse { get => adresse; set => adresse = value; }


        public static void AjouterClient(Client l)
        {
            List<Client> Lp = getListeClient();
            if (Lp == null)
            {

                Lp = new List<Client>();
            }
            Lp.Add(l);
            string json = JsonConvert.SerializeObject(Lp.ToArray());
            System.IO.File.WriteAllText(@"client.txt", json);
        }

        public static List<Client> getListeClient()
        {
            List<Client> Lp = null;
            string curFile = @"client.txt";
            if (File.Exists(curFile) == true)
            {
                using (StreamReader r = new StreamReader("client.txt"))
                {
                    string json = r.ReadToEnd();
                    Lp = JsonConvert.DeserializeObject<List<Client>>(json);
                    return Lp;

                }

            }
            else
            {
                return null;
            }
        }

        public static int getLastIdClient()
        {
            List<Client> Lp = getListeClient();
            if (Lp == null)
            {
                return 0;
            }
            else
            {
                return Lp.Last().IdPersonne;
            }
        }
    }
}
