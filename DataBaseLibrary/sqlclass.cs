using System.Windows;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System;

namespace DataBaseLibrary
{
    public class sqlclass
    {
        public sqlclass()
        {
        }
        public  SqlConnection connection;
        DataTable rowValueByID = new DataTable();

        public void Connect(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public DataTable Select(string row ,string table)
        {
            DataTable LibraryTable = new DataTable();
            SqlCommand command = new SqlCommand("Select " +row+ " FROM " + table, connection);

            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                adapter.Fill(LibraryTable);
            }

            return LibraryTable;
        }

        public static string GetID(string RowValue, string tableName, string rowName, SqlConnection connection)
        {
            DataSet GetIDValue = new DataSet();
            string collunID = " ";
            GetIDValue.Clear();
            SqlCommand command = new SqlCommand("SELECT " + tableName + "ID FROM " + tableName + " WHERE " + rowName + " = '" + RowValue + "'", connection);
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                adapter.Fill(GetIDValue);
            }
            collunID = GetIDValue.Tables[0].Rows[0][0].ToString();
            return collunID;
        }

        public DataTable RowValueByID(string rowSelect, string tableName, string row, string rowValue,  SqlConnection connection)
        {
            SqlCommand command = new SqlCommand("SELECT " + rowSelect + " FROM " + tableName + " WHERE " + row + " = '" + rowValue + "'", connection);
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                adapter.Fill(rowValueByID);
            }
            return rowValueByID;
        }


        public void Delete(string key, string keyValue, string table)
        {
            SqlCommand command = new SqlCommand("Delete From @table Where @key = @keyValue", connection);
            command.Parameters.AddWithValue("@table", table);
            command.Parameters.AddWithValue("@key", key);
            command.Parameters.AddWithValue("@keyValue", keyValue);
            int number = command.ExecuteNonQuery();
        }

        public void Insert(string value, string table)
        {
            SqlCommand command = new SqlCommand("INSERT INTO " + table + " VALUES ("+ value +")", connection);
            command.ExecuteNonQuery();
        }

        public DataTable CrateTAnle()
        {
            SqlCommand command = new SqlCommand("SELECT BookName FROM Book INNER JOIN IssueOfBooks ON Book.BookID = IssueOfBooks.BookID", connection);
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                adapter.Fill(rowValueByID);
            }
            return rowValueByID;
        }
    }
}
