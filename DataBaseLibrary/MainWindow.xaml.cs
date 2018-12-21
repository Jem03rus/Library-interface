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
            if ((BookNameTextBox.Text == "") || (AuthorTextBox.Visibility == Visibility.Visible && AuthorTextBox.Text == "") || (GenreTextBox.Visibility == Visibility.Visible && GenreTextBox.Text == "") || (AuthorComboBox.Visibility == Visibility.Visible && GenreComboBox.Text == "" || GenreComboBox.Visibility == Visibility.Visible && AuthorComboBox.Text == ""))
            {
                MessageBox.Show("Incorrect input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string bookID = Guid.NewGuid().ToString();
                string authorValue = "";
                string genreValue = "";

                if (AuthorTextBox.Visibility == Visibility.Visible)
                {
                    string authorGuid = Guid.NewGuid().ToString().Trim();
                    SQLWorker.Insert("'" + authorGuid + "', '" + AuthorTextBox.Text + "'", "Author");
                    authorValue = AuthorTextBox.Text;
                    ComboBoxConnection("Author");
                }
                else
                {
                    authorValue = AuthorComboBox.Text;
                }
                if (GenreTextBox.Visibility == Visibility.Visible)
                {
                    string genreGuid = Guid.NewGuid().ToString().Trim();
                    SQLWorker.Insert("'" + genreGuid + "', '" + GenreTextBox.Text + "'", "Genre");
                    genreValue = GenreTextBox.Text;
                    ComboBoxConnection("Genre");
                }
                else
                {
                    genreValue = GenreComboBox.Text;
                }

                var GenreID = sqlclass.GetID(genreValue, "GenreID", "Genre", "Genre", SQLWorker.connection);
                var AuthorID = sqlclass.GetID(authorValue, "AuthorID", "Author", "Author", SQLWorker.connection);



                SQLWorker.Insert("'" + bookID + "', '" + AuthorID + "', '" + GenreID + "', '" + BookNameTextBox.Text + "'", "Book");
                MessageBox.Show("Data successfully saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ComboBoxConnection("Book");
            }
        }

        private void SaveIssueBook_Click(object sender, RoutedEventArgs e)
        {
            if ((ReturnDAteDatePicker.Text == "") || (DateOfIssueDatePicker.Text == "") || (ReaderNameComboBox.Text == "") || (BookComboBox.Text == ""))
            {
                MessageBox.Show("Incorrect input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var BookID = sqlclass.GetID(BookComboBox.Text, "BookID", "Book", "Name", SQLWorker.connection);
                var ReaderID = sqlclass.GetID(ReaderNameComboBox.Text, "ReaderID", "Reader", "ReaderName", SQLWorker.connection);
                string id = Guid.NewGuid().ToString().Trim();
                SQLWorker.Insert("'" + id + "', '" + ReaderID + "', '" + DateOfIssueDatePicker.Text + "', '" + ReturnDAteDatePicker.Text + "', '" + BookID + "' , '" + "False' , NULL", "IssueOfBooks");
                MessageBox.Show("Data successfully saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
                ComboBoxConnection("Reader");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBoxConnection("Author");
            ComboBoxConnection("Genre");
            ComboBoxConnection("Book");
            ComboBoxConnection("Reader");

            var issueofbook = SQLWorker.Select("Reader.ReaderName, Reader.Adress, Reader.PhoneNumber, DateOfIssue, ReturnDate, Book.Name, IsReturned, ActualReturnDate", "IssueOfBooks INNER JOIN Reader ON IssueOfBooks.ReaderID = Reader.ReaderID INNER JOIN Book ON IssueOfBooks.BookID = Book.BookID ");

            ReaderTableGrid.ItemsSource = SQLWorker.Select("ReaderName, Adress, Email, PhoneNumber","Reader").DefaultView;

            IssueOfBookDataGrid.ItemsSource = issueofbook.DefaultView;

            IssueBookDataGrid.ItemsSource = SQLWorker.Select("Reader.ReaderName, Reader.Adress, Reader.PhoneNumber, DateOfIssue, ReturnDate, Book.Name", "IssueOfBooks INNER JOIN Reader ON IssueOfBooks.ReaderID = Reader.ReaderID INNER JOIN Book ON IssueOfBooks.BookID = Book.BookID ").DefaultView;

            AddBookDataGrid.ItemsSource = SQLWorker.Select("Name, Author.Author, Genre.Genre","Book INNER JOIN Author ON Book.AuthorID = Author.AuthorID INNER JOIN Genre ON Book.GenreID = Genre.GenreID").DefaultView;

            BookIssuedLabel.Content = BookIssuedLabel.Content + Convert.ToString(issueofbook.Rows.Count);
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
            ReaderNameTextBox.Text = "";
            AdressTextBox.Text = "";
            EmailTextBox.Text = "";
            PhoneNumberTextBox.Text = "";
        }

        private void IssueBookBackButton_Click(object sender, RoutedEventArgs e)
        {
            IssueBook.Visibility = Visibility.Hidden;
            IssueBookTable.Visibility = Visibility.Visible;
            ReaderNameComboBox.Text = "";
            DateOfIssueDatePicker.Text = "";
            ReturnDAteDatePicker.Text = "";
            BookComboBox.Text = "";
            IssueOfBookDataGrid.ItemsSource = SQLWorker.Select("Reader.ReaderName, Reader.Adress, Reader.PhoneNumber, DateOfIssue, ReturnDate, Book.Name, IsReturned, ActualReturnDate", "IssueOfBooks INNER JOIN Reader ON IssueOfBooks.ReaderID = Reader.ReaderID INNER JOIN Book ON IssueOfBooks.BookID = Book.BookID ").DefaultView;
        }
    

        private void AddBookBackButton_Click(object sender, RoutedEventArgs e)
        {
            IssueBookTable.Visibility = Visibility.Visible;
            AddBook.Visibility = Visibility.Hidden;
            BookNameTextBox.Clear();
            AuthorTextBox.Clear();
            AuthorComboBox.Text = "";
            GenreTextBox.Clear();
            GenreComboBox.Text = "";
            AuthorTextBox.Visibility = Visibility.Hidden;
            AuthorComboBox.Visibility = Visibility.Visible;
            GenreTextBox.Visibility = Visibility.Hidden;
            GenreComboBox.Visibility = Visibility.Visible;
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

        private void AddGenreButton_Click(object sender, RoutedEventArgs e)
        {
            if (GenreComboBox.Visibility == Visibility.Visible)
            {
                GenreComboBox.Visibility = Visibility.Hidden;
                GenreTextBox.Visibility = Visibility.Visible;
                AddOrChooseGenre.ToolTip = "Chooose genre";
            }
            else if (GenreComboBox.Visibility == Visibility.Hidden)
            {
                GenreTextBox.Visibility = Visibility.Hidden;
                GenreComboBox.Visibility = Visibility.Visible;
                AddOrChooseGenre.ToolTip = "Add genre";
            }

        }

        private void AddAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorComboBox.Visibility == Visibility.Visible)
            {
                AuthorComboBox.Visibility = Visibility.Hidden;
                AuthorTextBox.Visibility = Visibility.Visible;
                AddOrChooseAuthor.ToolTip = "Chooose author";
            }
            else if (AuthorComboBox.Visibility == Visibility.Hidden)
            {
                AuthorComboBox.Visibility = Visibility.Visible;
                AuthorTextBox.Visibility = Visibility.Hidden;
                AddOrChooseAuthor.ToolTip = "Add author";
            }

        }

        private void AcceptBookButton_Click(object sender, RoutedEventArgs e)
        {
            AcceptBookWindow AcceptBookWindow = new AcceptBookWindow();
            AcceptBookWindow.Visibility = Visibility.Visible;
        }
    }
}