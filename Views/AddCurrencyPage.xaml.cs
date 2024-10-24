using Divisas.Models;
using System;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Divisas.Views
{
    public partial class AddCurrencyPage : ContentPage
    {
        public event Action<Currency> CurrencyAdded;

        public AddCurrencyPage()
        {
            InitializeComponent();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var newCurrency = new Currency
            {
                Name = entryName.Text,
                Code = entryCode.Text,
                PurchasePrice = decimal.Parse(entryPurchasePrice.Text),
                SalePrice = decimal.Parse(entrySalePrice.Text)
            };

            CurrencyAdded?.Invoke(newCurrency);
            await Navigation.PopModalAsync();
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
