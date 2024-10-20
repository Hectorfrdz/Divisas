using System;

namespace Divisas.Utilities;

public class ConnectionDB
{
    public string NameDB { get; set; }

    public ConnectionDB(string databaseName)
    {
        NameDB = databaseName;
    }

    public string GetRouteFromDatabase()
    {
        string databaseRoute = string.Empty;

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            databaseRoute = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            databaseRoute = Path.Combine(databaseRoute, NameDB);
        }

        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            databaseRoute = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            databaseRoute = Path.Combine(databaseRoute, "..", "Library", NameDB);
        }

        return databaseRoute;
    }

    public void DeleteDatabase()
    {
        string databaseRoute = GetRouteFromDatabase();

        if (!File.Exists(databaseRoute))
        {
            Console.WriteLine("Database not found");
            return;
        }

        File.Delete(databaseRoute);
        Console.WriteLine("Database deleted");
    }
}
