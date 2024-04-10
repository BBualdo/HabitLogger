using Microsoft.Data.Sqlite;

namespace DatabaseLibrary
{
  public class DbContext
  {
    private SqliteConnection _Connection { get; set; }

    public DbContext()
    {
      _Connection = new SqliteConnection("Data Source=habits.db");
    }
  }
}
