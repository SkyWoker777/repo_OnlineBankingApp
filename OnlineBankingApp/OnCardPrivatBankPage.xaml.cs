using OnlineBankingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class OnCardPrivatBankPage : Page
    {

        public OnCardPrivatBankPage()
        {
            this.InitializeComponent();
            this.Loaded += OnCardPrivatBankPage_Loaded;
        }

        private void OnCardPrivatBankPage_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new OnCardPrivatBankViewModel();
            if (cb_currency.Items.Count != 0)
                cb_currency.SelectedIndex = 0;
        }

        private void cb_currency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = (OnCardPrivatBankViewModel)DataContext;
            vm.Currency = (cb_currency.SelectedItem as ComboBoxItem).Content.ToString();
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (e.OriginalSource as MenuFlyoutItem).DataContext as Models.ResponseAccountBalance;
            tbxRecipient.Text = item.Card;
        }
        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Focus(FocusState.Programmatic);
            }
        }
        private void cb_cards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = (OnCardPrivatBankViewModel)DataContext;
            var value = cb_cards.SelectedItem as Models.ResponseAccountBalance;
            vm.CardSender = value.Card;
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Instance.GoBack();
        }
    }
}
