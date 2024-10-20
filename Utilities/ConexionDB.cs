using System;

namespace Divisas.Utilities;

public class ConexionDB
{
    public string NombreDB { get; set; }

    public ConexionDB(string nombreBaseDatos)
    {
        NombreDB = nombreBaseDatos;
    }

    public string DevolverRuta()
    {
        string rutaBaseDatos = string.Empty;

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            rutaBaseDatos = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            rutaBaseDatos = Path.Combine(rutaBaseDatos, NombreDB);
        }

        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            rutaBaseDatos = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            rutaBaseDatos = Path.Combine(rutaBaseDatos, "..", "Library", NombreDB);
        }

        return rutaBaseDatos;
    }

    public void BorrarBaseDatos()
    {
        string rutaBaseDatos = DevolverRuta();

        if (!File.Exists(rutaBaseDatos))
        {
            Console.WriteLine("No se encontró la base de datos");
            return;
        }

        File.Delete(rutaBaseDatos);
        Console.WriteLine("Base de datos eliminada");
    }
}
