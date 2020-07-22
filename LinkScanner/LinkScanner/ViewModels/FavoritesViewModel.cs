using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using LinkScanner.Models;
using LinkScanner.Views;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace LinkScanner.ViewModels
{
    public class FavoritesViewModel : BaseViewModel
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
        /// Initializes MessagingCenter.Subscribe to get message as "AddToFavorites" from HistoryViewModel
        /// </summary>
        public FavoritesViewModel()
        {
            // Extracting data from the storage
            if (Application.Current.Properties.ContainsKey("FavoritesItems"))
            {
                Items = JsonConvert.DeserializeObject<ObservableCollection<Item>>(Application.Current.Properties["FavoritesItems"].ToString());
                LabelVisible = false;
                if (Items.Count != 0)
                    LabelVisible = false;
            }

            // Receiving a message from HistoryViewModel with the request to add an item to favorites vm
            MessagingCenter.Subscribe<HistoryViewModel, Item>(this, "AddToFavorites", AddToFavorites);

            // Receiving changed Item from EditItemPage
            MessagingCenter.Subscribe<EditItemViewModel, Item>(this, "FavoritesEditItem", EditItem);

            // Receiving a message from HistoryViewModel that item was deleted
            MessagingCenter.Subscribe<HistoryViewModel, Item>(this, "ItemDeleted", ItemDeleted);
        }

        #region Subscribe Methods

        /// <summary>
        /// Adds an items to favorites 
        /// </summary>
        /// <param name="vm">View Models</param>
        /// <param name="item">Item to add</param>
        void AddToFavorites(HistoryViewModel vm, Item item)
        {
            LabelVisible = false;

            // If there is no item with the same id, then add
            if (Items.All(x => x.Id != item.Id))
                Items.Insert(0, item);

            // Saving in a storage
            Application.Current.Properties["FavoritesItems"] = JsonConvert.SerializeObject(Items);
        }

        /// <summary>
        /// Changes the 'Url' property of an item
        /// </summary>
        /// <param name="vm">View Model</param>
        /// <param name="item">An Item whose property has been changed</param>
        void EditItem(EditItemViewModel vm, Item item)
        {
            int oldItemIndex = Items.IndexOf(Items.FirstOrDefault(arg => arg.Id == item.Id));

            if (oldItemIndex != -1)
                Items[oldItemIndex].Url = item.Url;

            // Saving in a storage
            Application.Current.Properties["FavoritesItems"] = JsonConvert.SerializeObject(Items);
        }

        /// <summary>
        /// If an item don't exists in both (History and Favorites) pages,
        /// then an image file of this items will be deleted from the storage
        /// </summary>
        /// <param name="vm">View Model</param>
        /// <param name="item">An item that has been deleted from page</param>
        void ItemDeleted(HistoryViewModel vm, Item item)
        {
            if(Items.All(x => x.ImagePath != item.ImagePath))
                DeleteImageFile(item.ImagePath);
        }

        #endregion

        /// <summary>
        /// Deletes the item from Favorites vm and if it doesn't exists in History,
        /// then deletes the image from the storage
        /// </summary>
        /// <param name="item">An item that is needed to delete</param>
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
            Application.Current.Properties["FavoritesItems"] = JsonConvert.SerializeObject(Items);
        }

        /// <summary>
        /// Navigates to EditItemPage to show the details of the item and edit them
        /// </summary>
        /// <param name="item">Item whose details will be shown</param>
        async void TappedItem(Item item)
        {
            await PushAsync(new EditItemPage(new EditItemViewModel("FavoritesViewModel", Clone(item))));
        }
    }
}
