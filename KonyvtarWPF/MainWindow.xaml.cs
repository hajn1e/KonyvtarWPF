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
                StatusTextBlock.Text = "Sikeres kapcsolat";
                LoadBooks();
            }
            else
            {
                StatusTextBlock.Text = "Sikertelen kapcsolat";
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




    }
}