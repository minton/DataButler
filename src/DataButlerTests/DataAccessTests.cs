using DataButler.Utilities;
using NUnit.Framework;

namespace DataButlerTests
{
    [TestFixture]
    public class DataAccessTests : DataAccessTestsBase
    {

        [Test]
        public void FailsWhenGivenAnInvalidFile()
        {
            string failure;
            var result = Database.GetDatabaseName(@"C:\Windows\write.exe", out failure);
            Assert.IsNull(result);
            Assert.IsNotNullOrEmpty(failure);
        }
        [Test]
        public void CanRetreiveDatabaseName()
        {
            string failure;
            var name = Database.GetDatabaseName(SampleBackup, out failure);
            Assert.AreEqual("Sample", name);
        }
        [Test]
        public void CanRestoreToNonExistingDatabase()
        {
            Database.RestoreDatabase(SampleBackup, DatabaseName);
        }
        [Test]
        public void CanRestoreDatabaseOverwrittingExisting()
        {
            //Create original
            Database.RestoreDatabase(SampleBackup, DatabaseName);
            //Attemp to overwrite it
            Database.RestoreDatabase(SampleBackup, DatabaseName);
        }
        [Test]
        public void CanRestoreDatabaseToDifferentName()
        {
            Database.RestoreDatabase(SampleBackup, DatabaseName2);
        }
    }
}
