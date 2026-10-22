using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using ZstdSharp.Unsafe;

namespace KonyvtarWPF
{
    internal class Database
    {
        private readonly string connectionString =
            "Server=localhost;" +
            "Database=wpf_crud;" +
            "Port=3306;" +
            "User ID=root;" +
            "Password=;";
        public bool TestConnection()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database connection failed: {ex.Message}");
                return false;
            }
        }

        //Read
        public List<Book> GetBooks()
        {
            List<Book> books = new List<Book>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM books";
                MySqlCommand command = new MySqlCommand(sql, connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    string title = reader.GetString("title");
                    string author = reader.GetString("author");
                    int publishedYear = reader.GetInt32("publish_year");
                    decimal price = reader.GetDecimal("price");
                    int stock = reader.GetInt32("stock");
                    Book book = new Book(id, title, author, publishedYear, price, stock);
                    books.Add(book);
                }
            }
            return books;
        }

        public void InsertBook(Book book)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "INSERT INTO books (title, author, publish_year, price, stock) VALUES (@title, @author, @publish_year, @price, @stock)";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@title", book.Title);
            command.Parameters.AddWithValue("@author", book.Author);
            command.Parameters.AddWithValue("@publish_year", book.PublishedYear);
            command.Parameters.AddWithValue("@price", book.Price);
            command.Parameters.AddWithValue("@stock", book.Stock);
            command.ExecuteNonQuery();
        }

        public void UpdateBook(Book book)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "UPDATE books SET title=@title, author=@author, publish_year=@publish_year, price=@price, stock=@stock WHERE id=@id";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@title", book.Title);
            command.Parameters.AddWithValue("@author", book.Author);
            command.Parameters.AddWithValue("@publish_year", book.PublishedYear);
            command.Parameters.AddWithValue("@price", book.Price);
            command.Parameters.AddWithValue("@stock", book.Stock);
            command.Parameters.AddWithValue("@id", book.Id);
            command.ExecuteNonQuery();

        }

        public void DeleteBook(int id)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            string sql = "DELETE FROM books WHERE id=@id";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }


        public List<Book> SearchBooks(string search)
        {
            List<Book> books = new List<Book>();
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string sql = "SELECT * FROM books where title LIKE @search OR author LIKE @search";
            MySqlCommand command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@search", "%" + search + "%");
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Book book = new Book(
                    reader.GetInt32("id"),
                    reader.GetString("title"),
                    reader.GetString("author"),
                    reader.GetInt32("publish_year"),
                    reader.GetDecimal("price"),
                    reader.GetInt32("stock"));
                books.Add(book);        
            }
            return books;

        }
    }
}
