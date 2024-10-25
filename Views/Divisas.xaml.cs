using Divisas.DataAccess;
using System.Collections.ObjectModel;
using Divisas.Models;

namespace Divisas.Views;

public partial class Divisas : ContentPage
{
    private readonly DemoDbContext _dbContext;
    public ObservableCollection<Currency> Currencies { get; set; }
    public ObservableCollection<Currency> FilteredCurrencies { get; set; }

    public Divisas(DemoDbContext dbContext)
    {
        _dbContext = dbContext;
        InitializeComponent();

        Currencies = new ObservableCollection<Currency>();
        FilteredCurrencies = new ObservableCollection<Currency>();

        LoadCurrencies();

        BindingContext = this;
    }

    private void LoadCurrencies()
    {
        var currencyList = _dbContext.Currency.ToList();
        if (currencyList.Any())
        {
            foreach (var currency in currencyList)
            {
                Currencies.Add(currency);
                FilteredCurrencies.Add(currency);
            }
        }

        lvCurrency.ItemsSource = FilteredCurrencies;
    }

    private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = e.NewTextValue.ToLower();

        var filteredList = Currencies.Where(c =>
            c.Name.ToLower().Contains(searchText) ||
            c.Code.ToLower().Contains(searchText)).ToList();

        FilteredCurrencies.Clear();
        foreach (var currency in filteredList)
        {
            FilteredCurrencies.Add(currency);
        }
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
            Currencies.Clear();
            FilteredCurrencies.Clear();
            LoadCurrencies();
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
                currencyInDb.Name = updatedCurrency.Name;
                currencyInDb.Code = updatedCurrency.Code;
                currencyInDb.PurchasePrice = updatedCurrency.PurchasePrice;
                currencyInDb.SalePrice = updatedCurrency.SalePrice;

                _dbContext.SaveChanges();

                // Actualiza la colección local
                var index = Currencies.IndexOf(Currencies.FirstOrDefault(c => c.Id == updatedCurrency.Id));
                if (index != -1)
                {
                    Currencies[index] = new Currency
                    {
                        Id = updatedCurrency.Id,
                        Name = updatedCurrency.Name,
                        Code = updatedCurrency.Code,
                        PurchasePrice = updatedCurrency.PurchasePrice,
                        SalePrice = updatedCurrency.SalePrice
                    };
                }

                // Actualiza FilteredCurrencies
                var filteredIndex = FilteredCurrencies.IndexOf(FilteredCurrencies.FirstOrDefault(c => c.Id == updatedCurrency.Id));
                if (filteredIndex != -1)
                {
                    FilteredCurrencies[filteredIndex] = new Currency
                    {
                        Id = updatedCurrency.Id,
                        Name = updatedCurrency.Name,
                        Code = updatedCurrency.Code,
                        PurchasePrice = updatedCurrency.PurchasePrice,
                        SalePrice = updatedCurrency.SalePrice
                    };
                }

                // Forzar actualización de la UI
                lvCurrency.ItemsSource = null;
                lvCurrency.ItemsSource = FilteredCurrencies; // Reasigna la fuente para que la UI se actualice
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", "No se pudo editar la divisa: " + ex.Message, "OK");
        }
    }



    private void UpdateFilteredCurrencies()
    {
        Currencies.Clear();
        FilteredCurrencies.Clear();
        LoadCurrencies();
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
                    _dbContext.Currency.Remove(currency);
                    _dbContext.SaveChanges();

                    Currencies.Clear();
                    FilteredCurrencies.Clear();
                    LoadCurrencies();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "No se pudo eliminar la divisa: " + ex.Message, "OK");
                }
            }
        }
    }


}
