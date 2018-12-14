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
        string procedureName;
        string connectionName;
        string connectionString;
        SqlDataAdapter adapter;
        DataTable libraryTable;

        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }


        private void NewMethod(string Name)
        {
            string sql = "SELECT * FROM " + Name;
            libraryTable = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);

                adapter.InsertCommand = new SqlCommand("sp_" + Name, connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                if (procedureName == "BookName")
                {
                    //  adapter.InsertCommand.Parameters.Add();
                    //  SqlParameter parameter = adapter.InsertCommand.Parameters.Add(qq.Text);
                    using (command = new SqlCommand("INSERT INTO " + Name + " VALUES(@Author, @AuthorID, @BookID)", connection))
                    {
                        command.Parameters.Add(new SqlParameter("Author", AuthorTextBox.Text));
                    }
                }
                else if (procedureName == "Author")
                {
                    adapter.InsertCommand.Parameters.Add("@Author", SqlDbType.VarChar, 50, "Author");
                    SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@BookID", SqlDbType.Int, 0, "BookID");
                    parameter.Direction = ParameterDirection.Output;
                }
                else if (procedureName == "Genre")
                {
                    adapter.InsertCommand.Parameters.Add("@Genre", SqlDbType.VarChar, 50, "Genre");
                    SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@BookID", SqlDbType.Int, 0, "BookID");
                    parameter.Direction = ParameterDirection.Output;
                }
                else if (procedureName == "ReaderInfo")
                {
                    adapter.InsertCommand.Parameters.Add("@Name", SqlDbType.VarChar, 50, "Name");
                    adapter.InsertCommand.Parameters.Add("@Adress", SqlDbType.VarChar, 50, "Adress");
                    SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@ReaderID", SqlDbType.Int, 0, "BookID");
                    parameter.Direction = ParameterDirection.Output;
                }
                else if (procedureName == "IssueOfBook")
                {
                    adapter.InsertCommand.Parameters.Add("@DateOfIssue", SqlDbType.VarChar, 50, "DateOfIssue");
                    adapter.InsertCommand.Parameters.Add("@ReturnDate", SqlDbType.VarChar, 50, "ReturnDate");
                    adapter.InsertCommand.Parameters.Add("@BookID", SqlDbType.Int, 0, "BookID");
                    SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@ReaderID", SqlDbType.Int, 0, "BookID");
                    parameter.Direction = ParameterDirection.Output;
                }

                adapter.Fill(libraryTable);
                LibraryGrid.ItemsSource = libraryTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NewMethod(procedureName = "BookName");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            NewMethod(procedureName = "Author");
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            NewMethod(procedureName = "Genre");
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            NewMethod(procedureName = "ReaderInfo");
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            NewMethod(procedureName = "IssueOfBook");
        }



        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (LibraryGrid.SelectedItems != null)
            {
                for (int i = 0; i < LibraryGrid.SelectedItems.Count; i++)
                {
                    DataRowView datarowView = LibraryGrid.SelectedItems[i] as DataRowView;
                    if (datarowView != null)
                    {
                        DataRow dataRow = (DataRow)datarowView.Row;
                        dataRow.Delete();
                    }
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();
            try
            {
                using (SqlCommand command = new SqlCommand("INSERT * FROM BookName", connection))
                {
                    command.Parameters.Add(new SqlParameter("Name", BookNameTextBox.Text));
                    command.Parameters.Add(new SqlParameter("BookID", 1));
                    command.Parameters.Add(new SqlParameter("GenreID", 13));
                    command.Parameters.Add(new SqlParameter("AuthorID", 2));
                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                Console.WriteLine("Count not insert.");
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBoxConnection(connectionName = "Author");
            ComboBoxConnection(connectionName = "Genre");
        }

        private void ComboBoxConnection(string name)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM " + name, connection);
            DataSet ds = new DataSet();
            da.Fill(ds, name);

            if (connectionName == "Author")
            {
                AuthorComboBox.DisplayMemberPath = ds.Tables[0].Columns[name].ToString();
                AuthorComboBox.ItemsSource = ds.Tables[0].DefaultView;
            }
            else if (connectionName == "Genre")
            {
                GenreComboBox.DisplayMemberPath = ds.Tables[0].Columns[name].ToString();
                GenreComboBox.ItemsSource = ds.Tables[0].DefaultView;
            }

        }


        private void UpdateDB()
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter);
            adapter.Update(libraryTable);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UpdateDB();
            MessageBox.Show("Данные сохранены");
        }

        private void AuthorButton_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorComboBox.Visibility == Visibility.Visible)
            {
                AuthorComboBox.Visibility = Visibility.Hidden;
                AuthorTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                AuthorComboBox.Visibility = Visibility.Visible;
                AuthorTextBox.Visibility = Visibility.Hidden;
            }
        }

        private void BookNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
