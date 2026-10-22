using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KonyvtarWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Book> books;
        private Database database;
        private Book? selectedBook;

        public MainWindow()
        {
            InitializeComponent();
            database = new Database();
            books = new List<Book>();

            if (database.TestConnection())
            {
                TxtStatus.Text = "Sikeres kapcsolat";
                LoadBooks();
            }
            else
            {
                TxtStatus.Text = "Sikertelen kapcsolat";
                MessageBox.Show("Nem sikerült csatlakozni az adatbázishoz. Ellenőrizze a kapcsolatot és próbálja újra.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadBooks()
        {
            try
            {
                books = database.GetBooks();
                BooksDataGrid.ItemsSource = null;
                BooksDataGrid.ItemsSource = books;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a könyvek betöltésekor: \n\n{ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                Book? book = GetBookFromForm();

                if (book == null)
                {
                    return;
                }
                database.InsertBook(book);

                LoadBooks();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nem sikerült hozzáadni a könyvet \n\n"
                    + ex.Message,
                    "Hiba",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
            }
        }

        private Book? GetBookFromForm(int id = 0)
        {
            string title = TxtTitle.Text.Trim();
            string author = TxtAuthor.Text.Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Szerző megadása kötelező");
                TxtAuthor.Focus();
                return null;
            }

            bool validYear = int.TryParse(TxtYear.Text.Trim(), out int publishedYear);
            if (!validYear)
            {
                MessageBox.Show("Kiadás évének egész számnak kell lennie");
                TxtYear.Focus();
                return null;
            }
            if (publishedYear < 0 || publishedYear > DateTime.Now.Year)
            {
                MessageBox.Show("Érvénytelen kiadási év");
                TxtYear.Focus();
                return null;
            }

            bool validPrice = decimal.TryParse(TxtPrice.Text.Trim(), out decimal price);
            if (!validPrice)
            {
                MessageBox.Show("Az árnak egész számnak kell lennie");
                TxtPrice.Focus();
                return null;
            }
            if (price < 0)
            {
                MessageBox.Show("Érvénytelen ár");
                TxtPrice.Focus();
                return null;
            }

            bool validStock = int.TryParse(TxtStock.Text.Trim(), out int stock);
            if (!validStock)
            {
                MessageBox.Show("A készletnek egész számnak kell lennie");
                TxtStock.Focus();
                return null;
            }
            if (stock < 0)
            {
                MessageBox.Show("Érvénytelen készletszám");
                TxtStock.Focus();
                return null;
            }

            if (id == 0)
            {
                return new Book(id, title, author, publishedYear, price, stock);
            }
            return new Book(id, title, author, publishedYear, price, stock);

        }
        private void BooksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (BooksDataGrid.SelectedItem is Book book)
            {
                selectedBook = book;
                TxtTitle.Text = book.Title;
                TxtAuthor.Text = book.Author;
                TxtYear.Text = book.PublishedYear.ToString();
                TxtPrice.Text = book.Price.ToString();
                TxtStock.Text = book.Stock.ToString();
            }
        }

        private void ClearForm()
        {
            selectedBook = null;
            TxtTitle.Text = string.Empty;
            TxtAuthor.Text = string.Empty;
            TxtYear.Text = string.Empty;
            TxtPrice.Text = string.Empty;
            TxtStock.Text = string.Empty;
            BooksDataGrid.SelectedItem = null;
        }
   
        

        private void UpdateBookButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedBook == null)
                {
                    MessageBox.Show("Elöször válaszd ki egy könyvet",
                        "Figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Book? updatedBook = GetBookFromForm(selectedBook.Id);
                if (updatedBook == null) { return; };

                database.UpdateBook(updatedBook);

                MessageBox.Show("Könyv sikeresen módosítva!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);

                
                LoadBooks();
                ClearForm();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Nem sikerült mődodítani a könyvet\n\n," + ex.Message,
                    "Hiba",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }


            
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            LoadBooks();
        }

        private void ClearFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void BooksDataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            selectedBook = BooksDataGrid.SelectedItem as Book;

            if(selectedBook == null)
            {
                return;
            }

            TxtTitle.Text = selectedBook.Title;
            TxtAuthor.Text = selectedBook.Author;
            TxtYear.Text = selectedBook.PublishedYear.ToString();
            TxtPrice.Text = selectedBook.Price.ToString();
            TxtStock.Text = selectedBook.Stock.ToString();
            TxtStatus.Text = $"Kiválasztva: {selectedBook.Title}";


        }

        private void DeleteBookButton_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                if (selectedBook == null)
                {
                    MessageBox.Show("Elöször válasz ki egy könyvet",
                        "Figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Warning);
                }MessageBoxResult result = MessageBox.Show(
                    "Biztosan törölni szeretnéd ezt a könyvet?\n\n"
                    +selectedBook.Title
                    +"\n",
                    "Megerősítés",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    database.DeleteBook(selectedBook.Id);

                    MessageBox.Show("Könyv sikeresen törölve!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadBooks();
                    ClearForm();
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Nem sikerült törölni a künyvet \n\n"
                    + ex.Message,
                    "Hiba",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }
    }
}