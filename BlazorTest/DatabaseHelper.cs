using System.Data.SQLite;
using System.IO;
using static MudBlazor.CategoryTypes;

//tutorial by Rory Mulcahey used: https://www.youtube.com/watch?v=7TpljNN0IvA

public static class DatabaseHelper
{
    //private static string connectionString = @"Data Source= ..\BlazorTest\Files\SQLite_XLN_chatlogs.db;Version=3";
    public static string DbCall()
    {
        SQLiteConnection sqlite_conn;
        sqlite_conn = CreateConnection();
        InitializeDatabase(sqlite_conn);
        InsertData(sqlite_conn);
        ReadData(sqlite_conn);
        return ("Success");
    }


    public static string InitializeDatabase(SQLiteConnection connection)
    {
        if (!File.Exists(@"..\BlazorTest\Files\SQLite_XLN_chatlogs.db"))
        {
            SQLiteConnection.CreateFile(@"..\BlazorTest\Files\SQLite_XLN_chatlogs.db");
            //using (var connection = new SQLiteConnection(connectionString))
            {

                SQLiteCommand sqlite_cmd;
                string CreateTablesql = @"
                    CREATE TABLE Chatlogs (
                        ItemID INTEGER NOT NULL UNIQUE,
	                    ConversationID INTEGER NOT NULL,
                        Username TEXT,
	                    IssueID INTEGER,
                        Message TEXT,
	                    TimeSent TEXT NOT NULL,
                        IssueSolved INTEGER,
	                    PRIMARY KEY(ItemID AUTOINCREMENT)
                    )";
                sqlite_cmd = connection.CreateCommand();
                sqlite_cmd.CommandText = CreateTablesql;
                sqlite_cmd.ExecuteNonQuery();
                return ("Table created");
            }
        }
        else
        {
            return ("Database already exists");
        }
    }

    static SQLiteConnection CreateConnection()
    {

        SQLiteConnection sqlite_conn;
        // Create a new database connection:
        sqlite_conn = new SQLiteConnection("Data Source= ..\\BlazorTest\aFiles\\SQLite_XLN_chatlogs.db; Version = 3; New = True; Compress = True; ");
         // Open the connection:
         try
        {
            sqlite_conn.Open();
        }
        catch (Exception ex)
        {
            Console.WriteLine("PERSONAL ERROR: " + ex);
        }
        return sqlite_conn;
    }

    static void InsertData(SQLiteConnection conn)
    {
        SQLiteCommand sqlite_cmd;
        sqlite_cmd = conn.CreateCommand();
        sqlite_cmd.CommandText = @"
            INSERT INTO Chatlogs (ConversationID, Username, IssueID, Message, TimeSent, IssueSolved
                VALUES(2,'Daisy_B', 0, 'Welcome', '2024-19-03 10:04:35', 0); ";
         sqlite_cmd.ExecuteNonQuery();


    }

    static void ReadData(SQLiteConnection conn)
    {
        SQLiteDataReader sqlite_datareader;
        SQLiteCommand sqlite_cmd;
        sqlite_cmd = conn.CreateCommand();
        sqlite_cmd.CommandText = "SELECT * FROM Chatlogs";

        sqlite_datareader = sqlite_cmd.ExecuteReader();
        while (sqlite_datareader.Read())
        {
            string myreader = sqlite_datareader.GetString(0);
            Console.WriteLine(myreader);
        }
        conn.Close();
    }
}