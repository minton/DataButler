using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Dapper;

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

        public static SqlDatabase[] GetDatabases(out string failure)
        {
            const string sql = @"select name from sys.databases where name not in (N'master', N'tempdb', N'model', N'msdb', N'ReportServer', N'ReportServerTempDB') order by name";
            failure = string.Empty;
            try
            {
                using (var con = (SqlConnection)OpenConnection())
                {
                    con.InfoMessage += (s, e) => Log(e.Message);
                    var databases = con.Query<SqlDatabase>(sql).ToArray();
                    const string lastBackupSql = @"select top 1
		                                                       bmf.physical_device_name
                                                       from msdb.dbo.backupset bs
                                                       join msdb.dbo.backupmediafamily bmf on bmf.media_set_id = bs.media_set_id
                                                       where bs.type = 'd' 
                                                       and bs.database_name = '{0}'
                                                       and bmf.device_type = 2
                                                       order by backup_finish_date desc";
                    foreach (var db in databases)
                    {                        
                        db.LastBackupName = con.Query<string>(string.Format(lastBackupSql, db.Name)).SingleOrDefault();
                    }
                    return databases;
                }
            }
            catch (SqlException e)
            {
                failure = e.Message;
                return Enumerable.Empty<SqlDatabase>().ToArray();
            }
        }

        public static string Backup(SqlDatabase database, BackgroundWorker backgroundWorker = null)
        {
            string result;
            var fileName = GetBackupFileName(database);
            _bgw = backgroundWorker;
            using (var con = (SqlConnection)OpenConnection())
            {
                con.InfoMessage += (sender, e) => Log(e.Message);
                BackupDatabase(fileName, database.Name, con);
                result = VerifyDatabaseBackup(fileName, database.Name, con);
            }
            return result;
        }

        static string GetBackupFileName(SqlDatabase database)
        {
            var backupDir = string.IsNullOrEmpty(database.LastBackupName)
                ? @"C:\temp\"
                : Path.GetDirectoryName(database.LastBackupName);
            var backupName = string.Format("{0}{1}", database.Name, DateTime.Now.ToString("yyyyMMddHHmm"));
            var fullBackupName = Path.Combine(backupDir, string.Format("{0}.bak", backupName));
            var copyCount = 0;
            while (File.Exists(fullBackupName))
            {
                fullBackupName = Path.Combine(backupDir, string.Format("{0}_Copy{1}.bak", backupName, ++copyCount));
            }
            return fullBackupName;
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
                           .FirstOrDefault();
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

        static void BackupDatabase(string file, string database, IDbConnection con)
        {
            Log("Preparing SQL Backup '{0}' to {1}...", database, file);
            const string sql = @"BACKUP DATABASE [{0}] TO  DISK = N'{1}' WITH  COPY_ONLY,  DESCRIPTION = N'Generated by DataButler {2}', FORMAT, INIT, SKIP, NOREWIND, NOUNLOAD, COMPRESSION,  STATS = 10, CHECKSUM";
            con.Execute(string.Format(sql, database, file, Application.ProductVersion), commandTimeout: 900);
            Log("Backup processed finished.");
        }

        static string VerifyDatabaseBackup(string file, string database, IDbConnection con)
        {
            Log("Verifying SQL Backup...", database, file);
            const string positionSql = @"select position from msdb..backupset where database_name=N'{0}' and backup_set_id=(select max(backup_set_id) from msdb..backupset where database_name=N'{0}')";
            var position = con.Query<int?>(string.Format(positionSql, database)).SingleOrDefault();
            if (position.HasValue == false)
            {
                Log("Verify failed. Backup information for database '{0}' not found.", database);
                return "<error>";
            }
            const string sql = @"RESTORE VERIFYONLY FROM  DISK = N'{0}' WITH  FILE = {1},  NOUNLOAD,  NOREWIND";
            con.Execute(string.Format(sql, file, position.Value), commandTimeout: 900);
            return position.ToString();
        }
    }
}
