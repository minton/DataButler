using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using DataButler.Utilities;
using NUnit.Framework;

namespace DataButlerTests
{
    public class DataAccessTestsBase
    {
        [TestFixtureSetUp]
        public void Initialize()
        {
            //Locate SQL Default Backup Path
            var randomFileName = string.Format("{0}.bak", Path.GetRandomFileName());
            SampleBackup = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), randomFileName);
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DataButlerTests.Sample.bak"))
            using (var outStream = File.Create(SampleBackup))
            {
                stream.CopyTo(outStream);
                outStream.Flush();
                outStream.Close();
            }
            DatabaseName = Path.GetRandomFileName();
            DatabaseName2 = Path.GetRandomFileName();
        }

        [SetUp]
        [TearDown]
        public void CleanUpDatabases()
        {
            DropDatabaseIfExists(DatabaseName);
            DropDatabaseIfExists(DatabaseName2);            
        }

        static void DropDatabaseIfExists(string database)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                Database.DropExistingDatabase(database, con);
            }
        }

        protected string SampleBackup { get; private set; }
        protected string DatabaseName { get; private set; }
        protected string DatabaseName2 { get; private set; }
        private const string ConnectionString = "Data Source=(local);Initial Catalog=master;Integrated Security=SSPI;";
    }
}