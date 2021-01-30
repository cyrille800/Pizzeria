using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    class DessertComande : Dessert
    {
        private int qte;

        public int Qte
        {
            get { return qte; }
            set { qte = value; }
        }

        public Dessert Dessert
        {
            get { return new Dessert(this.Nom,this.Type,this.Image,this.Prix,this.Id ); }
        }


        public DessertComande()
        {

        }

        public DessertComande(String nom, String type, String image,double prix,int id , int qte) : base(nom, type, image, prix, id)
        {
            this.qte = qte;
        }

        public DessertComande(Dessert p, int qte) : base(p.Nom, p.Type, p.Image, p.Prix, p.Id)
        {
            this.qte = qte;
        }
    }
}
