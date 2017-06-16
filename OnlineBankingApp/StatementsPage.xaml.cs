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
    public sealed partial class StatementsPage : Page
    {
        private string cardNumber;

        public StatementsPage()
        {
            this.InitializeComponent();
            Loaded += StatementsPage_Loaded;
        }

        private void StatementsPage_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new StatementsViewModel(cardNumber);
            var rootControl = Window.Current.Content as RootControl;
            rootControl.TitleText = "Выписка по карте";
            Window.Current.Content = rootControl;
            Window.Current.Activate();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                cardNumber = (string)e.Parameter;
            }
        }

        private void listViewContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (args.ItemIndex % 2 != 0)
            {
                args.ItemContainer.Background = new SolidColorBrush(Windows.UI.Colors.WhiteSmoke);
            }
            if (listStat.Items.Count != 0)
            {
                tb_empty.Visibility = Visibility.Collapsed;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Instance.GoBack();
        }

    }
}
