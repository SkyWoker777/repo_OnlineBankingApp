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
    public sealed partial class OnCardInUkrainePage : Page
    {
        public OnCardInUkrainePage()
        {
            this.InitializeComponent();
            this.Loaded += OnCardInUkrainePage_Loaded;
        }

        private void OnCardInUkrainePage_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new OnCardInUkraineViewModel();
            if (cb_currency.Items.Count != 0)
                cb_currency.SelectedIndex = 0;
        }

        private void cb_currency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = (OnCardInUkraineViewModel)DataContext;
            vm.Currency = (cb_currency.SelectedItem as ComboBoxItem).Content.ToString();
        }
        private void cb_cards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = (OnCardInUkraineViewModel)DataContext;
            var value = cb_cards.SelectedItem as Models.ResponseAccountBalance;
            vm.CardSender = value.Card;
        }
        private void descriptionBox_GotFocus(object sender, RoutedEventArgs e)
        {
            scroll.ChangeView(0, 50, 1);
        }
        private void descriptionBox_LostFocus(object sender, RoutedEventArgs e)
        {
            scroll.ChangeView(0, 0, 1);
        }
        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Focus(FocusState.Programmatic);
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Instance.GoBack();
        }
    }
}
