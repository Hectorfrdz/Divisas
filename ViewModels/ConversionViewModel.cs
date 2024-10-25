using Divisas.DataAccess;
using Divisas.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace Divisas.ViewModels
{
    public class ConversionViewModel : BindableObject
    {
        private readonly DemoDbContext _dbContext;

        public ObservableCollection<Currency> Currencies { get; set; }
        public ObservableCollection<Transaction> Transactions { get; set; }
        public ObservableCollection<Transaction> TempTransactions { get; set; }

        private string _amountEntry;
        public string AmountEntry
        {
            get => _amountEntry;
            set
            {
                _amountEntry = value;
                OnPropertyChanged();
                ConvertCurrency();
            }
        }

        private Currency _selectedCurrency1;
        public Currency SelectedCurrency1
        {
            get => _selectedCurrency1;
            set
            {
                _selectedCurrency1 = value;
                OnPropertyChanged();
                ConvertCurrency();
            }
        }

        private Currency _selectedCurrency2;
        public Currency SelectedCurrency2
        {
            get => _selectedCurrency2;
            set
            {
                _selectedCurrency2 = value;
                OnPropertyChanged();
                ConvertCurrency();
            }
        }

        private string _convertedAmount;
        public string ConvertedAmount
        {
            get => _convertedAmount;
            set
            {
                _convertedAmount = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveTransactionsCommand { get; set; }

        public ConversionViewModel(DemoDbContext dbContext)
        {
            _dbContext = dbContext;

            Currencies = new ObservableCollection<Currency>();
            Transactions = new ObservableCollection<Transaction>();
            TempTransactions = new ObservableCollection<Transaction>();

            SaveTransactionsCommand = new Command(OnSaveTransactions);

            LoadCurrencies();
            LoadTransactions();
        }

        private void LoadCurrencies()
        {
            var currencyList = _dbContext.Currency.ToList();
            foreach (var currency in currencyList)
            {
                Currencies.Add(currency);
            }
        }

        private void LoadTransactions()
        {
            var transactionList = _dbContext.Transaction
                                            .OrderByDescending(t => t.Date)
                                            .ToList();

            Transactions.Clear();
            foreach (var trans in transactionList)
            {
                Transactions.Add(trans);
            }
        }

        private void ConvertCurrency()
        {
            if (double.TryParse(AmountEntry, out double amount) &&
                SelectedCurrency1 != null &&
                SelectedCurrency2 != null)
            {
                double conversionRate = (double)(SelectedCurrency2.PurchasePrice / SelectedCurrency1.PurchasePrice);
                double convertedAmount = amount * conversionRate;
                ConvertedAmount = $"{convertedAmount:F2} {SelectedCurrency2.Code}";

                TempTransactions.Add(new Transaction
                {
                    FromCurrencyId = SelectedCurrency1.Id,
                    ToCurrencyId = SelectedCurrency2.Id,
                    AmountConverted = (decimal)amount,
                    ConvertedValue = (decimal)convertedAmount,
                    ConversionRate = (decimal)conversionRate,
                    Date = DateTime.UtcNow
                });
            }
        }

        private void OnSaveTransactions()
        {
            foreach (var transaction in TempTransactions)
            {
                _dbContext.Transaction.Add(transaction);
            }
            _dbContext.SaveChanges();

            TempTransactions.Clear();
            LoadTransactions();
        }
    }
}
