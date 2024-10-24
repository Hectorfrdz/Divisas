using Divisas.Models;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Divisas.Views
{
    public partial class EditCurrencyPage : ContentPage
    {
        private Currency _currency;

        public event Action<Currency> CurrencyEdited;

        public EditCurrencyPage(Currency currency)
        {
            InitializeComponent();
            _currency = currency;
            LoadCurrencyData();
        }

        private void LoadCurrencyData()
        {
            entryName.Text = _currency.Name;
            entryCode.Text = _currency.Code;
            entryPurchasePrice.Text = _currency.PurchasePrice.ToString();
            entrySalePrice.Text = _currency.SalePrice.ToString();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            _currency.Name = entryName.Text;
            _currency.Code = entryCode.Text;
            _currency.PurchasePrice = decimal.Parse(entryPurchasePrice.Text);
            _currency.SalePrice = decimal.Parse(entrySalePrice.Text);

            CurrencyEdited?.Invoke(_currency);
            await Navigation.PopModalAsync();
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
