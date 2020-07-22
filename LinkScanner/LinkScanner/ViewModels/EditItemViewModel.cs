using System;
using System.Windows.Input;
using LinkScanner.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LinkScanner.ViewModels
{
    /// <summary>
    /// ViewModel of the EditItemPage
    /// </summary>
    public class EditItemViewModel : BaseViewModel
    {
        /// <summary>
        /// Object of type of "Item"
        /// </summary>
        public Item Item { get; set; }

        /// <summary>
        /// Command for cancel button
        /// </summary>
        public ICommand CancelCommand => new Command(CancelClicked);

        /// <summary>
        /// Command for save button
        /// </summary>
        public ICommand SaveCommand => new Command<EditItemViewModel>(SaveClicked);

        /// <summary>
        /// Command to open URL in browser
        /// </summary>
        public ICommand OpenWebCommand => new Command(OpenWeb);
        /// <summary>
        /// Name of the page
        /// </summary>
        public string Sender { get; }   

        /// <summary>
        /// Initializes the property "Item" and title of the page
        /// </summary>
        /// <param name="item">Object of type of "Item"</param>
        public EditItemViewModel(string sender, Item item)
        {
            Sender = sender;
            Item = item;
        }

        /// <summary>
        /// Navigates to the previous page
        /// </summary>
        async void CancelClicked()
            => await PopAsync();

        /// <summary>
        /// Sends the "EditItem" message
        /// </summary>
        async void SaveClicked(EditItemViewModel vm)
        {
            MessagingCenter.Send(this, Sender == "HistoryViewModel" ? "HistoryEditItem" : "FavoritesEditItem", vm.Item);

            await PopAsync();
        }

        /// <summary>
        /// Opens the URL in browser
        /// </summary>
        async void OpenWeb()
        {
            try
            {
                await Browser.OpenAsync(new UriBuilder(Item.Url).Uri);
            }
            catch (UriFormatException)
            {
                await DisplayAlert("Error", "Invalid URL: Page could not be opened", "OK");
            }
        }
    }
}
