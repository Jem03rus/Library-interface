using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;


namespace DataBaseLibrary
{
    /// <summary>
    /// Логика взаимодействия для AcceptBookWindow.xaml
    /// </summary>
    public partial class AcceptBookWindow : Window
    {
        sqlclass SQLWorker = new sqlclass();
        MainWindow update = new MainWindow();
        string connectionString;
        public AcceptBookWindow()
        {
            InitializeComponent();
            //connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            //SQLWorker.Connect(connectionString);
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //var table = SQLWorker.Select("ReaderName", "Reader");
            //ChooseNameComboBox.ItemsSource = table.DefaultView;
            //ChooseNameComboBox.DisplayMemberPath = table.Columns[0].ToString();
        }

        private void ChooseNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             string ComboBoxValue = ((System.Data.DataRowView)ChooseNameComboBox.SelectedItem).Row.ItemArray[0].ToString();
             string ComboBoxValueID = sqlclass.GetID(ComboBoxValue, "ReaderID", "Reader", "ReaderName", SQLWorker.connection);
             var table = SQLWorker.Select("Book.Name", "IssueOfBooks INNER JOIN Book ON Book.BookID = IssueOfBooks.BookID WHERE ReaderID = '" + ComboBoxValueID + "'");
            
             ChooseBookComboBox.ItemsSource = table.DefaultView;
             ChooseBookComboBox.DisplayMemberPath = table.Columns[0].ToString();
        }

        private void AcceptBookButton_Click(object sender, RoutedEventArgs e)
        {
            if ((ChooseNameComboBox.Text == "") || (ChooseBookComboBox.Text == "") || (ActualReturnDatePicker.Text == ""))
            {
                MessageBox.Show("Incorrect input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string ComboBoxValue = ((System.Data.DataRowView)ChooseNameComboBox.SelectedItem).Row.ItemArray[0].ToString();
                string ComboBoxValueID = sqlclass.GetID(ComboBoxValue, "ReaderID", "Reader", "ReaderName", SQLWorker.connection);
                SQLWorker.Update("IssueOfBooks", "IsReturned = '1', ActualReturnDate = '" + ActualReturnDatePicker.Text + "' WHERE ReaderID = '" + ComboBoxValueID + "'");
                update.DataGridUpdate();           
            }

        }
    }
}
