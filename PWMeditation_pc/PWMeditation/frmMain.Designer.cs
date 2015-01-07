namespace PWMeditation
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.bSettings = new System.Windows.Forms.Button();
            this.cbAccounts = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bConnect = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tmUpdate = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.labStatus = new System.Windows.Forms.Label();
            this.pbExp = new System.Windows.Forms.ProgressBar();
            this.labExp = new System.Windows.Forms.Label();
            this.labLevel = new System.Windows.Forms.Label();
            this.nIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.labMeditationTime = new System.Windows.Forms.Label();
            this.labAllMeditationTime = new System.Windows.Forms.Label();
            this.labDeepMeditationTime = new System.Windows.Forms.Label();
            this.lblEstimatedExp = new System.Windows.Forms.Label();
            this.rtbChat = new System.Windows.Forms.RichTextBox();
            this.bShowInfo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bSettings
            // 
            this.bSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bSettings.Location = new System.Drawing.Point(12, 207);
            this.bSettings.Name = "bSettings";
            this.bSettings.Size = new System.Drawing.Size(193, 62);
            this.bSettings.TabIndex = 0;
            this.bSettings.Text = "Настройки";
            this.bSettings.UseVisualStyleBackColor = true;
            this.bSettings.Click += new System.EventHandler(this.bSettings_Click);
            // 
            // cbAccounts
            // 
            this.cbAccounts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAccounts.FormattingEnabled = true;
            this.cbAccounts.Location = new System.Drawing.Point(12, 35);
            this.cbAccounts.Name = "cbAccounts";
            this.cbAccounts.Size = new System.Drawing.Size(193, 21);
            this.cbAccounts.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Персонаж:";
            // 
            // bConnect
            // 
            this.bConnect.Location = new System.Drawing.Point(12, 62);
            this.bConnect.Name = "bConnect";
            this.bConnect.Size = new System.Drawing.Size(193, 45);
            this.bConnect.TabIndex = 3;
            this.bConnect.Text = "Подключиться";
            this.bConnect.UseVisualStyleBackColor = true;
            this.bConnect.Click += new System.EventHandler(this.bConnect_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(32, 32);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tmUpdate
            // 
            this.tmUpdate.Enabled = true;
            this.tmUpdate.Interval = 1000;
            this.tmUpdate.Tick += new System.EventHandler(this.tmUpdate_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Статус: ";
            // 
            // labStatus
            // 
            this.labStatus.AutoSize = true;
            this.labStatus.Location = new System.Drawing.Point(56, 110);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(0, 13);
            this.labStatus.TabIndex = 5;
            // 
            // pbExp
            // 
            this.pbExp.Location = new System.Drawing.Point(12, 126);
            this.pbExp.Name = "pbExp";
            this.pbExp.Size = new System.Drawing.Size(193, 23);
            this.pbExp.TabIndex = 6;
            // 
            // labExp
            // 
            this.labExp.AutoSize = true;
            this.labExp.Location = new System.Drawing.Point(219, 48);
            this.labExp.Name = "labExp";
            this.labExp.Size = new System.Drawing.Size(39, 13);
            this.labExp.TabIndex = 7;
            this.labExp.Text = "Опыт:";
            // 
            // labLevel
            // 
            this.labLevel.AutoSize = true;
            this.labLevel.Location = new System.Drawing.Point(219, 35);
            this.labLevel.Name = "labLevel";
            this.labLevel.Size = new System.Drawing.Size(30, 13);
            this.labLevel.TabIndex = 8;
            this.labLevel.Text = "Лвл:";
            // 
            // nIcon
            // 
            this.nIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("nIcon.Icon")));
            this.nIcon.Text = "PWMeditation";
            this.nIcon.Visible = true;
            this.nIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.nIcon_MouseDoubleClick);
            // 
            // labMeditationTime
            // 
            this.labMeditationTime.AutoSize = true;
            this.labMeditationTime.Location = new System.Drawing.Point(219, 78);
            this.labMeditationTime.Name = "labMeditationTime";
            this.labMeditationTime.Size = new System.Drawing.Size(68, 13);
            this.labMeditationTime.TabIndex = 9;
            this.labMeditationTime.Text = "Медитация:";
            // 
            // labAllMeditationTime
            // 
            this.labAllMeditationTime.AutoSize = true;
            this.labAllMeditationTime.Location = new System.Drawing.Point(219, 104);
            this.labAllMeditationTime.Name = "labAllMeditationTime";
            this.labAllMeditationTime.Size = new System.Drawing.Size(54, 13);
            this.labAllMeditationTime.TabIndex = 10;
            this.labAllMeditationTime.Text = "Сегодня:";
            // 
            // labDeepMeditationTime
            // 
            this.labDeepMeditationTime.AutoSize = true;
            this.labDeepMeditationTime.Location = new System.Drawing.Point(219, 91);
            this.labDeepMeditationTime.Name = "labDeepMeditationTime";
            this.labDeepMeditationTime.Size = new System.Drawing.Size(85, 13);
            this.labDeepMeditationTime.TabIndex = 11;
            this.labDeepMeditationTime.Text = "Гл. медитация:";
            // 
            // lblEstimatedExp
            // 
            this.lblEstimatedExp.AutoSize = true;
            this.lblEstimatedExp.Location = new System.Drawing.Point(219, 62);
            this.lblEstimatedExp.Name = "lblEstimatedExp";
            this.lblEstimatedExp.Size = new System.Drawing.Size(41, 13);
            this.lblEstimatedExp.TabIndex = 12;
            this.lblEstimatedExp.Text = "Будет:";
            // 
            // rtbChat
            // 
            this.rtbChat.BackColor = System.Drawing.Color.Black;
            this.rtbChat.ForeColor = System.Drawing.Color.White;
            this.rtbChat.Location = new System.Drawing.Point(219, 126);
            this.rtbChat.Name = "rtbChat";
            this.rtbChat.ReadOnly = true;
            this.rtbChat.Size = new System.Drawing.Size(193, 143);
            this.rtbChat.TabIndex = 14;
            this.rtbChat.Text = "";
            // 
            // bShowInfo
            // 
            this.bShowInfo.Location = new System.Drawing.Point(12, 181);
            this.bShowInfo.Name = "bShowInfo";
            this.bShowInfo.Size = new System.Drawing.Size(39, 20);
            this.bShowInfo.TabIndex = 15;
            this.bShowInfo.Text = "Info";
            this.bShowInfo.UseVisualStyleBackColor = true;
            this.bShowInfo.Click += new System.EventHandler(this.bShowInfo_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 281);
            this.Controls.Add(this.bShowInfo);
            this.Controls.Add(this.rtbChat);
            this.Controls.Add(this.lblEstimatedExp);
            this.Controls.Add(this.labDeepMeditationTime);
            this.Controls.Add(this.labAllMeditationTime);
            this.Controls.Add(this.labMeditationTime);
            this.Controls.Add(this.labLevel);
            this.Controls.Add(this.labExp);
            this.Controls.Add(this.pbExp);
            this.Controls.Add(this.labStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bConnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbAccounts);
            this.Controls.Add(this.bSettings);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Meditation";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bSettings;
        private System.Windows.Forms.ComboBox cbAccounts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bConnect;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer tmUpdate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labStatus;
        private System.Windows.Forms.ProgressBar pbExp;
        private System.Windows.Forms.Label labExp;
        private System.Windows.Forms.Label labLevel;
        private System.Windows.Forms.NotifyIcon nIcon;
        private System.Windows.Forms.Label labMeditationTime;
        private System.Windows.Forms.Label labAllMeditationTime;
        private System.Windows.Forms.Label labDeepMeditationTime;
        private System.Windows.Forms.Label lblEstimatedExp;
        private System.Windows.Forms.RichTextBox rtbChat;
        private System.Windows.Forms.Button bShowInfo;
    }
}

