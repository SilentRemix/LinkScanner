using LinkScanner.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinkScanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritesPage : ContentPage
    {
        readonly FavoritesViewModel vm;

        /// <summary>
        /// Initializes a 'FavoritesViewModel' field 
        /// </summary>
        public FavoritesPage()
        {
            InitializeComponent();
            vm = new FavoritesViewModel();
        }

        /// <summary>
        /// Raises when Item in ListView tapped and executes TappedItemCommand in FavoritesViewModel.cs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            vm.TappedItemCommand.Execute(e.Item);
        }
    }
}