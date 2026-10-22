using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace KonyvtarWPF
{
    public class Book
    {
        private int id;
        private string title;
        private string author;
        private int publishedYear;
        private decimal price;
        private int stock;

        public Book(int id, string title, string author, int publishedYear, decimal price, int stock)
        {
            this.Id = id;
            this.Title = title;
            this.Author = author;
            this.PublishedYear = publishedYear;
            this.Price = price;
            this.Stock = stock;
        }

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Author { get => author; set => author = value; }
        public int PublishedYear { get => publishedYear; set => publishedYear = value; }
        public decimal Price { get => price; set => price = value; }
        public int Stock { get => stock; set => stock = value; }
    }



}
