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
        public ObservableCollection<Transaction> TempTransactions { get; set; } // Colección temporal

        public Conversiones(DemoDbContext dbContext)
        {
            _dbContext = dbContext;
            InitializeComponent();

            Currencies = new ObservableCollection<Currency>();
            Transactions = new ObservableCollection<Transaction>();
            TempTransactions = new ObservableCollection<Transaction>(); // Inicializa la colección temporal

            LoadCurrencies();
            LoadTransactions();
        }

        private void LoadTransactions()
        {
            var transactionList = _dbContext.Transaction
                                            .OrderByDescending(t => t.Date) // Ordenar de más reciente a más antiguo
                                            .ToList();

            Transactions.Clear(); // Limpiar la colección antes de llenarla

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
                amountEntry.Text = "1"; // Establecer la cantidad por defecto a 1
                ConvertCurrency(); // Realiza la conversión inmediatamente
            }
        }

        private void OnPicker2SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currencyPicker2.SelectedIndex != -1)
            {
                var selectedCurrency = Currencies[currencyPicker2.SelectedIndex];
                priceLabel2.Text = $"Precio: {selectedCurrency.PurchasePrice:C}"; // Mostrar el precio
                ConvertCurrency(); // Realiza la conversión inmediatamente
            }
        }

        private void OnAmountEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            ConvertCurrency(); // Llama a la conversión cuando se cambie el texto
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

                priceLabel2.Text = $"{convertedAmount:F2} {currency2.Code}"; // Muestra la cantidad convertida

                // Almacenar la transacción en la colección temporal
                TempTransactions.Add(new Transaction
                {
                    FromCurrencyId = currency1.Id,
                    ToCurrencyId = currency2.Id,
                    AmountConverted = (decimal)amount,
                    ConvertedValue = (decimal)convertedAmount,
                    ConversionRate = (decimal)conversionRate,
                    Date = DateTime.UtcNow // O la fecha que desees
                });
            }
        }

        private void OnSaveTransactionsClicked(object sender, EventArgs e)
        {
            foreach (var transaction in TempTransactions)
            {
                _dbContext.Transaction.Add(transaction); // Añadir la nueva transacción
            }
            _dbContext.SaveChanges(); // Guardar los cambios en la base de datos
            Transactions.Clear(); // Limpiar la lista de transacciones existentes
            LoadTransactions(); // Volver a cargar las transacciones desde la base de datos
            TempTransactions.Clear(); // Limpiar la colección temporal
        }
    }
}
