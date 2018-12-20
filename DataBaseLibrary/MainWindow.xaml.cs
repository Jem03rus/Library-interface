using System;
using System.Windows;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace DataBaseLibrary
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString;
        sqlclass SQLWorker;

        public MainWindow()
        {
            SQLWorker = new sqlclass();
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SQLWorker.Connect(connectionString);
        }

        private void SaveBookButton_Click(object sender, RoutedEventArgs e)
        {
            if ((GenreComboBox.Text == "") || (AuthorComboBox.Text == "") || (BookNameTextBox.Text == ""))
            {
                MessageBox.Show("Incorrect input");
            }
            else
            {
                var GenreID = sqlclass.GetID(GenreComboBox.Text, "Genre", "Genre", SQLWorker.connection);
                var AuthorID = sqlclass.GetID(AuthorComboBox.Text, "Author", "Author", SQLWorker.connection);
                string id = Guid.NewGuid().ToString().Trim();
                SQLWorker.Insert("'" + id + "', '" + AuthorID + "', '" + GenreID + "', '" + BookNameTextBox.Text + "'", "Book");
            }
        }

        private void SaveIssueBook_Click(object sender, RoutedEventArgs e)
        {
            if ((ReturnDAteDatePicker.Text == "") || (DateOfIssueDatePicker.Text == "") || (ReaderNameComboBox.Text == "") || (BookComboBox.Text == ""))
            {
                MessageBox.Show("Incorrect input");
            }
            else
            {
                var BookID = sqlclass.GetID(BookComboBox.Text, "Book", "Name", SQLWorker.connection);
                var ReaderID = sqlclass.GetID(ReaderNameComboBox.Text, "Reader", "ReaderName", SQLWorker.connection);
                string id = Guid.NewGuid().ToString().Trim();
                SQLWorker.Insert("'" + id + "', '" + ReaderID + "', '" + DateOfIssueDatePicker.Text + "', '" + ReturnDAteDatePicker.Text + "', '" + BookID + "'", "IssueOfBooks");
            }
        }

        private void SaveReader_Click(object sender, RoutedEventArgs e)
        {
            if (PhoneNumberTextBox.Text.Length != 11)
            {
                MessageBox.Show("Incorrect phone number entry", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else  if ((ReaderNameTextBox.Text == "") || (AdressTextBox.Text == "") || (EmailTextBox.Text == "") || (PhoneNumberTextBox.Text == ""))
            {

                MessageBox.Show("Incorrect input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string id = Guid.NewGuid().ToString().Trim();
                SQLWorker.Insert("'" + id + "', '" + ReaderNameTextBox.Text + "', '" + AdressTextBox.Text + "', '" + EmailTextBox.Text + "', " + PhoneNumberTextBox.Text, "Reader");
                MessageBox.Show("Data successfully saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBoxConnection("Author");
            ComboBoxConnection("Genre");
            ComboBoxConnection("Book");
            ComboBoxConnection("Reader");


            var sss = SQLWorker.CrateTAnle();
            IssueOfBookDataGrid.ItemsSource = sss.DefaultView;
        }


        private void ComboBoxConnection(string TableName)
        {
            var table = SQLWorker.Select("*", TableName);
            if (TableName == "Author")
            {
                AuthorComboBox.ItemsSource = table.DefaultView;
                AuthorComboBox.DisplayMemberPath = table.Columns["Author"].ToString();
            }
            else if (TableName == "Genre")
            {
                GenreComboBox.ItemsSource = table.DefaultView;
                GenreComboBox.DisplayMemberPath = table.Columns["Genre"].ToString();
            }
            else if (TableName == "Book")
            {
                BookComboBox.ItemsSource = table.DefaultView;
                BookComboBox.DisplayMemberPath = table.Columns["Name"].ToString();
            }
            else if (TableName == "Reader")
            {
                ReaderNameComboBox.ItemsSource = table.DefaultView;
                ReaderNameComboBox.DisplayMemberPath = table.Columns["ReaderName"].ToString();
            }
        }

        private void PhoneNumberTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.Text, 0));
        }

        private void AddReaderBackButton_Click(object sender, RoutedEventArgs e)
        {
            AddReader.Visibility = Visibility.Hidden;
            IssueBookTable.Visibility = Visibility.Visible;
        }

        private void IssueBookBackButton_Click(object sender, RoutedEventArgs e)
        {
            IssueBook.Visibility = Visibility.Hidden;
            IssueBookTable.Visibility = Visibility.Visible;
        }

        private void InsertBookBackButton_Click(object sender, RoutedEventArgs e)
        {
            IssueBookTable.Visibility = Visibility.Visible;
            AddBook.Visibility = Visibility.Hidden;
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            IssueBookTable.Visibility = Visibility.Hidden;
            AddBook.Visibility = Visibility.Visible;
        }

        private void IssueBookMenu_Click(object sender, RoutedEventArgs e)
        {
            IssueBookTable.Visibility = Visibility.Hidden;
            IssueBook.Visibility = Visibility.Visible;
        }

        private void AddReaderButton_Click(object sender, RoutedEventArgs e)
        {
            IssueBookTable.Visibility = Visibility.Hidden;
            AddReader.Visibility = Visibility.Visible;
        }

    }
}