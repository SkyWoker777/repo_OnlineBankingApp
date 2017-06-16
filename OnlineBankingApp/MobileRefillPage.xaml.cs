using OnlineBankingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel.Contacts;
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
    public sealed partial class MobileRefillPage : Page
    {
        public MobileRefillPage()
        {
            this.InitializeComponent();
            Loaded += MobileRefillPage_Loaded;
        }

        private void MobileRefillPage_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new MobileRefillViewModel();
            if (cb_operator.Items.Count != 0)
            {
                cb_operator.SelectedIndex = 0;
            }
        }
        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                this.Focus(FocusState.Programmatic);
            }
        }
        private void cb_operator_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = (MobileRefillViewModel)DataContext;
            vm.MobOperator = cb_operator.SelectedValue.ToString();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Instance.GoBack();
        }

        private async void ContactButton_Click(object sender, RoutedEventArgs e)
        {
            var contactPicker = new ContactPicker();
            contactPicker.SelectionMode = ContactSelectionMode.Fields;
            contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);

            Contact contact = await contactPicker.PickContactAsync();
            StringBuilder output = new StringBuilder();

            if (contact != null)
            {
                foreach (ContactPhone phone in contact.Phones)
                {
                    if (!phone.Number.Contains("+38"))
                    {
                        string clearPhone = phone.Number.Replace("(", "")
                            .Replace(")", "")
                            .Replace(" ", "")
                            .Replace("-","");
                        output.AppendFormat("+38{0}", clearPhone);
                    }
                    else { output.Append(phone.Number); }
                }
                if (output.Length == 13)
                {
                    tbxPhone.Text = output.ToString();
                    output.Clear();
                }
                else
                {
                    ppup.Height = Window.Current.Bounds.Height;
                    ppup.Width = Window.Current.Bounds.Width;
                    ppup.IsOpen = true;
                    tbxPhone.Text = string.Empty;
                }
            }
        }
    }
}
