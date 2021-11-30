namespace MDRConfigTool
{
    partial class FileSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileSelector));
            this.tbFilePath = new System.Windows.Forms.TextBox();
            this.btnFolderBrowse = new System.Windows.Forms.Button();
            this.btnOPenFile = new System.Windows.Forms.Button();
            this.pnlFileSelect = new System.Windows.Forms.Panel();
            this.btnBrowseFile = new System.Windows.Forms.Button();
            this.tbFileName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbProjectName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.lbFileSelect = new System.Windows.Forms.Label();
            this.pnlDataDisplay = new System.Windows.Forms.Panel();
            this.lbReadData = new System.Windows.Forms.Label();
            this.btnTableOK = new System.Windows.Forms.Button();
            this.dgvListDisplay = new System.Windows.Forms.DataGridView();
            this.pnlSolutionsettings = new System.Windows.Forms.Panel();
            this.tbSolExplain = new System.Windows.Forms.TextBox();
            this.btnOpenSolution = new System.Windows.Forms.Button();
            this.gbAmsSettings = new System.Windows.Forms.GroupBox();
            this.cbRoutes = new System.Windows.Forms.ComboBox();
            this.lbAmsNetId = new System.Windows.Forms.Label();
            this.pnlWelcome = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lbWelcome = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.pnlFinish = new System.Windows.Forms.Panel();
            this.btnActivate = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnFinish = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlFileSelect.SuspendLayout();
            this.pnlDataDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListDisplay)).BeginInit();
            this.pnlSolutionsettings.SuspendLayout();
            this.gbAmsSettings.SuspendLayout();
            this.pnlWelcome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlFinish.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbFilePath
            // 
            this.tbFilePath.Location = new System.Drawing.Point(88, 105);
            this.tbFilePath.Name = "tbFilePath";
            this.tbFilePath.Size = new System.Drawing.Size(387, 20);
            this.tbFilePath.TabIndex = 0;
            this.tbFilePath.Text = "C:\\";
            // 
            // btnFolderBrowse
            // 
            this.btnFolderBrowse.Location = new System.Drawing.Point(400, 131);
            this.btnFolderBrowse.Name = "btnFolderBrowse";
            this.btnFolderBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnFolderBrowse.TabIndex = 1;
            this.btnFolderBrowse.Text = "Browse...";
            this.btnFolderBrowse.UseVisualStyleBackColor = true;
            this.btnFolderBrowse.Click += new System.EventHandler(this.FolderBrowse_Click);
            // 
            // btnOPenFile
            // 
            this.btnOPenFile.Location = new System.Drawing.Point(524, 306);
            this.btnOPenFile.Name = "btnOPenFile";
            this.btnOPenFile.Size = new System.Drawing.Size(75, 23);
            this.btnOPenFile.TabIndex = 2;
            this.btnOPenFile.Text = "Open";
            this.btnOPenFile.UseVisualStyleBackColor = true;
            this.btnOPenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // pnlFileSelect
            // 
            this.pnlFileSelect.Controls.Add(this.btnBrowseFile);
            this.pnlFileSelect.Controls.Add(this.tbFileName);
            this.pnlFileSelect.Controls.Add(this.label2);
            this.pnlFileSelect.Controls.Add(this.tbProjectName);
            this.pnlFileSelect.Controls.Add(this.label1);
            this.pnlFileSelect.Controls.Add(this.richTextBox1);
            this.pnlFileSelect.Controls.Add(this.lbFileSelect);
            this.pnlFileSelect.Controls.Add(this.tbFilePath);
            this.pnlFileSelect.Controls.Add(this.btnOPenFile);
            this.pnlFileSelect.Controls.Add(this.btnFolderBrowse);
            this.pnlFileSelect.Location = new System.Drawing.Point(12, 12);
            this.pnlFileSelect.Name = "pnlFileSelect";
            this.pnlFileSelect.Size = new System.Drawing.Size(603, 385);
            this.pnlFileSelect.TabIndex = 3;
            this.pnlFileSelect.Visible = false;
            // 
            // btnBrowseFile
            // 
            this.btnBrowseFile.Location = new System.Drawing.Point(88, 240);
            this.btnBrowseFile.Name = "btnBrowseFile";
            this.btnBrowseFile.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseFile.TabIndex = 10;
            this.btnBrowseFile.Text = "Browse...";
            this.btnBrowseFile.UseVisualStyleBackColor = true;
            this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
            // 
            // tbFileName
            // 
            this.tbFileName.Location = new System.Drawing.Point(88, 213);
            this.tbFileName.Name = "tbFileName";
            this.tbFileName.Size = new System.Drawing.Size(125, 20);
            this.tbFileName.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(91, 196);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Drive List File Name:";
            // 
            // tbProjectName
            // 
            this.tbProjectName.Location = new System.Drawing.Point(88, 173);
            this.tbProjectName.Name = "tbProjectName";
            this.tbProjectName.Size = new System.Drawing.Size(125, 20);
            this.tbProjectName.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(91, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Choose a Project Name:";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(200, 306);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(318, 32);
            this.richTextBox1.TabIndex = 5;
            this.richTextBox1.Text = "NOTE: Make sure your Excel spreadsheet, drive configuration file, and MDR_Control" +
    " library are in the directory you selected.";
            // 
            // lbFileSelect
            // 
            this.lbFileSelect.AutoSize = true;
            this.lbFileSelect.Location = new System.Drawing.Point(88, 86);
            this.lbFileSelect.Name = "lbFileSelect";
            this.lbFileSelect.Size = new System.Drawing.Size(343, 13);
            this.lbFileSelect.TabIndex = 3;
            this.lbFileSelect.Text = "Please choose the directory where you would like to save your solution:";
            // 
            // pnlDataDisplay
            // 
            this.pnlDataDisplay.Controls.Add(this.lbReadData);
            this.pnlDataDisplay.Controls.Add(this.btnTableOK);
            this.pnlDataDisplay.Controls.Add(this.dgvListDisplay);
            this.pnlDataDisplay.Location = new System.Drawing.Point(12, 15);
            this.pnlDataDisplay.Name = "pnlDataDisplay";
            this.pnlDataDisplay.Size = new System.Drawing.Size(603, 382);
            this.pnlDataDisplay.TabIndex = 4;
            this.pnlDataDisplay.Visible = false;
            // 
            // lbReadData
            // 
            this.lbReadData.AutoSize = true;
            this.lbReadData.Location = new System.Drawing.Point(5, 4);
            this.lbReadData.Name = "lbReadData";
            this.lbReadData.Size = new System.Drawing.Size(563, 13);
            this.lbReadData.TabIndex = 2;
            this.lbReadData.Text = "The below table shows the information read from you spreadhseet. Please verify th" +
    "at this is correct before clicking next";
            // 
            // btnTableOK
            // 
            this.btnTableOK.Location = new System.Drawing.Point(524, 352);
            this.btnTableOK.Name = "btnTableOK";
            this.btnTableOK.Size = new System.Drawing.Size(75, 23);
            this.btnTableOK.TabIndex = 1;
            this.btnTableOK.Text = "Next";
            this.btnTableOK.UseVisualStyleBackColor = true;
            this.btnTableOK.Click += new System.EventHandler(this.btnTableOK_Click);
            // 
            // dgvListDisplay
            // 
            this.dgvListDisplay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListDisplay.Location = new System.Drawing.Point(3, 23);
            this.dgvListDisplay.Name = "dgvListDisplay";
            this.dgvListDisplay.ReadOnly = true;
            this.dgvListDisplay.Size = new System.Drawing.Size(597, 327);
            this.dgvListDisplay.TabIndex = 0;
            // 
            // pnlSolutionsettings
            // 
            this.pnlSolutionsettings.Controls.Add(this.tbSolExplain);
            this.pnlSolutionsettings.Controls.Add(this.btnOpenSolution);
            this.pnlSolutionsettings.Controls.Add(this.gbAmsSettings);
            this.pnlSolutionsettings.Location = new System.Drawing.Point(12, 12);
            this.pnlSolutionsettings.Name = "pnlSolutionsettings";
            this.pnlSolutionsettings.Size = new System.Drawing.Size(603, 385);
            this.pnlSolutionsettings.TabIndex = 5;
            this.pnlSolutionsettings.Visible = false;
            // 
            // tbSolExplain
            // 
            this.tbSolExplain.Location = new System.Drawing.Point(87, 27);
            this.tbSolExplain.Multiline = true;
            this.tbSolExplain.Name = "tbSolExplain";
            this.tbSolExplain.ReadOnly = true;
            this.tbSolExplain.Size = new System.Drawing.Size(388, 74);
            this.tbSolExplain.TabIndex = 5;
            this.tbSolExplain.Text = "We will now start scanning in your hardware, editing your drive\'s start-up parame" +
    "ters, and then creating the necessary function blocks and variables in the PLC.\r" +
    "\n\r\nPlease provide the target AMSNetID.";
            // 
            // btnOpenSolution
            // 
            this.btnOpenSolution.Location = new System.Drawing.Point(500, 353);
            this.btnOpenSolution.Name = "btnOpenSolution";
            this.btnOpenSolution.Size = new System.Drawing.Size(99, 23);
            this.btnOpenSolution.TabIndex = 3;
            this.btnOpenSolution.Text = "Next";
            this.btnOpenSolution.UseVisualStyleBackColor = true;
            this.btnOpenSolution.Click += new System.EventHandler(this.btnOpenSolution_Click);
            // 
            // gbAmsSettings
            // 
            this.gbAmsSettings.Controls.Add(this.cbRoutes);
            this.gbAmsSettings.Controls.Add(this.lbAmsNetId);
            this.gbAmsSettings.Location = new System.Drawing.Point(116, 131);
            this.gbAmsSettings.Name = "gbAmsSettings";
            this.gbAmsSettings.Size = new System.Drawing.Size(343, 67);
            this.gbAmsSettings.TabIndex = 2;
            this.gbAmsSettings.TabStop = false;
            // 
            // cbRoutes
            // 
            this.cbRoutes.FormattingEnabled = true;
            this.cbRoutes.Location = new System.Drawing.Point(165, 26);
            this.cbRoutes.Name = "cbRoutes";
            this.cbRoutes.Size = new System.Drawing.Size(172, 21);
            this.cbRoutes.TabIndex = 2;
            // 
            // lbAmsNetId
            // 
            this.lbAmsNetId.AutoSize = true;
            this.lbAmsNetId.Location = new System.Drawing.Point(63, 26);
            this.lbAmsNetId.Name = "lbAmsNetId";
            this.lbAmsNetId.Size = new System.Drawing.Size(96, 13);
            this.lbAmsNetId.TabIndex = 1;
            this.lbAmsNetId.Text = "Target Ams Net Id:";
            // 
            // pnlWelcome
            // 
            this.pnlWelcome.Controls.Add(this.textBox1);
            this.pnlWelcome.Controls.Add(this.lbWelcome);
            this.pnlWelcome.Controls.Add(this.pictureBox1);
            this.pnlWelcome.Controls.Add(this.btnStart);
            this.pnlWelcome.Location = new System.Drawing.Point(12, 12);
            this.pnlWelcome.Name = "pnlWelcome";
            this.pnlWelcome.Size = new System.Drawing.Size(603, 385);
            this.pnlWelcome.TabIndex = 6;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(19, 321);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(573, 32);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // lbWelcome
            // 
            this.lbWelcome.AutoSize = true;
            this.lbWelcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbWelcome.Location = new System.Drawing.Point(34, 1);
            this.lbWelcome.Name = "lbWelcome";
            this.lbWelcome.Size = new System.Drawing.Size(508, 29);
            this.lbWelcome.TabIndex = 2;
            this.lbWelcome.Text = "Welcome to the MDR Drive configuration Tool!";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(600, 326);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(518, 353);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Next";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // pnlFinish
            // 
            this.pnlFinish.Controls.Add(this.btnActivate);
            this.pnlFinish.Controls.Add(this.textBox2);
            this.pnlFinish.Controls.Add(this.btnFinish);
            this.pnlFinish.Location = new System.Drawing.Point(12, 12);
            this.pnlFinish.Name = "pnlFinish";
            this.pnlFinish.Size = new System.Drawing.Size(603, 385);
            this.pnlFinish.TabIndex = 8;
            this.pnlFinish.Visible = false;
            // 
            // btnActivate
            // 
            this.btnActivate.Location = new System.Drawing.Point(200, 313);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(181, 72);
            this.btnActivate.TabIndex = 3;
            this.btnActivate.Text = "Activate Configuration and Close";
            this.btnActivate.UseVisualStyleBackColor = true;
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(19, 128);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(561, 105);
            this.textBox2.TabIndex = 2;
            this.textBox2.Text = resources.GetString("textBox2.Text");
            // 
            // btnFinish
            // 
            this.btnFinish.Location = new System.Drawing.Point(399, 314);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(204, 71);
            this.btnFinish.TabIndex = 1;
            this.btnFinish.Text = "Close Utility without Activating";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 401);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(627, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "...";
            // 
            // tsStatus
            // 
            this.tsStatus.Name = "tsStatus";
            this.tsStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // FileSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 423);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlSolutionsettings);
            this.Controls.Add(this.pnlFileSelect);
            this.Controls.Add(this.pnlDataDisplay);
            this.Controls.Add(this.pnlFinish);
            this.Controls.Add(this.pnlWelcome);
            this.Name = "FileSelector";
            this.Text = "MDR Drive Configuration Tool";
            this.pnlFileSelect.ResumeLayout(false);
            this.pnlFileSelect.PerformLayout();
            this.pnlDataDisplay.ResumeLayout(false);
            this.pnlDataDisplay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListDisplay)).EndInit();
            this.pnlSolutionsettings.ResumeLayout(false);
            this.pnlSolutionsettings.PerformLayout();
            this.gbAmsSettings.ResumeLayout(false);
            this.gbAmsSettings.PerformLayout();
            this.pnlWelcome.ResumeLayout(false);
            this.pnlWelcome.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlFinish.ResumeLayout(false);
            this.pnlFinish.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbFilePath;
        private System.Windows.Forms.Button btnFolderBrowse;
        private System.Windows.Forms.Button btnOPenFile;
        private System.Windows.Forms.Panel pnlFileSelect;
        private System.Windows.Forms.Panel pnlDataDisplay;
        private System.Windows.Forms.DataGridView dgvListDisplay;
        private System.Windows.Forms.Panel pnlSolutionsettings;
        private System.Windows.Forms.Button btnTableOK;
        private System.Windows.Forms.Button btnOpenSolution;
        private System.Windows.Forms.GroupBox gbAmsSettings;
        private System.Windows.Forms.Label lbAmsNetId;
        private System.Windows.Forms.Panel pnlWelcome;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lbFileSelect;
        private System.Windows.Forms.Label lbReadData;
        private System.Windows.Forms.Label lbWelcome;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbProjectName;
        private System.Windows.Forms.TextBox tbFileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowseFile;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox tbSolExplain;
        private System.Windows.Forms.Panel pnlFinish;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsStatus;
        private System.Windows.Forms.Button btnActivate;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ComboBox cbRoutes;
    }

     
}

