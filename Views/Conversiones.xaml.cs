using Divisas.DataAccess;
using Divisas.ViewModels;

namespace Divisas.Views
{
    public partial class Conversiones : ContentPage
    {
        private ConversionViewModel _viewModel;

        public Conversiones(DemoDbContext dbContext)
        {
            InitializeComponent();
            _viewModel = new ConversionViewModel(dbContext);
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadCurrencies(); // Recarga las divisas cada vez que la página aparece
        }
    }

}
