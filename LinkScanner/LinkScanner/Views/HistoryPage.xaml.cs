using LinkScanner.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinkScanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        readonly HistoryViewModel vm;

        /// <summary>
        /// Initializes a 'HistoryViewModel' field 
        /// </summary>
        public HistoryPage()
        {
            InitializeComponent();

            vm =  new HistoryViewModel();
        }

        /// <summary>
        /// Raises when Item in ListView tapped and executes TappedItemCommand in HistoryViewModel.cs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            vm.TappedItemCommand.Execute(e.Item);
        }
    }
}