using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PWMeditation
{
    partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
            RefreshList();
        }

        private void RefreshList()
        {
            lvAccountList.Items.Clear();
            foreach (Settings set in Program.SettingsList)
            {
                string login = set.Login;
                string server = set.ServerName; if (string.IsNullOrEmpty(server)) server = string.Format("{0}:{1}", set.Server, set.Port);
                string role = set.PlayerName;
                string force = set.Force ? "Усиленный" : "Обычный";

                lvAccountList.Items.Add(login).SubItems.AddRange(new string[] { server, role, force });
            }
        }
        private int Index
        {
            get
            {
                foreach (ListViewItem sel in lvAccountList.SelectedItems)
                {
                    return sel.Index;
                }
                return -1;
            }
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            frmAccount frm = new frmAccount();
            frm.ShowDialog();
            if (!frm.Save) return;
            Program.SettingsList.Add(frm.AccountSettings);
            RefreshList();
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bDel_Click(object sender, EventArgs e)
        {
            if (Index == -1) return;
            Program.SettingsList.RemoveAt(Index);
            RefreshList();
        }

        private void bEdit_Click(object sender, EventArgs e)
        {
            if (Index == -1) return;
            frmAccount frm = new frmAccount(Program.SettingsList[Index]);
            frm.ShowDialog();
            RefreshList();
        }
    }
}
