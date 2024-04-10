using Microsoft.Data.Sqlite;

namespace DatabaseLibrary
{
  public class DbContext
  {
    private SqliteConnection _Connection { get; set; }

    public DbContext()
    {
      _Connection = new SqliteConnection("Data Source=habits.db");
      CreateTables();
    }

    private void CreateTables()
    {
      using (_Connection)
      {
        _Connection.Open();

        string createTablesQuery = @"CREATE TABLE IF NOT EXISTS habit(
                                    habit_id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    name TEXT,
                                    unit TEXT);
                                    CREATE TABLE IF NOT EXISTS record(
                                    record_id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    habit_id INTEGER,
                                    date TEXT,
                                    quantity INTEGER,
                                    FOREIGN KEY(habit_id) REFERENCES habit(habit_id)
                                    )";

        using (SqliteCommand command = new SqliteCommand(createTablesQuery, _Connection))
        {
          command.ExecuteNonQuery();
        }

        _Connection.Close();
      }
    }
  }
}
