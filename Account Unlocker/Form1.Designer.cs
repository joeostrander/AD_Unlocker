namespace Account_Unlocker
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.labelDomain = new System.Windows.Forms.Label();
            this.comboBoxDomain = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.comboBoxSite = new System.Windows.Forms.ComboBox();
            this.labelUserID = new System.Windows.Forms.Label();
            this.textBoxUserIDSearch = new System.Windows.Forms.TextBox();
            this.buttonGo = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeaderServerFullName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderServer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderUserId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSite = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderUserState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBadPwdCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLastBadPwd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPwdLastSet = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLockoutTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderOrigLock = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPasswordAge = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStripListView1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.unlockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.passwordResetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripForm = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonFindUser = new System.Windows.Forms.Button();
            this.contextMenuStripListView1.SuspendLayout();
            this.contextMenuStripForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelDomain
            // 
            this.labelDomain.AutoSize = true;
            this.labelDomain.Location = new System.Drawing.Point(12, 9);
            this.labelDomain.Name = "labelDomain";
            this.labelDomain.Size = new System.Drawing.Size(46, 13);
            this.labelDomain.TabIndex = 0;
            this.labelDomain.Text = "&Domain:";
            // 
            // comboBoxDomain
            // 
            this.comboBoxDomain.FormattingEnabled = true;
            this.comboBoxDomain.Location = new System.Drawing.Point(12, 25);
            this.comboBoxDomain.Name = "comboBoxDomain";
            this.comboBoxDomain.Size = new System.Drawing.Size(121, 21);
            this.comboBoxDomain.Sorted = true;
            this.comboBoxDomain.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(172, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "&Login ID (optional):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(334, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "&Password (optional):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(496, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "S&ite:";
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(175, 26);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(124, 20);
            this.textBoxUsername.TabIndex = 7;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(337, 26);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(124, 20);
            this.textBoxPassword.TabIndex = 8;
            // 
            // comboBoxSite
            // 
            this.comboBoxSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSite.FormattingEnabled = true;
            this.comboBoxSite.Items.AddRange(new object[] {
            "Get sites..."});
            this.comboBoxSite.Location = new System.Drawing.Point(499, 26);
            this.comboBoxSite.Name = "comboBoxSite";
            this.comboBoxSite.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSite.TabIndex = 9;
            this.comboBoxSite.SelectedIndexChanged += new System.EventHandler(this.comboBoxGetSites_SelectedIndexChanged);
            // 
            // labelUserID
            // 
            this.labelUserID.AutoSize = true;
            this.labelUserID.Location = new System.Drawing.Point(12, 67);
            this.labelUserID.Name = "labelUserID";
            this.labelUserID.Size = new System.Drawing.Size(46, 13);
            this.labelUserID.TabIndex = 10;
            this.labelUserID.Text = "&User ID:";
            // 
            // textBoxUserIDSearch
            // 
            this.textBoxUserIDSearch.Location = new System.Drawing.Point(12, 83);
            this.textBoxUserIDSearch.Name = "textBoxUserIDSearch";
            this.textBoxUserIDSearch.Size = new System.Drawing.Size(124, 20);
            this.textBoxUserIDSearch.TabIndex = 11;
            // 
            // buttonGo
            // 
            this.buttonGo.Location = new System.Drawing.Point(142, 81);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(75, 23);
            this.buttonGo.TabIndex = 12;
            this.buttonGo.Text = "&Status";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderServerFullName,
            this.columnHeaderServer,
            this.columnHeaderUserId,
            this.columnHeaderSite,
            this.columnHeaderUserState,
            this.columnHeaderBadPwdCount,
            this.columnHeaderLastBadPwd,
            this.columnHeaderPwdLastSet,
            this.columnHeaderLockoutTime,
            this.columnHeaderOrigLock,
            this.columnHeaderPasswordAge});
            this.listView1.ContextMenuStrip = this.contextMenuStripListView1;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(12, 136);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(907, 374);
            this.listView1.TabIndex = 13;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderServerFullName
            // 
            this.columnHeaderServerFullName.Text = "Server Full Name";
            this.columnHeaderServerFullName.Width = 0;
            // 
            // columnHeaderServer
            // 
            this.columnHeaderServer.Text = "Server";
            // 
            // columnHeaderUserId
            // 
            this.columnHeaderUserId.Text = "User ID";
            // 
            // columnHeaderSite
            // 
            this.columnHeaderSite.Text = "Site";
            // 
            // columnHeaderUserState
            // 
            this.columnHeaderUserState.Text = "User State";
            this.columnHeaderUserState.Width = 87;
            // 
            // columnHeaderBadPwdCount
            // 
            this.columnHeaderBadPwdCount.Text = "Bad Pwd Count";
            this.columnHeaderBadPwdCount.Width = 100;
            // 
            // columnHeaderLastBadPwd
            // 
            this.columnHeaderLastBadPwd.Text = "Last Bad Pwd";
            this.columnHeaderLastBadPwd.Width = 140;
            // 
            // columnHeaderPwdLastSet
            // 
            this.columnHeaderPwdLastSet.Text = "Pwd Last Set";
            this.columnHeaderPwdLastSet.Width = 140;
            // 
            // columnHeaderLockoutTime
            // 
            this.columnHeaderLockoutTime.Text = "Lockout Time";
            this.columnHeaderLockoutTime.Width = 140;
            // 
            // columnHeaderOrigLock
            // 
            this.columnHeaderOrigLock.Text = "Orig Lock";
            this.columnHeaderOrigLock.Width = 80;
            // 
            // columnHeaderPasswordAge
            // 
            this.columnHeaderPasswordAge.Text = "Password Age";
            this.columnHeaderPasswordAge.Width = 100;
            // 
            // contextMenuStripListView1
            // 
            this.contextMenuStripListView1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.unlockToolStripMenuItem,
            this.passwordResetToolStripMenuItem});
            this.contextMenuStripListView1.Name = "contextMenuStripListView1";
            this.contextMenuStripListView1.ShowImageMargin = false;
            this.contextMenuStripListView1.Size = new System.Drawing.Size(131, 48);
            this.contextMenuStripListView1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // unlockToolStripMenuItem
            // 
            this.unlockToolStripMenuItem.Name = "unlockToolStripMenuItem";
            this.unlockToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.unlockToolStripMenuItem.Text = "&Unlock";
            this.unlockToolStripMenuItem.Click += new System.EventHandler(this.unlockToolStripMenuItem_Click);
            // 
            // passwordResetToolStripMenuItem
            // 
            this.passwordResetToolStripMenuItem.Name = "passwordResetToolStripMenuItem";
            this.passwordResetToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.passwordResetToolStripMenuItem.Text = "&Password Reset";
            this.passwordResetToolStripMenuItem.Click += new System.EventHandler(this.passwordResetToolStripMenuItem_Click);
            // 
            // contextMenuStripForm
            // 
            this.contextMenuStripForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.contextMenuStripForm.Name = "contextMenuStripForm";
            this.contextMenuStripForm.ShowImageMargin = false;
            this.contextMenuStripForm.Size = new System.Drawing.Size(83, 26);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(82, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // buttonFindUser
            // 
            this.buttonFindUser.Location = new System.Drawing.Point(268, 81);
            this.buttonFindUser.Name = "buttonFindUser";
            this.buttonFindUser.Size = new System.Drawing.Size(75, 23);
            this.buttonFindUser.TabIndex = 14;
            this.buttonFindUser.Text = "&Find User ID";
            this.buttonFindUser.UseVisualStyleBackColor = true;
            this.buttonFindUser.Click += new System.EventHandler(this.buttonFindUser_Click);
            // 
            // Form1
            // 
            this.AcceptButton = this.buttonGo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 522);
            this.ContextMenuStrip = this.contextMenuStripForm;
            this.Controls.Add(this.buttonFindUser);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.textBoxUserIDSearch);
            this.Controls.Add(this.labelUserID);
            this.Controls.Add(this.comboBoxSite);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxUsername);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxDomain);
            this.Controls.Add(this.labelDomain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(760, 500);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStripListView1.ResumeLayout(false);
            this.contextMenuStripForm.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDomain;
        private System.Windows.Forms.ComboBox comboBoxDomain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.ComboBox comboBoxSite;
        private System.Windows.Forms.Label labelUserID;
        private System.Windows.Forms.TextBox textBoxUserIDSearch;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeaderServer;
        private System.Windows.Forms.ColumnHeader columnHeaderSite;
        private System.Windows.Forms.ColumnHeader columnHeaderUserState;
        private System.Windows.Forms.ColumnHeader columnHeaderBadPwdCount;
        private System.Windows.Forms.ColumnHeader columnHeaderLastBadPwd;
        private System.Windows.Forms.ColumnHeader columnHeaderPwdLastSet;
        private System.Windows.Forms.ColumnHeader columnHeaderLockoutTime;
        private System.Windows.Forms.ColumnHeader columnHeaderOrigLock;
        private System.Windows.Forms.ColumnHeader columnHeaderServerFullName;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripListView1;
        private System.Windows.Forms.ToolStripMenuItem unlockToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripForm;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem passwordResetToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeaderUserId;
        private System.Windows.Forms.ColumnHeader columnHeaderPasswordAge;
        private System.Windows.Forms.Button buttonFindUser;
    }
}

