using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Entidades.Final
{
    public class ADO
    {
        public static event ApellidoUsuarioExistenteDelegado ApellidoUsuarioExistente;

        public delegate void ApellidoUsuarioExistenteDelegado(object sender, EventArgs e);

        private string conexion;
        static ADO(){}

        public void Verificar() { }

        public static bool Agregar(Usuario user)
        {
            string connectionString = "Server=DESKTOP-LNLKOC3\\SQLEXPRESS;Database=laboratorio_2;Trusted_Connection=True;";

            // Consulta para verificar si el apellido ya existe
            string checkQuery = "SELECT COUNT(*) FROM dbo.usuarios WHERE apellido = @apellido";

            string query = "INSERT INTO dbo.usuarios (nombre, apellido, dni, correo, clave) VALUES (@nombre, @apellido, @dni, @correo, @clave)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Verificar si el apellido ya existe
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@apellido", user.Apellido);
                        int count = (int)checkCommand.ExecuteScalar();

                        if (count > 0)
                        {
                            // Disparar el evento si el apellido ya existe
                            ApellidoUsuarioExistente.Invoke(user, null);
                            return false;
                        }
                    }

                    // Insertar el usuario si el apellido no existe
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", user.Nombre);
                        command.Parameters.AddWithValue("@apellido", user.Apellido);
                        command.Parameters.AddWithValue("@dni", user.Dni);
                        command.Parameters.AddWithValue("@correo", user.Correo);
                        command.Parameters.AddWithValue("@clave", user.Clave);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0; // Retorna true si se insertó al menos una fila
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al agregar usuario: {ex.Message}");
                    return false;
                }
            }
        }

        public static void Eliminar(Usuario user)
        {
            string connectionString = "Server=DESKTOP-LNLKOC3\\SQLEXPRESS;Database=laboratorio_2;Trusted_Connection=True;";

            // Comando DELETE para eliminar un usuario específico basado en el dni
            string query = "DELETE FROM Usuarios WHERE dni = @dni";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Agregar el parámetro para el dni
                        command.Parameters.AddWithValue("@dni", user.Dni);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Usuario eliminado exitosamente.");
                        }
                        else
                        {
                            Console.WriteLine("No se encontró un usuario con el DNI especificado.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        public static void ModificarUsuario(Usuario user)
        {
            string connectionString = "Server=DESKTOP-LNLKOC3\\SQLEXPRESS;Database=laboratorio_2;Trusted_Connection=True;";

            // Comando UPDATE para modificar todos los campos, excepto el dni
            string query = "UPDATE Usuarios SET nombre = @nombre, apellido = @apellido, correo = @correo, clave = @clave WHERE dni = @dni";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", user.Nombre);
                        command.Parameters.AddWithValue("@apellido", user.Apellido);
                        command.Parameters.AddWithValue("@correo", user.Correo);
                        command.Parameters.AddWithValue("@clave", user.Clave);
                        command.Parameters.AddWithValue("@dni", user.Dni);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Usuario modificado exitosamente.");
                        }
                        else
                        {
                            Console.WriteLine("No se encontró un usuario con el DNI especificado.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        public static List<Usuario> ObtenerTodos(string apellidoUsuario)
        {
            return null;
        }

        public static List<Usuario> ObtenerTodos()
        {
            List<Usuario> usuarios = new List<Usuario>();

            string connectionString = "Server=DESKTOP-LNLKOC3\\SQLEXPRESS;Database=laboratorio_2;Trusted_Connection=True;";
            string query = "SELECT nombre, apellido, dni, correo, clave FROM dbo.usuarios";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string nombre = reader.GetString(0);
                                string apellido = reader.GetString(1);
                                int dni = reader.GetInt32(2);
                                string correo = reader.GetString(3);
                                string clave = reader.GetString(4);

                                usuarios.Add(new Usuario(nombre, apellido, dni, correo, clave));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener los usuarios: {ex.Message}");
                }
            }

            return usuarios;
        }

    }
}
