using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace p1final2024
{
    public class ArticuloDAO
    {
        string connectionString = "server=sql5.freesqldatabase.com;user=sql5712512;database=sql5712512;port=3306;password=rUpYP1VwYa";


        public ArticuloDAO()
        {

        }

        public int Create(Articulo articulo)
        {
            int nuevoId = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO articulos (nombre, descripcion, precio, imagen) VALUES (@nombre, @descripcion, @precio, @imagen); SELECT LAST_INSERT_ID();";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", articulo.Nombre);
                        cmd.Parameters.AddWithValue("@descripcion", articulo.Descripcion);
                        cmd.Parameters.AddWithValue("@precio", articulo.Precio);
                        cmd.Parameters.AddWithValue("@imagen", articulo.Imagen);
                        nuevoId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error de MySQL: {ex.Message}");
                    throw; 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    throw; 
                }
            }
            return nuevoId;
        }

        public Articulo Read(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM articulos WHERE id = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Articulo
                            {
                                Id = reader.GetInt32("id"),
                                Nombre = reader.GetString("nombre"),
                                Descripcion = reader.GetString("descripcion"),
                                Precio = reader.GetDecimal("precio"),
                                Imagen = reader["image"] as byte[]
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<Articulo> ReadAll()
        {
            List<Articulo> articulos = new List<Articulo>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM articulos ORDER BY id DESC";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                articulos.Add(new Articulo
                                {
                                    Id = reader.GetInt32("id"),
                                    Nombre = reader.GetString("nombre"),
                                    Descripcion = reader.GetString("descripcion"),
                                    Precio = reader.GetDecimal("precio"),
                                    Imagen = reader["imagen"] as byte[]
                                });
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error de MySQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return articulos;
        }


        public void Update(Articulo articulo)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE articulos SET nombre = @nombre, descripcion = @descripcion, precio = @precio, imagen = @imagen WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", articulo.Nombre);
                        cmd.Parameters.AddWithValue("@descripcion", articulo.Descripcion);
                        cmd.Parameters.AddWithValue("@precio", articulo.Precio);
                        cmd.Parameters.AddWithValue("@imagen", articulo.Imagen);
                        cmd.Parameters.AddWithValue("@id", articulo.Id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error de MySQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void Delete(int id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM articulos WHERE id = @id"; 
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error de MySQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
