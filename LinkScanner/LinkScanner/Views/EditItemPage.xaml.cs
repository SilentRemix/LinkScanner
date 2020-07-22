using LinkScanner.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LinkScanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditItemPage : ContentPage
    {
        /// <summary>
        /// Initializes a 'EditItemViewModel' field and BindingContext
        /// </summary>
        public EditItemPage(EditItemViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
}