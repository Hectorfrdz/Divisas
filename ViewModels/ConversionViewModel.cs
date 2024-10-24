using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Divisas.Models;

namespace Divisas.ViewModels
{
    public class ConversionViewModel : INotifyPropertyChanged
    {
        private decimal _amount;
        private Currency _fromCurrency;
        private Currency _toCurrency;
        private ObservableCollection<ConversionHistoryItem> _conversionesList;

        public event PropertyChangedEventHandler PropertyChanged;

        public ConversionViewModel()
        {
            ConversionesList = new ObservableCollection<ConversionHistoryItem>();
            ConvertCommand = new Command(OnConvert);
        }

        public decimal Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }

        public Currency FromCurrency
        {
            get => _fromCurrency;
            set
            {
                _fromCurrency = value;
                OnPropertyChanged(nameof(FromCurrency));
            }
        }

        public Currency ToCurrency
        {
            get => _toCurrency;
            set
            {
                _toCurrency = value;
                OnPropertyChanged(nameof(ToCurrency));
            }
        }

        public ObservableCollection<ConversionHistoryItem> ConversionesList
        {
            get => _conversionesList;
            set
            {
                _conversionesList = value;
                OnPropertyChanged(nameof(ConversionesList));
            }
        }

        public ICommand ConvertCommand { get; }

        private void OnConvert()
        {
            if (FromCurrency != null && ToCurrency != null && Amount > 0)
            {
                // Aquí se realizaría la lógica de conversión
                decimal conversionRate = GetConversionRate(FromCurrency, ToCurrency);
                decimal convertedValue = Amount * conversionRate;

                // Añadir al historial
                ConversionesList.Add(new ConversionHistoryItem
                {
                    Moneda = ToCurrency.Name,
                    MontoConvertido = convertedValue
                });

                // Reiniciar el monto
                Amount = 0;
            }
        }

        private decimal GetConversionRate(Currency fromCurrency, Currency toCurrency)
        {
            // Lógica para obtener el tipo de cambio entre divisas
            // Por simplicidad, asumimos que es 1.0, deberías reemplazarlo con la lógica real
            return 1.0m; // Placeholder
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ConversionHistoryItem
    {
        public string Moneda { get; set; }
        public decimal MontoConvertido { get; set; }
    }
}
