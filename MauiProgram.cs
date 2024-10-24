using Microsoft.Extensions.Logging;
using Divisas.DataAccess;
using Divisas.Views;

namespace Divisas
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddDbContext<DemoDbContext>();
            builder.Services.AddTransient<Inicio>();
            builder.Services.AddTransient<Ajustes>();
            builder.Services.AddTransient<Divisas.Views.Divisas>();
            builder.Services.AddTransient<Conversiones>();

            var dbContext = new DemoDbContext();
            dbContext.Database.EnsureCreated();
            dbContext.Dispose();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
