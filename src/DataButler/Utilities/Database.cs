using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using Dapper;
using System.Data.SqlClient;

namespace DataButler.Utilities
{
    public static class Database
    {
        static BackgroundWorker _bgw;
        public static string DefaultDataDirectory { get; set; }
        private const string ConnectionString = "Data Source=(local);Initial Catalog=master;Integrated Security=SSPI;";

        private static IDbConnection OpenConnection()
        {
            var con = new SqlConnection(ConnectionString);
            Log("Saying hello to SQL...");
            con.Open();
            Log("SQL greeted us.");
            SetDefaultDataDirectory(con);
            return con;
        }

        static void Log(string message, params string[] parameters)
        {
            if (_bgw != null)
                _bgw.ReportProgress(0, string.Format(message, parameters));
        }

        static void SetDefaultDataDirectory(SqlConnection con)
        {
            Log("Retrieving SQL's data directory...");
            var getSqlPathQuery = @"declare @SmoRoot nvarchar(512)
                                    exec master.dbo.xp_instance_regread N'HKEY_LOCAL_MACHINE', N'SOFTWARE\Microsoft\MSSQLServer\Setup', N'SQLPath', @SmoRoot OUTPUT
                                    select @SmoRoot";
            var sqlPath = con.Query<string>(getSqlPathQuery).SingleOrDefault();
            DefaultDataDirectory = Path.Combine(sqlPath, "Data");
            Log(string.Format("Found {0}.", DefaultDataDirectory));
        }

        public static string GetDatabaseName(string file, out string failure)
        {
            failure = string.Empty;
            try
            {
                using (var con = (SqlConnection) OpenConnection())
                {
                    con.InfoMessage += (s, e) => Log(e.Message);
                    var header =
                        con.Query(string.Format("RESTORE HEADERONLY FROM DISK = N'{0}' WITH NOUNLOAD", file))
                           .SingleOrDefault();
                    return header.DatabaseName;
                }
            }            
            catch (SqlException e)
            {
                failure = e.Message;
                return null;
            }
        }

        public static void RestoreDatabase(string file, string restoreAs, BackgroundWorker backgroundWorker = null)
        {
            _bgw = backgroundWorker;
            using (var con = (SqlConnection)OpenConnection())
            {
                con.InfoMessage += (sender, e) => Log(e.Message);
                DropExistingDatabase(restoreAs, con);
                RestoreDatabase(file, restoreAs, con);
            }
        }

        public static void DropExistingDatabase(string database, IDbConnection con)
        {
            
            if (!DatabaseExists(database)) return;

            var sql = string.Format(
                        @"If Exists(Select * from sysdatabases where name = '{0}')
                          BEGIN
	                          ALTER DATABASE [{0}] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
	                          EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'{0}'
	                          DROP DATABASE [{0}]
                          END", database);
            Log("Found existing database named {0}. Dropping...", database);
            con.Execute(sql);
            Log("{0} dropped.", database);
        }

        private static bool DatabaseExists(string database)
        {
            using (var con = OpenConnection())
            {
                return con.Query<bool>(string.Format("select cast(count(*) as bit) from sysdatabases where name = '{0}'", database)).SingleOrDefault();
            }
        }

        static void RestoreDatabase(string file, string restoreAs, IDbConnection con)
        {
            Log("Loading Meta from SQL Backup '{0}'...", file);
            var files = LoadFileList(restoreAs, file, con);
            Log("Restoring backup to {0}...", restoreAs);
            var sql = string.Format(@"RESTORE DATABASE [{0}] FROM  DISK = N'{1}' WITH  FILE = 1,  MOVE N'{2}' TO N'{3}',  MOVE N'{4}' TO N'{5}',  NOUNLOAD,  REPLACE,  STATS = 10", restoreAs, file, files.DatabaseLogicalName, files.DatabaseFile, files.LogLogicalName, files.LogFile);
            con.Execute(sql, commandTimeout: 900);
        }

        private static FileList LoadFileList(string restoreAs, string file, IDbConnection con)
        {
            var results = con.Query(string.Format("RESTORE FILELISTONLY FROM  DISK = N'{0}' WITH  NOUNLOAD,  FILE = 1", file)).ToList();
            var fileList = new FileList
                               {
                                   DatabaseLogicalName = results[0].LogicalName,
                                   DatabaseFile = GenerateNewPhysicalPath(results[0].PhysicalName, restoreAs, false),
                                   LogLogicalName = results[1].LogicalName,
                                   LogFile = GenerateNewPhysicalPath(results[1].PhysicalName, restoreAs, true)
                               };
            Log("Meta Output:");
            Log("DatabaseLogicalName = {0}", fileList.DatabaseLogicalName);
            Log("DatabaseFile = {0}", fileList.DatabaseFile);
            Log("LogLogicalName = {0}", fileList.LogLogicalName);
            Log("LogFile = {0}", fileList.LogFile);
            return fileList;
        }

        private static string GenerateNewPhysicalPath(string original, string restoreAs, bool log)
        {
            var databaseFileExt = Path.GetExtension(original);
            return string.Format(@"{0}\{1}{2}", DefaultDataDirectory, restoreAs, log ? string.Format("_log{0}", databaseFileExt) : databaseFileExt);
        }
    }
}
