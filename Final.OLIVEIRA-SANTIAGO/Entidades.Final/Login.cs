using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Entidades.Final
{
    public class Login
    {
        protected string email;
        protected string pass;

        public string Email
        {
            get
            {
                return this.email;
            }
        }

        public string Pass
        {
            get
            {
                return this.pass;
            }
        }

        public Login(string correo, string clave)
        {
            this.email = correo;
            this.pass = clave;
        }


        public bool Loguear()
        {
        string connectionString = "Server=DESKTOP-LNLKOC3\\SQLEXPRESS;Database=laboratorio_2;Trusted_Connection=True;";
        string query = "SELECT COUNT(*) FROM dbo.usuarios WHERE correo = @correo AND clave = @clave";
            // Crear una conexión SQL
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Agregar parámetros a la consulta
                        command.Parameters.AddWithValue("@correo", Email);
                        command.Parameters.AddWithValue("@clave", Pass);

                        // Ejecutar la consulta y verificar si el usuario existe
                        int count = (int)command.ExecuteScalar();

                        return count > 0; // Retorna true si existe, false si no
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al conectar con la base de datos: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
