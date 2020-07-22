using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LinkScanner.ViewModels
{
    /// <summary>
    /// Page which represents information about app
    /// </summary>
    public class AboutViewModel : BaseViewModel
    {
        /// <summary>
        /// The command that is launched when you click on the email
        /// </summary>
        public ICommand EmailCommand => new Command(Email_OnClicked);

        public string EmailString => "noreply@gmail.com";

        /// <summary>
        /// Method for EmailCommand
        /// </summary>
        async void Email_OnClicked()
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = "About Link Scanner app",
                    To = new List<string> {EmailString},
                };

                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayAlert("Error"," Email is not supported on this device", "OK");
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Internal error occured", "OK");
            }
        }
    }
}
