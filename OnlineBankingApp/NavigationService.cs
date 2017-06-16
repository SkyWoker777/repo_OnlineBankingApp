using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OnlineBankingApp
{
    sealed class NavigationService
    {
        private Type currentPage;
        public static Frame RootFrame { get; set; }


        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            currentPage = e.SourcePageType;
        }

        public void NavigateTo(Type sourcePage)
        {
            if (currentPage != sourcePage)
            {
                RootFrame.Navigate(sourcePage);
            }
        }

        public void NavigateTo(Type sourcePage, object parameter)
        {
            if (currentPage != sourcePage)
            {
                RootFrame.Navigate(sourcePage, parameter);
            }
        }

        public void GoBack()
        {
            if (RootFrame.CanGoBack)
                RootFrame.GoBack();
            else RootFrame.Navigate(typeof(HomePage));
        }

        private void SystemNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = this.BackRequested();
            }
        }
        private bool BackRequested()
        {
            // Get a hold of the current frame so that we can inspect the app back stack
            if (RootFrame == null)
                return false;
            if (RootFrame.CanGoBack)
            {
                RootFrame.GoBack();
                return true;
            }
            return false;
        }

        private NavigationService() {
            SystemNavigationManager.GetForCurrentView().BackRequested +=
                SystemNavigationManager_BackRequested;
            RootFrame.Navigated += OnNavigated;
        }

        private static readonly Lazy<NavigationService> instance =
            new Lazy<NavigationService>(() => new NavigationService());

        public static NavigationService Instance => instance.Value;
    }
}
