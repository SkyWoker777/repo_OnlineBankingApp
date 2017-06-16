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
    public sealed partial class OverviewPaymentPage : Page
    {
        public OverviewPaymentPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {   
            if (e.Parameter != null)
            {
                ResponsePayment rp = new ResponsePayment();
                string sectionDb = string.Empty;
                var list = (System.Collections.ArrayList)e.Parameter;
                for(int i = 0; i < list.Count; i++)
                {
                    if(i == 1) { sectionDb = (string)list[i]; }
                    else { rp = (ResponsePayment)list[i]; }
                }
                DataContext = new PaymentStatusViewModel(rp, sectionDb);
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Instance.GoBack();
        }
    }
}
