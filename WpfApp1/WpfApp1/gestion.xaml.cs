﻿using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour gestionIndex.xaml
    /// </summary>
    public partial class gestion : Window
    {
        public static gestion pagePrinicipal;
        public gestion()
        {
            InitializeComponent();
            this.Content = new GestionIndex();
            pagePrinicipal = this;
        }
    }
}
