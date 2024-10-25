using Divisas.DataAccess;
using Divisas.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace Divisas.Views
{
    public partial class Conversiones : ContentPage
    {
        private readonly DemoDbContext _dbContext;
        public ObservableCollection<Currency> Currencies { get; set; }
        public ObservableCollection<Transaction> Transactions { get; set; }
        public ObservableCollection<Transaction> TempTransactions { get; set; }

        public Conversiones(DemoDbContext dbContext)
        {
            _dbContext = dbContext;
            InitializeComponent();

            Currencies = new ObservableCollection<Currency>();
            Transactions = new ObservableCollection<Transaction>();
            TempTransactions = new ObservableCollection<Transaction>();

            LoadTransactions();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            LoadCurrencies();
        }

        private void LoadTransactions()
        {
            var transactionList = _dbContext.Transaction
                                            .OrderByDescending(t => t.Date) 
                                            .ToList();

            Transactions.Clear();

            if (transactionList.Any())
            {
                foreach (var trans in transactionList)
                {
                    Transactions.Add(trans);
                }
            }

            lvTransaction.ItemsSource = Transactions;
        }

        private void LoadCurrencies()
        {
            Currencies.Clear();
            var currencyList = _dbContext.Currency.ToList();

            if (currencyList.Any())
            {
                foreach (var currency in currencyList)
                {
                    Currencies.Add(currency);
                }
            }

            currencyPicker1.ItemsSource = Currencies.Select(c => c.Name).ToList();
            currencyPicker2.ItemsSource = Currencies.Select(c => c.Name).ToList();
        }

        private void OnPicker1SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currencyPicker1.SelectedIndex != -1)
            {
                amountEntry.Text = "1";
                ConvertCurrency();
            }
        }

        private void OnPicker2SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currencyPicker2.SelectedIndex != -1)
            {
                var selectedCurrency = Currencies[currencyPicker2.SelectedIndex];
                priceLabel2.Text = $"Precio: {selectedCurrency.PurchasePrice:C}";
                ConvertCurrency();
            }
        }

        private void OnAmountEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            ConvertCurrency();
        }

        private void ConvertCurrency()
        {
            if (double.TryParse(amountEntry.Text, out double amount) &&
                currencyPicker1.SelectedIndex != -1 &&
                currencyPicker2.SelectedIndex != -1)
            {
                var currency1 = Currencies[currencyPicker1.SelectedIndex];
                var currency2 = Currencies[currencyPicker2.SelectedIndex];

                double conversionRate = (double)(currency2.PurchasePrice / currency1.PurchasePrice);
                double convertedAmount = amount * conversionRate;

                priceLabel2.Text = $"{convertedAmount:F2} {currency2.Code}";

                TempTransactions.Add(new Transaction
                {
                    FromCurrencyId = currency1.Id,
                    ToCurrencyId = currency2.Id,
                    AmountConverted = (decimal)amount,
                    ConvertedValue = (decimal)convertedAmount,
                    ConversionRate = (decimal)conversionRate,
                    Date = DateTime.UtcNow
                });
            }
        }

        private void OnSaveTransactionsClicked(object sender, EventArgs e)
        {
            foreach (var transaction in TempTransactions)
            {
                _dbContext.Transaction.Add(transaction);
            }
            _dbContext.SaveChanges();
            Transactions.Clear();
            LoadTransactions();
            TempTransactions.Clear();
        }
    }
}
