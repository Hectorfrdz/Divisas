using Divisas.DataAccess;
using Divisas.Models;
using Divisas.ViewModels;

namespace Divisas.Views;

public partial class Divisas : ContentPage
{
    private readonly DemoDbContext _dbContext;

    public Divisas(DemoDbContext dbContext)
    {
        _dbContext = dbContext;
        InitializeComponent();
        BindingContext = new DivisasViewModel(dbContext);
    }

    private async void OnAddCurrencyClicked(object sender, EventArgs e)
    {
        var addCurrencyPage = new AddCurrencyPage();

        // Suscribirse al evento CurrencyAdded
        addCurrencyPage.CurrencyAdded += (newCurrency) =>
        {
            var viewModel = BindingContext as DivisasViewModel;
            viewModel?.AddCurrency(newCurrency);
        };

        await Navigation.PushModalAsync(addCurrencyPage);
    }

    private async void OnEditCurrencyClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var currencyToEdit = button?.CommandParameter as Currency;

        if (currencyToEdit != null)
        {
            var editCurrencyPage = new EditCurrencyPage(currencyToEdit);

            editCurrencyPage.CurrencyEdited += (editedCurrency) =>
            {
                var viewModel = BindingContext as DivisasViewModel;
                viewModel?.EditCurrency(editedCurrency);

                // Recarga la vista completa
                BindingContext = new DivisasViewModel(_dbContext); // Esto recargará el BindingContext
            };

            await Navigation.PushModalAsync(editCurrencyPage);
        }
    }



    private async void OnDeleteCurrencyClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var currency = button?.CommandParameter as Currency;

        if (currency != null)
        {
            bool confirm = await DisplayAlert("Confirmar eliminación", $"¿Estás seguro de que deseas eliminar la divisa {currency.Name}?", "Sí", "No");
            if (confirm)
            {
                try
                {
                    // Usa el ViewModel para eliminar la divisa
                    var viewModel = BindingContext as DivisasViewModel;
                    viewModel?.DeleteCurrency(currency);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "No se pudo eliminar la divisa: " + ex.Message, "OK");
                }
            }
        }
    }

}
