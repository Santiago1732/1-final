using Entidades.Final;
using System.Text;
using System.Xml.Serialization;

using System.Text.Json;
using System.Data.SqlClient;

using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
public static class Manejadora
{
    public static bool EscribirArchivo(List<Usuario> listaUsuarios)
    {
        bool retorno = false;

        try
        {
            // Obtener los apellidos repetidos
            var apellidosRepetidos = listaUsuarios
                .GroupBy(u => u.Apellido)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            // Filtrar usuarios con apellidos repetidos y agrupar los correos por apellido
            var emailsPorApellido = listaUsuarios
                .Where(u => apellidosRepetidos.Contains(u.Apellido))
                .GroupBy(u => u.Apellido)
                .ToList();

            // Ruta del archivo
            string filePath = "C:\\Users\\santi\\OneDrive\\Escritorio\\Logs\\logs.txt";

            // Escribir los correos electrónicos en el archivo sin sobrescribir
            using (StreamWriter writer = new StreamWriter(filePath, append: true))  // 'append: true' asegura que no sobrescriba el archivo
            {
                foreach (var grupo in emailsPorApellido)
                {
                    string fechaHora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    // Escribir el apellido como título
                    writer.WriteLine($"Apellidos repetidos: {grupo.Key} - Fecha: {fechaHora}");

                    // Escribir todos los correos electrónicos del apellido repetido
                    foreach (var usuario in grupo)
                    {
                        writer.WriteLine(usuario.Correo);
                    }

                    // Espacio en blanco entre los grupos de apellidos
                    writer.WriteLine();
                }
            }

            retorno = true;
            Console.WriteLine("Los correos electrónicos han sido guardados en el archivo.");
        }
        catch (Exception ex)
        {
            // Error genérico para cualquier otra excepción
            Console.WriteLine("Error desconocido: " + ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
        return retorno;  // Devuelve true si todo salió bien, o false si ocurrió un error.
    }

    //TIENE OUT
    public static bool DerealizarJson(string path, List<Usuario> users) 
    {
        bool retorno = false;
        //C:\\Users\\santi\\OneDrive\\Escritorio\\Logs

        users = new List<Usuario>();
        try
        {
            // Obtener la ruta del archivo JSON
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, path);

            // Leer el contenido del archivo
            string json = File.ReadAllText(filePath);

            // Deserializar el JSON a una lista de objetos Patente
            users = JsonSerializer.Deserialize<List<Usuario>>(json);

            if (users != null && users.Count > 0)
            {
                retorno = true;
            }
            else
            {
                Console.WriteLine("No se encontraron datos válidos en el archivo JSON.");
            }

            retorno = true;

            return retorno;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al deserializar el archivo: " + ex.Message);
        }
        return retorno;
}

    public static bool SerealizarJson(List<Usuario> users, string path)
    {
        try
        {
            // Obtener la ruta del escritorio
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Combinar la ruta del escritorio con el nombre del archivo
            string fullPath = Path.Combine(desktopPath, "C:\\Users\\santi\\OneDrive\\Escritorio\\Logs\\JSON.json");

            // Serializar los datos a JSON
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });

            // Guardar el JSON en un archivo
            File.WriteAllText(fullPath, json);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
