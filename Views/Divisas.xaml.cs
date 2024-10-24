using Divisas.DataAccess;
using System.Collections.ObjectModel;
using Divisas.Models;

namespace Divisas.Views;

public partial class Divisas : ContentPage
{
    private readonly DemoDbContext _dbContext;
    public ObservableCollection<Currency> Currencies { get; set; }

    public Divisas(DemoDbContext dbContext)
    {
        _dbContext = dbContext;
        InitializeComponent();

        Currencies = new ObservableCollection<Currency>();

        LoadCurrencies();
    }

    private void LoadCurrencies()
    {
        var currencyList = _dbContext.Currency.ToList();
        if (currencyList.Any())
        {
            foreach (var currency in currencyList)
            {
                Currencies.Add(currency);
            }
        }

        lvCurrency.ItemsSource = Currencies;
    }

    private async void OnAddCurrencyClicked(object sender, EventArgs e)
    {
        var addCurrencyPage = new AddCurrencyPage();
        addCurrencyPage.CurrencyAdded += OnCurrencyAdded;
        await Navigation.PushModalAsync(addCurrencyPage);
    }

    private void OnCurrencyAdded(Currency newCurrency)
    {
        try
        {
            _dbContext.Currency.Add(newCurrency);
            _dbContext.SaveChanges();
            Currencies.Add(newCurrency);
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", "No se pudo agregar la divisa: " + ex.Message, "OK");
        }
    }

    private async void OnEditCurrencyClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var currencyToEdit = button?.CommandParameter as Currency;

        if (currencyToEdit != null)
        {
            var editCurrencyPage = new EditCurrencyPage(currencyToEdit);
            editCurrencyPage.CurrencyEdited += OnCurrencyEdited;
            await Navigation.PushModalAsync(editCurrencyPage);
        }
    }

    private void OnCurrencyEdited(Currency updatedCurrency)
    {
        try
        {
            var currencyInDb = _dbContext.Currency.FirstOrDefault(c => c.Id == updatedCurrency.Id);
            if (currencyInDb != null)
            {
                // Actualizar los campos
                currencyInDb.Name = updatedCurrency.Name;
                currencyInDb.Code = updatedCurrency.Code;
                currencyInDb.PurchasePrice = updatedCurrency.PurchasePrice;
                currencyInDb.SalePrice = updatedCurrency.SalePrice;

                // Guardar cambios
                _dbContext.SaveChanges();

                // Actualizar la lista observable para reflejar los cambios en la UI
                var index = Currencies.IndexOf(currencyInDb);
                Currencies[index] = currencyInDb;
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", "No se pudo editar la divisa: " + ex.Message, "OK");
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
                    // Eliminar la divisa de la base de datos
                    _dbContext.Currency.Remove(currency);
                    _dbContext.SaveChanges();

                    // Eliminar la divisa de la lista observable para actualizar la UI
                    Currencies.Remove(currency);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "No se pudo eliminar la divisa: " + ex.Message, "OK");
                }
            }
        }
    }


}
