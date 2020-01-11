using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Calendar.DatabaseManager
{
    public class DatabaseManagerModel : IDatabaseManager
    {
        private readonly string dbConnectionString;
        private SqlConnection sqlConnection;

        public DatabaseManagerModel(string connectionString)
        {
            dbConnectionString = connectionString;
        }

        private bool Connect()
        {
            try
            {
                sqlConnection = new SqlConnection(dbConnectionString);
                sqlConnection.Open();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void Disconnect()
        {
            if (sqlConnection != null)
            {
                sqlConnection.Close();
            }
        }

        public DateInfo[] GetDataFromDatabase()
        {
            Connect();

            var data = new List<DateInfo>();

            try
            {
                using (var command = new SqlCommand(@"select DateTime, TasksDescription from Data", sqlConnection))
                {
                    var sqlDataReader = command.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        var dateTime = DateTime.Parse((string)sqlDataReader.GetValue(0));
                        var tasksDescription = (string)sqlDataReader.GetValue(1);

                        data.Add(new DateInfo(dateTime, tasksDescription));
                    }
                }
            }
            catch (Exception)
            {
            }

            Disconnect();

            return data.ToArray();
        }

        public void WriteDataToDatabase(DateInfo[] dateTimeData)
        {
            Connect();

            try
            {
                //Delete
                using (var command = new SqlCommand($"delete from Data", sqlConnection))
                {
                    command.ExecuteNonQuery();
                }

                //Write
                foreach (var dateInfo in dateTimeData)
                {
                    using (var command = new SqlCommand($"insert into Data (DateTime, TasksDescription) values ('{dateInfo.DateTime.ToString()}', '{dateInfo.TasksDescription}')", sqlConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
            }

            Disconnect();
        }
    }
}
