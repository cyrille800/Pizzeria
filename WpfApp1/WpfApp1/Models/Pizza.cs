using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    class Pizza
    {
        private String nom;
        private Dictionary<String, int> ingrediant;
        private String type;
        private String image;
        private Dictionary<String, double> prix;

        public String Nom
        {
            get { return nom; }
            set { nom = value;  }
        }

        public Dictionary<String, double> Prix
        {
            get { return prix; }
            set { prix = value; }
        }

        public Pizza()
        {

        }
        public String Image
        {
            get { return image; }
            set { image = value; }
        }

        public Dictionary<String, int> Ingredient
        {
            get { return ingrediant; }
            set { ingrediant = value; }
        }

        public String Type
        {
            get { return type; }
            set { type = value; }
        }

        public Pizza(String nom, String type)
        {
            ingrediant = new Dictionary<String, int>();
            this.nom = nom;
            this.prix = new Dictionary<String, double>();
            this.type = type;
        }

    }
}
