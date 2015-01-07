namespace PWMeditation
{
    partial class frmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.lvAccountList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bAdd = new System.Windows.Forms.Button();
            this.bDel = new System.Windows.Forms.Button();
            this.bClose = new System.Windows.Forms.Button();
            this.bEdit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvAccountList
            // 
            this.lvAccountList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lvAccountList.FullRowSelect = true;
            this.lvAccountList.GridLines = true;
            this.lvAccountList.Location = new System.Drawing.Point(12, 12);
            this.lvAccountList.Name = "lvAccountList";
            this.lvAccountList.Size = new System.Drawing.Size(300, 244);
            this.lvAccountList.TabIndex = 0;
            this.lvAccountList.UseCompatibleStateImageBehavior = false;
            this.lvAccountList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Логин";
            this.columnHeader1.Width = 68;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Сервер";
            this.columnHeader2.Width = 87;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Персонаж";
            this.columnHeader3.Width = 67;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Тип входа";
            this.columnHeader4.Width = 74;
            // 
            // bAdd
            // 
            this.bAdd.Location = new System.Drawing.Point(318, 12);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(113, 38);
            this.bAdd.TabIndex = 1;
            this.bAdd.Text = "Добавить";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // bDel
            // 
            this.bDel.Location = new System.Drawing.Point(318, 56);
            this.bDel.Name = "bDel";
            this.bDel.Size = new System.Drawing.Size(113, 38);
            this.bDel.TabIndex = 2;
            this.bDel.Text = "Удалить";
            this.bDel.UseVisualStyleBackColor = true;
            this.bDel.Click += new System.EventHandler(this.bDel_Click);
            // 
            // bClose
            // 
            this.bClose.Location = new System.Drawing.Point(318, 218);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(113, 38);
            this.bClose.TabIndex = 3;
            this.bClose.Text = "Закрыть";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // bEdit
            // 
            this.bEdit.Location = new System.Drawing.Point(318, 100);
            this.bEdit.Name = "bEdit";
            this.bEdit.Size = new System.Drawing.Size(113, 38);
            this.bEdit.TabIndex = 4;
            this.bEdit.Text = "Изменить";
            this.bEdit.UseVisualStyleBackColor = true;
            this.bEdit.Click += new System.EventHandler(this.bEdit_Click);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 268);
            this.Controls.Add(this.bEdit);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.bDel);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.lvAccountList);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройка";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvAccountList;
        private System.Windows.Forms.Button bAdd;
        private System.Windows.Forms.Button bDel;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button bEdit;
    }
}