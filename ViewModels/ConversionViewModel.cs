using Divisas.DataAccess;
using Divisas.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Divisas.ViewModels
{
    public class ConversionViewModel : BindableObject
    {
        private readonly DemoDbContext _dbContext;
        private double _amount;
        private string _convertedAmount;
        private Currency _selectedCurrency1;
        private Currency _selectedCurrency2;

        public ObservableCollection<Currency> Currencies { get; set; }
        public ObservableCollection<Transaction> Transactions { get; set; }
        public ObservableCollection<Transaction> TempTransactions { get; set; }

        public double Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged();
                ConvertCurrency();
            }
        }

        public string ConvertedAmount
        {
            get => _convertedAmount;
            set
            {
                _convertedAmount = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveTransactionsCommand { get; }

        public ConversionViewModel(DemoDbContext dbContext)
        {
            _dbContext = dbContext;
            Currencies = new ObservableCollection<Currency>();
            Transactions = new ObservableCollection<Transaction>();
            TempTransactions = new ObservableCollection<Transaction>();

            SaveTransactionsCommand = new Command(SaveTransactions);
            LoadCurrencies();
            LoadTransactions();
        }

        private void LoadTransactions()
        {
            var transactionList = _dbContext.Transaction
                                            .OrderByDescending(t => t.Date)
                                            .Take(10)
                                            .ToList();
            Transactions.Clear();
            foreach (var trans in transactionList)
            {
                Transactions.Add(trans);
            }
        }


        public void LoadCurrencies()
        {
            var currencyList = _dbContext.Currency.ToList();
            Currencies.Clear();
            foreach (var currency in currencyList)
            {
                Currencies.Add(currency);
            }
            Amount = 0;
            ConvertedAmount = string.Empty;
        }

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

        private void ConvertCurrency()
        {
            if (Amount > 0 && SelectedCurrency1 != null && SelectedCurrency2 != null)
            {
                double conversionRate = (double)(SelectedCurrency2.PurchasePrice / SelectedCurrency1.PurchasePrice);
                double convertedAmount = Amount * conversionRate;

                ConvertedAmount = $"{convertedAmount:F2} {SelectedCurrency2.Code}";

                TempTransactions.Clear();

                TempTransactions.Add(new Transaction
                {
                    FromCurrencyId = SelectedCurrency1.Id,
                    ToCurrencyId = SelectedCurrency2.Id,
                    AmountConverted = (decimal)Amount,
                    ConvertedValue = (decimal)convertedAmount,
                    ConversionRate = (decimal)conversionRate,
                    Date = DateTime.UtcNow
                });
            }
        }



        private void SaveTransactions()
        {
            foreach (var transaction in TempTransactions)
            {
                _dbContext.Transaction.Add(transaction);
            }
            _dbContext.SaveChanges();
            LoadTransactions();
            TempTransactions.Clear();
        }
    }
}
