using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DataButler.Utilities;

namespace DataButler
{
    public partial class Restore : Form
    {
        readonly string _backupPath;
        readonly BackgroundWorker _backgroundWorker = new BackgroundWorker { WorkerReportsProgress = true };

        public Restore(string backupPath)
        {
            _backupPath = backupPath;
            InitializeComponent();
            lnkVersion.Text = Application.ProductVersion;
            Text += string.Format(" - [{0}]", _backupPath);
            _backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
            _backgroundWorker.RunWorkerCompleted += BackgroundWorkerOnRunWorkerCompleted;
            _backgroundWorker.ProgressChanged += ProgressReported;
            string failure;
            var databaseName = Database.GetDatabaseName(_backupPath, out failure);
            if (string.IsNullOrEmpty(databaseName))
            {
                LogMessage(failure);
                btnRestore.Enabled = false;
                txtLog.Visible = true;
            }
            txtName.Text = databaseName ?? "Invalid file.";
            txtName.SelectAll();
            txtName.Focus();
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

        private void RestoreClicked(object sender, EventArgs e)
        {
            txtLog.BackColor = SystemColors.Window;
            txtLog.ForeColor = SystemColors.WindowText;
            txtLog.Visible = true;
            Cursor = Cursors.WaitCursor;
            btnRestore.Enabled =
                txtName.Enabled = false;
            WaitImage.Visible = true;
            _backgroundWorker.RunWorkerAsync(txtName.Text);
        }

        private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var restoreAs = doWorkEventArgs.Argument.ToString();
            Database.RestoreDatabase(_backupPath, restoreAs, _backgroundWorker);
        }

        private void BackgroundWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            Cursor = Cursors.Default;
            var success = txtLog.Text.Contains("RESTORE DATABASE successfully processed");
            txtLog.BackColor = success ? Color.MediumSpringGreen : Color.DarkRed;
            txtLog.ForeColor = success ? Color.Black : Color.Yellow;
            var message = success ? "Restore SUCCESSFULLY COMPLETE." : "Restore FAILED!";
            LogMessage(message);
            btnRestore.Enabled = true;
        }

        private void VersionLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/minton/DataButler");
        }
    }
}
