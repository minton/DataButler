using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DataButler.Utilities;

namespace DataButler
{
    public partial class Backup : Form
    {
        readonly BackgroundWorker _backgroundWorker = new BackgroundWorker { WorkerReportsProgress = true };
        public string userBackupName;
        public Backup()
        {
            InitializeComponent();
            lnkVersion.Text = Application.ProductVersion;
            _backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
            _backgroundWorker.RunWorkerCompleted += BackgroundWorkerOnRunWorkerCompleted;
            _backgroundWorker.ProgressChanged += ProgressReported;
            btnBackup.Enabled = false;
            string failure;

            var databases = Database.GetDatabases(out failure);
            if (databases.Any())
            {
                cbDatabase.Items.AddRange(databases);
                return;
            }

            LogMessage(failure);
            txtLog.Visible = true;
        }

        void ProgressReported(object sender, ProgressChangedEventArgs e)
        {
            var userState = e.UserState.ToString();
            LogMessage(userState);
        }

        void LogMessage(string msg)
        {
            txtLog.AppendText(string.Format("{0}{1}", msg, Environment.NewLine));
            txtLog.ScrollToCaret();
        }

        private void BackupClicked(object sender, EventArgs e)
        {
            txtLog.BackColor = SystemColors.Window;
            txtLog.ForeColor = SystemColors.WindowText;
            txtLog.Visible = true;
            txtLog.Clear();
            Cursor = Cursors.WaitCursor;
            WaitImage.Visible = true;
            _backgroundWorker.RunWorkerAsync(cbDatabase.SelectedItem as SqlDatabase);
        }

        private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            userBackupName = txtBackupAs.Text;
            var database = e.Argument as SqlDatabase;
            e.Result = Database.Backup(database, userBackupName, _backgroundWorker);
        }

        private void BackgroundWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Cursor = Cursors.Default;
            var result = e.Result as string;
            var success = txtLog.Text.Contains("BACKUP DATABASE successfully") &&
                          txtLog.Text.Contains(string.Format("The backup set on file {0} is valid.", result));
            txtLog.BackColor = success ? Color.MediumSpringGreen : Color.DarkRed;
            txtLog.ForeColor = success ? Color.Black : Color.Yellow;
            var message = success ? "Backup SUCCESSFULLY COMPLETE." : "Backup FAILED!";
            LogMessage(message);
            btnBackup.Enabled = true;
        }

        private void VersionLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/minton/DataButler");
        }

        private void DatabaseSelected(object sender, EventArgs e)
        {
            btnBackup.Enabled = cbDatabase.SelectedItem != null;
        }

        private void txtBackupAs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnBackup.PerformClick();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }
    }
}
