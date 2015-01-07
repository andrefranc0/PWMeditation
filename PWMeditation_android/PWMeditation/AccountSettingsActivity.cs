
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SiviliaFramework.Network;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PWMeditation
{
    [Activity(Label = "Account", Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.Light")]			
    public class AccountSettingsActivity : Activity
    {
        Spinner spinSelectedServer;
        ArrayAdapter arraySelectedServer;

        EditText etLogin;
        EditText etPassword;

        Spinner spinSelectedRole;
        ArrayAdapter arraySelectedRole;
        Button bLoadRoles;

        EditText etAutoMessage;

        CheckBox cbForce;
        CheckBox cbShowOnline;
        CheckBox cbAutoMessage;

        Button bSave;
        int accountIndex;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.AccountSettings);

            // Initialize controls
            spinSelectedServer = FindViewById<Spinner>(Resource.Id.spinSelectedServer);
            arraySelectedServer = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, Android.Resource.Id.Text1);
            arraySelectedServer.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinSelectedServer.Adapter = arraySelectedServer;

            etLogin = FindViewById<EditText>(Resource.Id.etLogin);
            etPassword = FindViewById<EditText>(Resource.Id.etPassword);

            spinSelectedRole = FindViewById<Spinner>(Resource.Id.spinSelectedRole);
            arraySelectedRole = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, Android.Resource.Id.Text1);
            arraySelectedRole.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinSelectedRole.Adapter = arraySelectedRole;

            bLoadRoles = FindViewById<Button>(Resource.Id.bLoadRoles);

            etAutoMessage = FindViewById<EditText>(Resource.Id.etAutoMessage);

            cbForce = FindViewById<CheckBox>(Resource.Id.cbForce);
            cbShowOnline = FindViewById<CheckBox>(Resource.Id.cbShowOnline);
            cbAutoMessage = FindViewById<CheckBox>(Resource.Id.cbAutoMessage);

            bSave = FindViewById<Button>(Resource.Id.bSave);

            // Initialize logic

            foreach (GameServer gameServer in Settings.GameServers)
            {
                arraySelectedServer.Add(gameServer.ToShortString());
            }
            arraySelectedServer.NotifyDataSetChanged();

            bLoadRoles.Click += bLoadRoles_Click;
            bSave.Click += bSave_Click;

            accountIndex = Intent.GetIntExtra("accountIndex", 0);
            LoadAccount();
        }

        private void bLoadRoles_Click(object sender, EventArgs e)
        {
            Account account = Settings.Accounts[accountIndex];
            CompleteAccount();
            account.LoadRolesList();
            LoadAccount();
        }
        private void bSave_Click(object sender, EventArgs e)
        {
            Finish();
        }
        protected override void OnStop()
        {
            base.OnStop();
            CompleteAccount();
            Settings.Save();
        }

        private void LoadAccount()
        {
            Account account = Settings.Accounts[accountIndex];

            int serverIndex = -1;
            for (int i = 0; i < Settings.GameServers.Count; i++)
            {
                GameServer server = Settings.GameServers[i];
                if (server.Host + ":" + server.Port == account.ServerAddress)
                {
                    serverIndex = i;
                    if (server.Name == account.ServerName)
                    {
                        break;
                    }
                }
            }
            if (serverIndex != -1)
                spinSelectedServer.SetSelection(serverIndex);

            etLogin.Text = account.Login;
            etPassword.Text = account.Password;

            arraySelectedRole.Clear();
            foreach (string roleName in account.Roles)
            {
                arraySelectedRole.Add(roleName);
            }
            arraySelectedRole.NotifyDataSetChanged();
            spinSelectedRole.SetSelection(account.SelectedRole);

            etAutoMessage.Text = account.AutoMessage;

            cbForce.Checked = account.Force;
            cbShowOnline.Checked = account.ShowOnline;
            cbAutoMessage.Checked = account.UseAutoMessage;
        }
        private void CompleteAccount()
        {
            Account account = Settings.Accounts[accountIndex];
            GameServer server = Settings.GameServers[(int)spinSelectedServer.SelectedItemId];

            account.ServerName = server.Name;
            account.ServerAddress = server.Host + ":" + server.Port;

            account.Login = etLogin.Text;
            account.Password = etPassword.Text;
            account.SelectedRole = (int)spinSelectedRole.SelectedItemId;

            account.AutoMessage = etAutoMessage.Text;

            account.Force = cbForce.Checked;
            account.ShowOnline = cbShowOnline.Checked;
            account.UseAutoMessage = cbAutoMessage.Checked;

            account.CompleteSettings(accountIndex);
        }
    }
}

