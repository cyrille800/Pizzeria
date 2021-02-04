using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Dessert
    {
        protected String nom;
        protected String type;
        protected String image;
        protected Double prix;
        protected int id;


        public String Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        public String Type
        {
            get { return type; }
            set { type = value; }
        }

        public String Image
        {
            get { return image; }
            set { image = value; }
        }

        public double Prix
        {
            get { return prix; }
            set { prix = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public Dessert()
        {

        }

        public Dessert(String nom,String type,String image,double prix,int id)
        {
            this.nom = nom;
            this.type = type;
            this.image = image;
            this.prix = prix;
            this.id = id;
        }
    }
}
