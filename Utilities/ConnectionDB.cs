using System;

namespace Divisas.Utilities;

public static class ConnectionDB
{
    //public string NameDB { get; set; }

    //public ConnectionDB(string databaseName)
    //{
    //    NameDB = databaseName;
    //}

    public static string GetRouteFromDatabase(string nameDatabaseRoute)
    {
        string databaseRoute = string.Empty;

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            databaseRoute = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            databaseRoute = Path.Combine(databaseRoute, nameDatabaseRoute);
        } 
        else if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            databaseRoute = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            databaseRoute = Path.Combine(databaseRoute, "..", "Library", nameDatabaseRoute);
        };

        return databaseRoute;
    }


    //public static void DeleteDatabase(string nameDatabaseRoute)
    //{
    //    string databaseRoute = GetRouteFromDatabase(nameDatabaseRoute);

    //    if (!File.Exists(databaseRoute))
    //    {
    //        Console.WriteLine("Database not found");
    //        return;
    //    }

    //    File.Delete(databaseRoute);
    //    Console.WriteLine("Database deleted");
    //}
}
