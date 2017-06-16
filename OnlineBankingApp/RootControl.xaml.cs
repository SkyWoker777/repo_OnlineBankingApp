using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public sealed partial class RootControl : UserControl, INotifyPropertyChanged
    {
        private string titleText;
        public RootControl()
        {
            this.InitializeComponent();
            NavigationService.RootFrame = rootFrame;
            rootFrame.Navigated += OnNavigated;
        }

        public Frame RootFrame
        {
            get
            {
                return rootFrame;
            }
        }
        public string TitleText
        {
            get { return titleText; }
            set
            {
                titleText = value;
                RaisePropertyChanged("TitleText");
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            splitMenu.IsPaneOpen = false;
            panelPayments.Visibility = Visibility.Collapsed;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            if (splitMenu.IsPaneOpen)
            {
                splitMenu.IsPaneOpen = false;
                panelPayments.Visibility = Visibility.Collapsed;
            }
            else
            {
                splitMenu.IsPaneOpen = true;
            }
            //mySplitView.IsPaneOpen = !mySplitView.IsPaneOpen;
        }

        private void Menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (homeItem.IsSelected)
            {
                NavigationService.Instance.NavigateTo(typeof(HomePage));
            }
            else if (refillItem.IsSelected)
            {
                NavigationService.Instance.NavigateTo(typeof(MobileRefillPage));
                TitleTextBox.Text = "Услуги связи";
            }
            else if (ratesItem.IsSelected)
            {
                NavigationService.Instance.NavigateTo(typeof(ExchangeRatesPage));
                TitleTextBox.Text = "Курсы валют";
            }

        }

        private void OnCardPbButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Instance.NavigateTo(typeof(OnCardPrivatBankPage));
            TitleTextBox.Text = "Платежи и переводы";
        }
        private void OnCardInUkrButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Instance.NavigateTo(typeof(OnCardInUkrainePage));
            TitleTextBox.Text = "Платежи и переводы";
        }
        private void OnVisaCardButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Instance.NavigateTo(typeof(OnVisaCardAnyBankPage));
            TitleTextBox.Text = "Платежи и переводы";
        }

        private void paymentsItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (panelPayments.Visibility == Visibility.Collapsed)
            {
                panelPayments.Visibility = Visibility.Visible;
            }
            else
            {
                panelPayments.Visibility = Visibility.Collapsed;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
