using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBankingApp
{
    public class ContentError
    {
        public static async void Show(string textError)
        {
            var dialog = new Windows.UI.Xaml.Controls.ContentDialog()
            {
                Title = "Упс!",
                RequestedTheme = Windows.UI.Xaml.ElementTheme.Dark,
                //FullSizeDesired = true,
                MaxWidth = 400 // Required for Mobile!
            };

            var panel = new Windows.UI.Xaml.Controls.StackPanel();
            panel.Children.Add(new Windows.UI.Xaml.Controls.TextBlock
            {
                Text = "\n"+ textError,
                TextWrapping = Windows.UI.Xaml.TextWrapping.Wrap,
            });
            dialog.Content = panel;
            dialog.PrimaryButtonText = "OK";

            var result = await dialog.ShowAsync();
        }
    }
}
