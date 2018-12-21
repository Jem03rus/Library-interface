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

        public static string GetID(string RowValue, string selectedRowName, string tableName, string rowName, SqlConnection connection)
        {
            DataTable GetIDValue = new DataTable();
            string collunID = " ";
            GetIDValue.Clear();
            SqlCommand command = new SqlCommand("SELECT " + selectedRowName + " FROM " + tableName + " WHERE " + rowName + " = '" + RowValue + "'", connection);
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                adapter.Fill(GetIDValue);
            }
            collunID = GetIDValue.Rows[0][0].ToString();
            return collunID;
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

        public void Update(string table, string values)
        {
            SqlCommand command = new SqlCommand("UPDATE " + table + " SET " + values, connection);
            command.ExecuteNonQuery();
        }
    }
}
