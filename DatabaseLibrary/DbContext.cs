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
      SeedHabitTable();
      SeedRecordTable();
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

    private void SeedHabitTable()
    {
      using (_Connection)
      {
        _Connection.Open();

        string countHabitRowsQuery = @"SELECT COUNT(*) FROM habit";

        using (SqliteCommand countCommand = new SqliteCommand(countHabitRowsQuery, _Connection))
        {
          int rowsNumber = Convert.ToInt32(countCommand.ExecuteScalar());

          if (rowsNumber == 0)
          {
            Dictionary<string, string> defaultHabits = new Dictionary<string, string>() {
              { "running", "kilometers" },
              { "swimming", "minutes" },
              { "reading", "pages" },
              { "drinking_water", "glasses" },
              { "meditating", "times" }
            };

            foreach (KeyValuePair<string, string> entry in defaultHabits)
            {
              Console.Clear();
              Console.WriteLine("Please wait...");
              Console.WriteLine($"Loading {entry.Key} habit...");
              string insertHabitQuery = $"INSERT INTO habit(name, unit) VALUES('{entry.Key}', '{entry.Value}')";
              using (SqliteCommand insertCommand = new SqliteCommand(insertHabitQuery, _Connection))
              {
                insertCommand.ExecuteNonQuery();
              }
            }
          }
        }

        Console.Clear();
        _Connection.Close();
      }
    }

    private void SeedRecordTable()
    {
      using (_Connection)
      {
        _Connection.Open();

        string countRecordRowsQuery = "SELECT COUNT(*) FROM record";

        using (SqliteCommand countCommand = new SqliteCommand(countRecordRowsQuery, _Connection))
        {
          int rowsNumber = Convert.ToInt32(countCommand.ExecuteScalar());

          if (rowsNumber == 0)
          {
            Console.Clear();
            Console.WriteLine("Loading...");

            string getHabitIdQuery = "SELECT habit_id FROM habit";

            using (SqliteCommand getHabitIdCommand = new SqliteCommand(getHabitIdQuery, _Connection))
            {
              using (SqliteDataReader reader = getHabitIdCommand.ExecuteReader())
              {
                while (reader.Read())
                {
                  int habitID = reader.GetInt32(0);

                  for (int i = 0; i < 5; i++)
                  {
                    Random random = new Random();
                    string date = DateTime.Now.AddDays(-random.Next(1, 365)).ToString("dd-mm-yy");
                    int quantity = random.Next(1, 10);

                    string insertRecordQuery = $"INSERT INTO record(habit_id, date, quantity) VALUES({habitID}, '{date}', {quantity})";

                    using (SqliteCommand insertCommand = new SqliteCommand(insertRecordQuery, _Connection))
                    {
                      insertCommand.ExecuteNonQuery();
                    }
                  }
                }
              }
            }
          }
        }
        Console.Clear();
        _Connection.Close();
      }
    }
  }
}
