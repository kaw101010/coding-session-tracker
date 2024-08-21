using System.Configuration;
using System.Data;
using System.Data.SQLite;
using Dapper;
using Spectre.Console;

namespace coding_tracker.Models
{
    public class DatabaseConnector
    {
        public SQLiteConnection Connection { get; }
        private readonly string table_name = ConfigurationManager.AppSettings.Get("tableName") ?? "coding_sessions";
        private readonly string db_name = ConfigurationManager.AppSettings.Get("databaseName") ?? "codingsessions.db";

        private readonly int assignedId = 102;
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
                                START_TIME TEXT NOT NULL,
                                END_TIME TEXT,
                                DURATION TEXT,
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

        public bool HasIncompleteRecords() {
            var conn = this.Connection;
            var idForUpdateCommand = $@"SELECT ID, START_TIME FROM {this.table_name} WHERE DURATION is NULL AND END_TIME is NULL";
            var queryCurrentSession = conn.Query(idForUpdateCommand).ToList();
            return queryCurrentSession.Count > 0;
        }

        public void InsertRecordIntoTable(string start_time,
                                            string? end_time,
                                            string? duration,
                                            string? additional_comments) {
            string getMaxID = $@"SELECT IFNULL(MAX(ID), 0) FROM {this.table_name}";
            int maxIdQuery = Connection.ExecuteScalar<int>(getMaxID);
            int maxId = maxIdQuery == 0 ? assignedId : maxIdQuery;
            string insertRecordCommandSQL = $@"INSERT INTO {this.table_name} (ID, 
                                        START_TIME, END_TIME, DURATION, COMMENT) 
                                   VALUES (@Id, @StartTime, @EndTime, @Duration, @AdditionalComments)";

            var parameters = new {
                        Id = ++maxId,
                        StartTime = start_time,
                        EndTime = end_time,
                        Duration = duration,
                        AdditionalComments = additional_comments
                    };
            this.Connection.Execute(insertRecordCommandSQL, parameters);
        }

        public void UpdateRecordAndEndSession(Dictionary<string, object> columnsToUpdate)
        {
        var idForUpdateCommand = $@"SELECT ID, START_TIME FROM {this.table_name} WHERE DURATION is NULL AND END_TIME is NULL";
        var queryCurrentSession = this.Connection.Query(idForUpdateCommand).ToList();
        if (queryCurrentSession.Count > 1) {
            Console.WriteLine("System Encountering some errors! Please Wait!");
            return;
        }
        var currSessionId = queryCurrentSession[0].ID;
        var currSessionStartTime = queryCurrentSession[0].START_TIME;
        var setClauses = new List<string>();
        var parameters = new DynamicParameters();
        string currSessionEndTime = "";
        foreach (var column in columnsToUpdate)
        {
            if (column.Key == "END_TIME") {
                currSessionEndTime = column.Value.ToString() ?? "";
            }
            setClauses.Add($"{column.Key} = @{column.Key}");
            parameters.Add($"@{column.Key}", column.Value);
        }
        setClauses.Add($"{"DURATION = @DURATION"}");
        var duration = CodingSession.GetTimeSpan(
            DateTime.Parse(currSessionStartTime),
            DateTime.Parse(currSessionEndTime));
        parameters.Add("@DURATION", duration);

        string updateSessionCommandSQL = $@"UPDATE {this.table_name} SET {string.Join(", ", setClauses)} WHERE Id = @Id";
        parameters.Add("@Id", currSessionId);
        this.Connection.Execute(updateSessionCommandSQL, parameters);
        }

        public List<CodingSession> GetSessionsOnDate(DateOnly date)
        {
            string getSessionsOnDateCommand = $"SELECT * FROM {this.table_name} WHERE START_TIME LIKE @DATE";
            var parameters = new DynamicParameters();
            parameters.Add("@DATE", $"%{date:dd-MM-yyyy}%");
            var codingSessions = this.Connection.Query(
                getSessionsOnDateCommand, parameters, commandType: CommandType.Text)
                .Select(x => 
                {
                    // manually map some properties
                    var session = new CodingSession
                    {
                        Duration = x.DURATION,
                        Comments = x.COMMENT
                    };
                    return session;
                }
                )
                .ToList();
            return codingSessions;
        }
    }
}