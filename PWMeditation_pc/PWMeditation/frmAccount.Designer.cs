namespace PWMeditation
{
    partial class frmAccount
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccount));
            this.cbServer = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbLogin = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.cbRole = new System.Windows.Forms.ComboBox();
            this.bLoad = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbForce = new System.Windows.Forms.CheckBox();
            this.bOk = new System.Windows.Forms.Button();
            this.timeUpdate = new System.Windows.Forms.Timer(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.cbShowOnline = new System.Windows.Forms.CheckBox();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbUseMessage = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbServer
            // 
            this.cbServer.FormattingEnabled = true;
            this.cbServer.Items.AddRange(new object[] {
            "Орион",
            "Вега",
            "Сириус",
            "Мира",
            "Таразед",
            "Альтаир",
            "Процион",
            "Астра",
            "Пегас",
            "Антарес",
            "Адара",
            "Феникс",
            "Лиридан",
            "Омега",
            "Archosaur",
            "Lost City",
            "Sanctuary",
            "Heavens Tear",
            "Raging Tide",
            "Harshlands",
            "Dreamweaver",
            "Morai"});
            this.cbServer.Location = new System.Drawing.Point(86, 12);
            this.cbServer.Name = "cbServer";
            this.cbServer.Size = new System.Drawing.Size(149, 21);
            this.cbServer.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Сервер:";
            // 
            // tbLogin
            // 
            this.tbLogin.Location = new System.Drawing.Point(86, 39);
            this.tbLogin.Name = "tbLogin";
            this.tbLogin.Size = new System.Drawing.Size(149, 21);
            this.tbLogin.TabIndex = 2;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(86, 65);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '•';
            this.tbPassword.Size = new System.Drawing.Size(149, 21);
            this.tbPassword.TabIndex = 3;
            // 
            // cbRole
            // 
            this.cbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRole.FormattingEnabled = true;
            this.cbRole.Location = new System.Drawing.Point(86, 91);
            this.cbRole.Name = "cbRole";
            this.cbRole.Size = new System.Drawing.Size(98, 21);
            this.cbRole.TabIndex = 4;
            // 
            // bLoad
            // 
            this.bLoad.Location = new System.Drawing.Point(190, 91);
            this.bLoad.Name = "bLoad";
            this.bLoad.Size = new System.Drawing.Size(45, 21);
            this.bLoad.TabIndex = 5;
            this.bLoad.Text = "Load";
            this.bLoad.UseVisualStyleBackColor = true;
            this.bLoad.Click += new System.EventHandler(this.bLoad_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Логин:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Пароль:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Персонажи:";
            // 
            // cbForce
            // 
            this.cbForce.AutoSize = true;
            this.cbForce.Location = new System.Drawing.Point(107, 145);
            this.cbForce.Name = "cbForce";
            this.cbForce.Size = new System.Drawing.Size(109, 17);
            this.cbForce.TabIndex = 9;
            this.cbForce.Text = "Усиленный вход";
            this.cbForce.UseVisualStyleBackColor = true;
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(86, 234);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(149, 41);
            this.bOk.TabIndex = 10;
            this.bOk.Text = "OK";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // timeUpdate
            // 
            this.timeUpdate.Enabled = true;
            this.timeUpdate.Tick += new System.EventHandler(this.timeUpdate_Tick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Медитация:";
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "Обычная",
            "Глубокая",
            "Обычная, а потом глубокая",
            "Глубокая, а потом обычная"});
            this.cbType.Location = new System.Drawing.Point(86, 118);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(149, 21);
            this.cbType.TabIndex = 11;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // cbShowOnline
            // 
            this.cbShowOnline.AutoSize = true;
            this.cbShowOnline.Location = new System.Drawing.Point(107, 165);
            this.cbShowOnline.Name = "cbShowOnline";
            this.cbShowOnline.Size = new System.Drawing.Size(127, 17);
            this.cbShowOnline.TabIndex = 13;
            this.cbShowOnline.Text = "Показывать онлайн";
            this.cbShowOnline.UseVisualStyleBackColor = true;
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(86, 188);
            this.tbMessage.MaxLength = 79;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(131, 21);
            this.tbMessage.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 193);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Автоответ:";
            // 
            // cbUseMessage
            // 
            this.cbUseMessage.AutoSize = true;
            this.cbUseMessage.Location = new System.Drawing.Point(220, 191);
            this.cbUseMessage.Name = "cbUseMessage";
            this.cbUseMessage.Size = new System.Drawing.Size(15, 14);
            this.cbUseMessage.TabIndex = 16;
            this.cbUseMessage.UseVisualStyleBackColor = true;
            // 
            // frmAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 287);
            this.Controls.Add(this.cbUseMessage);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.cbShowOnline);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.cbForce);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bLoad);
            this.Controls.Add(this.cbRole);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbLogin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbServer);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmAccount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Аккаунт";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLogin;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.ComboBox cbRole;
        private System.Windows.Forms.Button bLoad;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbForce;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Timer timeUpdate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.CheckBox cbShowOnline;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cbUseMessage;
    }
}