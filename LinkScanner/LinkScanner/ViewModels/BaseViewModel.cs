using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using LinkScanner.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LinkScanner.ViewModels
{
    /// <summary>
    /// Class which represents Base ViewModel for all ViewModels
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        private bool isBusy;
        /// <summary>
        /// Indicates activity indicator
        /// </summary>
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        /// <summary>
        /// Command to share item
        /// </summary>
        public ICommand ShareCommand => new Command<Item>(ShareMethod);

        /// <summary>
        /// Shares an item via social networks
        /// </summary>
        /// <param name="item"></param>
        async void ShareMethod(Item item)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Subject = "Scanned URL",
                Uri = item.Url
            });
        }

        /// <summary>
        /// Deletes the image file from the storage
        /// </summary>
        /// <param name="path">Path to the file</param>
        protected async void DeleteImageFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (FileNotFoundException)
            {
                await DisplayAlert("Error", "Failed to delete the file", "OK");
            }
            catch (UnauthorizedAccessException)
            {
                await DisplayAlert("Error", "Failed to delete the file", "OK");
            }
            catch (IOException)
            {
                await DisplayAlert("Error", "Failed to delete the file", "OK");
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Failed to delete the file", "OK");
            }
        }

        /// <summary>
        /// Creates the clone of the object
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="instance">Object that needed to be cloned</param>
        /// <returns>Clone of the object</returns>
        protected T Clone<T>(T instance)
        {
            var json = JsonConvert.SerializeObject(instance);
            return JsonConvert.DeserializeObject<T>(json);
        }

        #region Page Services

        /// <summary>
        /// Presents an alert dialog to the application user with a single cancel button.
        /// </summary>
        /// <param name="title">The title of the alert dialog.</param>
        /// <param name="message">The body text of the alert dialog.</param>
        /// <param name="cancel">Text to be displayed on the 'Cancel' button.</param>
        /// <returns>A task that represents the asynchronous push operation.</returns>
        public async Task DisplayAlert(string title, string message, string cancel)
            => await Application.Current.MainPage.DisplayAlert(title, message, cancel);

        /// <summary>
        /// Asynchronously adds a Page to the top of the navigation stack.
        /// </summary>
        /// <param name="page">The Page to be pushed on top of the navigation stack.</param>
        /// <returns>A task that represents the asynchronous push operation.</returns>
        public async Task PushAsync(Page page)
            => await Application.Current.MainPage.Navigation.PushAsync(page);

        /// <summary>
        /// Asynchronously removes the top Page from the navigation stack.
        /// </summary>
        /// <returns>The Page that had been at the top of the navigation stack.</returns>
        public async Task<Page> PopAsync()
            => await Application.Current.MainPage.Navigation.PopAsync();

        #endregion

        /// <summary>
        /// Changes property
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="backingStore">Reference to the field</param>
        /// <param name="value">Value</param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="onChanged"></param>
        /// <returns>true - if property is changed
        /// false - if property isn't changed</returns>
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName]string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Fires when property is changed
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }
}
