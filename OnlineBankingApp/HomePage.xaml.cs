using OnlineBankingApp.Models;
using OnlineBankingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace OnlineBankingApp
{
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
            Loaded += HomePage_Loaded;
        }

        private void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new AccountBalanceViewModel();
            var rootControl = Window.Current.Content as RootControl;
            rootControl.TitleText = "Главная";
            Window.Current.Content = rootControl;
            Window.Current.Activate();
        }

        private void lstCards_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as ResponseAccountBalance;
            string cardNumber = item.Card;
            NavigationService.Instance.NavigateTo(typeof(StatementsPage), cardNumber);
        }
    }
}
