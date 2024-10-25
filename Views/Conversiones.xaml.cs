using Divisas.DataAccess;
using Divisas.ViewModels;

namespace Divisas.Views
{
    public partial class Conversiones : ContentPage
    {
        public Conversiones(DemoDbContext dbContext)
        {
            InitializeComponent();
            BindingContext = new ConversionViewModel(dbContext);
        }
    }


}
