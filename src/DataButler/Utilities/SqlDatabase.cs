namespace DataButler.Utilities
{
    public class SqlDatabase
    {
        public SqlDatabase(){}

        public SqlDatabase(string name, string lastBackupName)
        {
            Name = name;
            LastBackupName = lastBackupName;
        }

        public string Name { get; private set; }
        public string LastBackupName { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
