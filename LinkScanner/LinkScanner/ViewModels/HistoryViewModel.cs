using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using LinkScanner.Models;
using LinkScanner.Views;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace LinkScanner.ViewModels
{
    /// <summary>
    /// ViewModel of the HistoryPage
    /// </summary>
    public class HistoryViewModel : BaseViewModel
    {
        /// <summary>
        /// Observable Collection of objects of type 'Item'
        /// </summary>
        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();

        private bool labelVisible = true;
        /// <summary>
        /// Indicates whether the label is visible or not
        /// </summary>
        public bool LabelVisible
        {
            get => labelVisible;
            private set => SetProperty(ref labelVisible, value);
        }

        /// <summary>
        /// Command for tapped item
        /// </summary>
        public ICommand TappedItemCommand => new Command<Item>(TappedItem);

        /// <summary>
        /// Command to delete item
        /// </summary>
        public ICommand DeleteItemCommand => new Command<Item>(DeleteItem);
        /// <summary>
        /// Command to add an item to favorites page
        /// </summary>
        public ICommand AddFavoritesCommand => new Command<Item>(AddToFavorites);

        /// <summary>
        /// Initializes MessagingCenter.Subscribe 
        /// </summary>
        public HistoryViewModel()
        {
            // Extracting data from the storage
            if (Application.Current.Properties.ContainsKey("HistoryItems"))
            {
                Items = JsonConvert.DeserializeObject<ObservableCollection<Item>>(Application.Current.Properties["HistoryItems"].ToString());
                if (Items.Count != 0)
                    LabelVisible = false;
            }

            // Receiving an Item from ScanViewModel
            MessagingCenter.Subscribe<ScanViewModel, Item>(this, "AddToHistory", AddToHistory);

            // Receiving changed Item from EditItemPage
            MessagingCenter.Subscribe<EditItemViewModel, Item>(this, "HistoryEditItem", EditItem);

            // Receiving Item and message from FavoritesViewModel
            MessagingCenter.Subscribe<FavoritesViewModel, Item>(this, "ItemDeleted", ItemDeleted);
        }

        #region Subscribe Methods

        /// <summary>
        /// Adds an item to history
        /// </summary>
        /// <param name="vm">View Model</param>
        /// <param name="item">Item to add</param>
        void AddToHistory(ScanViewModel vm, Item item)
        {
            LabelVisible = false;

            Items.Insert(0, item);

            // Saving in a storage
            Application.Current.Properties["HistoryItems"] = JsonConvert.SerializeObject(Items);
        }

        /// <summary>
        /// Changes the 'Url' property of an item
        /// </summary>
        /// <param name="vm">View Model</param>
        /// <param name="item">An item that has been edited</param>
        void EditItem(EditItemViewModel vm, Item item)
        {
            int oldItemIndex = Items.IndexOf(Items.FirstOrDefault(arg => arg.Id == item.Id));

            if (oldItemIndex != -1)
                Items[oldItemIndex].Url = item.Url;

            // Saving in a storage
            Application.Current.Properties["HistoryItems"] = JsonConvert.SerializeObject(Items);
        }

        /// <summary>
        /// If an item don't exists in both (History and Favorites) pages,
        /// then an image file of this items will be deleted from the storage
        /// </summary>
        /// <param name="vm">View Model</param>
        /// <param name="item">An item that has been deleted from page</param>
        void ItemDeleted(FavoritesViewModel vm, Item item)
        {
            if(Items.All(x => x.ImagePath != item.ImagePath))
                DeleteImageFile(item.ImagePath);
        }

        #endregion

        #region Context Action

        /// <summary>
        /// Button on MenuItem. Fires when it's clicked, and deletes selected item
        /// </summary>
        /// <param name="item">Selected item</param>
        void DeleteItem(Item item)
        {
            Items.Remove(item);

            if (Items.All(x => x.ImagePath != item.ImagePath))
            {
                // Sending the message to HistoryViewModel
                MessagingCenter.Send(this, "ItemDeleted", item);
            }

            if (Items.Count == 0)
                LabelVisible = true;

            // Saving in a storage
            Application.Current.Properties["HistoryItems"] = JsonConvert.SerializeObject(Items);
        }

        /// <summary>
        /// Button on MenuItem. Fires when it's clicked, and adds the item to favorites page
        /// </summary>
        /// <param name="item">Selected item</param>
        void AddToFavorites(Item item)
        {
            MessagingCenter.Send(this, "AddToFavorites", Clone(item));
        }

        #endregion

        /// <summary>
        /// Navigates to EditItemPage to show the details of the item and edit them
        /// </summary>
        /// <param name="item">Item whose details will be shown</param>
        async void TappedItem(Item item)
        {
            // Navigating to EditItemPage
            await PushAsync(new EditItemPage(new EditItemViewModel("HistoryViewModel", Clone(item))));
        }
    }
}
