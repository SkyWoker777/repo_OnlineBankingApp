using OnlineBankingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace OnlineBankingApp
{
    public sealed partial class ExchangeRatesPage : Page
    {
        public ExchangeRatesPage()
        {
            this.InitializeComponent();
            Loaded += ExchangeRatesPage_Loaded;
        }

        private void ExchangeRatesPage_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new ExchangeRatesViewModel();
            cb1.SelectedIndex = 1;
            cb2.SelectedIndex = 0;
        }

        private void btn_ChangeCcy_Click(object sender, RoutedEventArgs e)
        {
            if (cb2.SelectedIndex != cb1.SelectedIndex)
            {
                var cb2_index = cb2.SelectedIndex;
                cb2.SelectedIndex = cb1.SelectedIndex;
                cb1.SelectedIndex = cb2_index;
            }
        }
    }
}
