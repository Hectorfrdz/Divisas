using Divisas.DataAccess;
using Divisas.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace Divisas.ViewModels
{
    public class DivisasViewModel : INotifyPropertyChanged
    {
        private readonly DemoDbContext _dbContext;
        private ObservableCollection<Currency> _currencies;
        private ObservableCollection<Currency> _filteredCurrencies;
        private string _searchText;

        public ObservableCollection<Currency> Currencies
        {
            get => _currencies;
            set
            {
                _currencies = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Currency> FilteredCurrencies
        {
            get => _filteredCurrencies;
            set
            {
                _filteredCurrencies = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterCurrencies();
            }
        }

        public DivisasViewModel(DemoDbContext dbContext)
        {
            _dbContext = dbContext;
            Currencies = new ObservableCollection<Currency>();
            FilteredCurrencies = new ObservableCollection<Currency>();
            LoadCurrencies();
        }

        private void LoadCurrencies()
        {
            var currencyList = _dbContext.Currency.ToList();
            Currencies.Clear();
            foreach (var currency in currencyList)
            {
                Currencies.Add(currency);
            }
            FilteredCurrencies.Clear();
            foreach (var currency in Currencies)
            {
                FilteredCurrencies.Add(currency);
            }
        }

        private void FilterCurrencies()
        {
            var filteredList = Currencies.Where(c =>
                c.Name.ToLower().Contains(SearchText.ToLower()) ||
                c.Code.ToLower().Contains(SearchText.ToLower())).ToList();

            FilteredCurrencies.Clear();
            foreach (var currency in filteredList)
            {
                FilteredCurrencies.Add(currency);
            }
        }

        public void AddCurrency(Currency currency)
        {
            _dbContext.Currency.Add(currency);
            _dbContext.SaveChanges();
            LoadCurrencies();
        }

        public void EditCurrency(Currency currency)
        {
            var existingCurrency = _dbContext.Currency.Find(currency.Id);
            if (existingCurrency != null)
            {
                existingCurrency.Name = currency.Name;
                existingCurrency.Code = currency.Code;
                existingCurrency.PurchasePrice = currency.PurchasePrice;
                existingCurrency.SalePrice = currency.SalePrice;
                _dbContext.SaveChanges();

                LoadCurrencies();
            }
        }


        public void DeleteCurrency(Currency currency)
        {
            var existingCurrency = _dbContext.Currency.Find(currency.Id);
            if (existingCurrency != null)
            {
                _dbContext.Currency.Remove(existingCurrency);
                _dbContext.SaveChanges();
                LoadCurrencies();
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
