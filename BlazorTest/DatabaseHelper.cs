using System.Data.SQLite;
using System.IO;

//tutorial by Rory Mulcahey used: https://www.youtube.com/watch?v=7TpljNN0IvA

public static class DatabaseHelper
{
    private static string connectionString = @"Data Source= ..\BlazorTest\Files\SQLite_XLN_chatlogs.db;Version=3";

    public static string InitializeDatabase()
    {
        if (!File.Exists(@"..\BlazorTest\Files\SQLite_XLN_chatlogs.db"))
        {
            SQLiteConnection.CreateFile(@"..\BlazorTest\Files\SQLite_XLN_chatlogs.db");

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string createChatlogsTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Chatlogs (
	                    ItemID INTEGER NOT NULL UNIQUE,
	                    ConversationID INTEGER NOT NULL,
	                    Username TEXT,
	                    IssueID INTEGER,
	                    Message TEXT,
	                    TimeSent TEXT NOT NULL,
	                    IssueSolved INTEGER,
	                    PRIMARY KEY(ItemID AUTOINCREMENT)
                    )";

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = createChatlogsTableQuery;
                    command.ExecuteNonQuery();
                    return ("Database created");
                }
            }
        }
        else
        {
            return ("Databse already exists");
        }
    }
}