﻿using System.Windows.Navigation;

namespace WpfCap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowsNavigationUI = false;
        }
    }
}