namespace DataButler
{
    partial class Backup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Backup));
            this.lnkVersion = new System.Windows.Forms.LinkLabel();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBackup = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.WaitImage = new System.Windows.Forms.PictureBox();
            this.cbDatabase = new System.Windows.Forms.ComboBox();
            this.txtBackupAs = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WaitImage)).BeginInit();
            this.SuspendLayout();
            // 
            // lnkVersion
            // 
            this.lnkVersion.AutoSize = true;
            this.lnkVersion.Location = new System.Drawing.Point(112, 126);
            this.lnkVersion.Name = "lnkVersion";
            this.lnkVersion.Size = new System.Drawing.Size(40, 13);
            this.lnkVersion.TabIndex = 11;
            this.lnkVersion.TabStop = true;
            this.lnkVersion.Text = "0.0.0.0";
            this.lnkVersion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.VersionLinkClicked);
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.SystemColors.Window;
            this.txtLog.Location = new System.Drawing.Point(163, 136);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(272, 56);
            this.txtLog.TabIndex = 10;
            this.txtLog.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(173, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Backup:";
            // 
            // btnBackup
            // 
            this.btnBackup.Location = new System.Drawing.Point(313, 97);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(113, 33);
            this.btnBackup.TabIndex = 8;
            this.btnBackup.Text = "Backup";
            this.btnBackup.UseVisualStyleBackColor = true;
            this.btnBackup.Click += new System.EventHandler(this.BackupClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DataButler.Properties.Resources.Loading;
            this.pictureBox1.Location = new System.Drawing.Point(432, 31);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // WaitImage
            // 
            this.WaitImage.Image = global::DataButler.Properties.Resources.DataButler;
            this.WaitImage.Location = new System.Drawing.Point(-30, -15);
            this.WaitImage.Name = "WaitImage";
            this.WaitImage.Size = new System.Drawing.Size(230, 228);
            this.WaitImage.TabIndex = 7;
            this.WaitImage.TabStop = false;
            // 
            // cbDatabase
            // 
            this.cbDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDatabase.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F);
            this.cbDatabase.FormattingEnabled = true;
            this.cbDatabase.Location = new System.Drawing.Point(176, 25);
            this.cbDatabase.Name = "cbDatabase";
            this.cbDatabase.Size = new System.Drawing.Size(250, 28);
            this.cbDatabase.TabIndex = 12;
            this.cbDatabase.SelectionChangeCommitted += new System.EventHandler(this.DatabaseSelected);
            // 
            // txtBackupAs
            // 
            this.txtBackupAs.Location = new System.Drawing.Point(176, 71);
            this.txtBackupAs.Name = "txtBackupAs";
            this.txtBackupAs.Size = new System.Drawing.Size(250, 20);
            this.txtBackupAs.TabIndex = 13;
            this.txtBackupAs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBackupAs_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(173, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Backup As:";
            // 
            // Backup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 199);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBackupAs);
            this.Controls.Add(this.cbDatabase);
            this.Controls.Add(this.lnkVersion);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBackup);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.WaitImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Backup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DataButler - Backup";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WaitImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkVersion;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox WaitImage;
        private System.Windows.Forms.ComboBox cbDatabase;
        private System.Windows.Forms.TextBox txtBackupAs;
        private System.Windows.Forms.Label label2;

    }
}