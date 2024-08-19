using System.Configuration;
using System.Data.SQLite;
using Spectre.Console;

namespace coding_tracker.Models
{
    public class DatabaseConnector
    {
        public SQLiteConnection Connection { get; }
        public DatabaseConnector()
        {
            var dbName = ConfigurationManager.AppSettings.Get("databaseName");
            this.Connection = new($"Data Source = {dbName}");
            this.Connection.Open();
        }
        
        public void CreateTable()
        {
            var comm = this.Connection.CreateCommand();
            comm.CommandText = @"CREATE TABLE coding_sessions(
                                ID INTEGER PRIMARY KEY,
                                DATE
                                )";
        }

    }
}