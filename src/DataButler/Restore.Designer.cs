namespace DataButler
{
    partial class Restore
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Restore));
            this.btnRestore = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lnkVersion = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.WaitImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WaitImage)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRestore
            // 
            this.btnRestore.Location = new System.Drawing.Point(302, 58);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(113, 33);
            this.btnRestore.TabIndex = 1;
            this.btnRestore.Text = "Restore";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.RestoreClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(160, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Restore As:";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("MS Reference Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(162, 25);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(253, 27);
            this.txtName.TabIndex = 0;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.SystemColors.Window;
            this.txtLog.Location = new System.Drawing.Point(163, 96);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(272, 96);
            this.txtLog.TabIndex = 3;
            this.txtLog.Visible = false;
            // 
            // lnkVersion
            // 
            this.lnkVersion.AutoSize = true;
            this.lnkVersion.Location = new System.Drawing.Point(112, 126);
            this.lnkVersion.Name = "lnkVersion";
            this.lnkVersion.Size = new System.Drawing.Size(40, 13);
            this.lnkVersion.TabIndex = 4;
            this.lnkVersion.TabStop = true;
            this.lnkVersion.Text = "0.0.0.0";
            this.lnkVersion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.VersionLinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DataButler.Properties.Resources.Loading;
            this.pictureBox1.Location = new System.Drawing.Point(419, 31);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // WaitImage
            // 
            this.WaitImage.Image = global::DataButler.Properties.Resources.DataButler;
            this.WaitImage.Location = new System.Drawing.Point(-30, -15);
            this.WaitImage.Name = "WaitImage";
            this.WaitImage.Size = new System.Drawing.Size(230, 228);
            this.WaitImage.TabIndex = 0;
            this.WaitImage.TabStop = false;
            // 
            // Restore
            // 
            this.AcceptButton = this.btnRestore;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 199);
            this.Controls.Add(this.lnkVersion);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.WaitImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Restore";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DataButler";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WaitImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.PictureBox WaitImage;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.LinkLabel lnkVersion;
    }
}

