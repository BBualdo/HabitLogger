using DatabaseLibrary;

DbContext db = new DbContext();

Engine appEngine = new Engine();

while (appEngine.IsOn)
{
  appEngine.MainMenu();
}