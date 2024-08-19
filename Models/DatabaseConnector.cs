using System.Configuration;
using System.Data.SQLite;
using Dapper;

namespace coding_tracker.Models
{
    public class DatabaseConnector
    {
        public SQLiteConnection Connection { get; }
        private readonly string table_name = ConfigurationManager.AppSettings.Get("tableName") ?? "coding_sessions";
        private readonly string db_name = ConfigurationManager.AppSettings.Get("databaseName") ?? "codingsessions.db";
        public DatabaseConnector()
        {
            this.Connection = new($"Data Source = {this.db_name}");
        }

        public void OpenConnection() {
            this.Connection.Open();
        }
        
        public void CreateTable()
        {
            // create a new table
            var conn = this.Connection;
            string createTableCommandSQL = $@"CREATE TABLE IF NOT EXISTS {this.table_name} (
                                ID INTEGER PRIMARY KEY,
                                DATE TEXT NOT NULL,
                                START_TIME TEXT NOT NULL,
                                END_TIME TEXT NOT NULL,
                                DURATION,
                                COMMENT TEXT
                                );";
            conn.Execute(createTableCommandSQL);
        }

        public void CloseConnection() {
            this.Connection.Close();
        }

        public bool DatabaseNotConnected() {
            return this.Connection.State != System.Data.ConnectionState.Open;
        }

        public void insertRecordIntoTable() {
            string insertRecordCommandSQL = $@"INSERT INTO {this.table_name} VALUES(
                                            )";
        }
    }
}