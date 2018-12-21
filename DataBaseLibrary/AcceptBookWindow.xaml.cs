using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DataBaseLibrary
{
    /// <summary>
    /// Логика взаимодействия для AcceptBookWindow.xaml
    /// </summary>
    public partial class AcceptBookWindow : Window
    {
        sqlclass SQLWorker = new sqlclass();
        string connectionString;
        public AcceptBookWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SQLWorker.Connect(connectionString);
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var table = SQLWorker.Select("ReaderName", "Reader");
            ChooseNameComboBox.ItemsSource = table.DefaultView;
            ChooseNameComboBox.DisplayMemberPath = table.Columns[0].ToString();

            
            aNTON GONDON
        }

        private void ChooseNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             string ComboBoxValue = ((System.Data.DataRowView)ChooseNameComboBox.SelectedItem).Row.ItemArray[0].ToString();
             string ComboBoxValueID = sqlclass.GetID(ComboBoxValue, "ReaderID", "Reader", "ReaderName", SQLWorker.connection);
             var table = SQLWorker.Select("BookID", "IssueOfBooks WHERE ReaderID = '" + ComboBoxValueID + "'");
            
             ChooseBookComboBox.ItemsSource = table.DefaultView;
             ChooseBookComboBox.DisplayMemberPath = table.Columns[0].ToString();
        }
    }
}
