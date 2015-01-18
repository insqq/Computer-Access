namespace ComputerAccess
{
    partial class myServer
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_allUsers = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button_loadFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button_killProccess = new System.Windows.Forms.Button();
            this.button_DirectoryUP = new System.Windows.Forms.Button();
            this.button_loadFolder = new System.Windows.Forms.Button();
            this.button_loadProccess = new System.Windows.Forms.Button();
            this.button_showDisplay = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_allUsers
            // 
            this.button_allUsers.Location = new System.Drawing.Point(621, 13);
            this.button_allUsers.Name = "button_allUsers";
            this.button_allUsers.Size = new System.Drawing.Size(75, 23);
            this.button_allUsers.TabIndex = 1;
            this.button_allUsers.Text = "all Users";
            this.button_allUsers.UseVisualStyleBackColor = true;
            this.button_allUsers.Click += new System.EventHandler(this.buttonAllUsers_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "path";
            // 
            // button_loadFile
            // 
            this.button_loadFile.Location = new System.Drawing.Point(540, 13);
            this.button_loadFile.Name = "button_loadFile";
            this.button_loadFile.Size = new System.Drawing.Size(75, 23);
            this.button_loadFile.TabIndex = 3;
            this.button_loadFile.Text = "Load File";
            this.button_loadFile.UseVisualStyleBackColor = true;
            this.button_loadFile.Click += new System.EventHandler(this.button_loadFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Computers List";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(150, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Directory";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(419, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Proccess";
            // 
            // button_killProccess
            // 
            this.button_killProccess.Location = new System.Drawing.Point(378, 13);
            this.button_killProccess.Name = "button_killProccess";
            this.button_killProccess.Size = new System.Drawing.Size(75, 23);
            this.button_killProccess.TabIndex = 7;
            this.button_killProccess.Text = "Kill Proccess";
            this.button_killProccess.UseVisualStyleBackColor = true;
            this.button_killProccess.Click += new System.EventHandler(this.button_killProccess_Click);
            // 
            // button_DirectoryUP
            // 
            this.button_DirectoryUP.Location = new System.Drawing.Point(12, 46);
            this.button_DirectoryUP.Name = "button_DirectoryUP";
            this.button_DirectoryUP.Size = new System.Drawing.Size(75, 23);
            this.button_DirectoryUP.TabIndex = 8;
            this.button_DirectoryUP.Text = "directoryUP";
            this.button_DirectoryUP.UseVisualStyleBackColor = true;
            this.button_DirectoryUP.Click += new System.EventHandler(this.button_DirectoryUP_Click);
            // 
            // button_loadFolder
            // 
            this.button_loadFolder.Location = new System.Drawing.Point(459, 13);
            this.button_loadFolder.Name = "button_loadFolder";
            this.button_loadFolder.Size = new System.Drawing.Size(75, 23);
            this.button_loadFolder.TabIndex = 9;
            this.button_loadFolder.Text = "Load Folder";
            this.button_loadFolder.UseVisualStyleBackColor = true;
            this.button_loadFolder.Click += new System.EventHandler(this.button_loadFolder_Click);
            // 
            // button_loadProccess
            // 
            this.button_loadProccess.Location = new System.Drawing.Point(286, 13);
            this.button_loadProccess.Name = "button_loadProccess";
            this.button_loadProccess.Size = new System.Drawing.Size(86, 23);
            this.button_loadProccess.TabIndex = 10;
            this.button_loadProccess.Text = "Load Proccess";
            this.button_loadProccess.UseVisualStyleBackColor = true;
            this.button_loadProccess.Click += new System.EventHandler(this.button_loadProccess_Click);
            // 
            // button_showDisplay
            // 
            this.button_showDisplay.Location = new System.Drawing.Point(192, 13);
            this.button_showDisplay.Name = "button_showDisplay";
            this.button_showDisplay.Size = new System.Drawing.Size(88, 23);
            this.button_showDisplay.TabIndex = 11;
            this.button_showDisplay.Text = "Show Screen";
            this.button_showDisplay.UseVisualStyleBackColor = true;
            this.button_showDisplay.Click += new System.EventHandler(this.button_showDisplay_Click);
            // 
            // myServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 393);
            this.Controls.Add(this.button_showDisplay);
            this.Controls.Add(this.button_loadProccess);
            this.Controls.Add(this.button_loadFolder);
            this.Controls.Add(this.button_DirectoryUP);
            this.Controls.Add(this.button_killProccess);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_loadFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_allUsers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "myServer";
            this.Text = "myServer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.myServer_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_allUsers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_loadFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_killProccess;
        private System.Windows.Forms.Button button_DirectoryUP;
        private System.Windows.Forms.Button button_loadFolder;
        private System.Windows.Forms.Button button_loadProccess;
        private System.Windows.Forms.Button button_showDisplay;
    }
}

