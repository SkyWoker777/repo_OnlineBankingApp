using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().BackRequested +=
                SystemNavigationManager_BackRequested;
        }

        private void DeleteButtonPasswordBox_Click(object sender, RoutedEventArgs e)
        {
            tbxPassword.Password = string.Empty;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var rootControl = Window.Current.Content as RootControl;

            if (rootControl == null)
            {
                rootControl = new RootControl();
                Window.Current.Content = rootControl;
            }

            if (rootControl.RootFrame.Content == null)
            {
                rootControl.TitleText = "Главная";
                User user = new User("merchId");
                rootControl.RootFrame.Navigate(typeof(HomePage));
            }
            Window.Current.Activate();
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                this.Focus(FocusState.Programmatic);
            }
        }
        private void SystemNavigationManager_BackRequested(
            object sender,
            BackRequestedEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = this.BackRequested();
            }
        }

        private bool BackRequested()
        {
            // Get a hold of the current frame so that we can inspect the app back stack
            if (Frame == null)
                return false;

            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                return true;
            }
            return false;
        }
    }

}
