using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace OnlineBankingApp
{
    public class ContentSuccess
    {
        public static async Task<ContentDialogResult> ShowAsync(string textSuccess)
        {
            var dialog = new ContentDialog()
            {
                Title = "Успех!",
                RequestedTheme = Windows.UI.Xaml.ElementTheme.Light,
                //FullSizeDesired = true,
                MaxWidth = 400 // Required for Mobile!
            };

            var panel = new StackPanel();
            panel.Children.Add(new TextBlock
            {
                Text = "\n" + textSuccess,
                TextWrapping = Windows.UI.Xaml.TextWrapping.Wrap,
            });
            dialog.Content = panel;
            dialog.PrimaryButtonText = "OK";

            var result = await dialog.ShowAsync();
            return result;
        }
    }
}
