using System;
using System.Diagnostics;

using SiviliaFramework.Network.Plugins;
using SiviliaFramework.Data;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace PWMeditation
{
    [Activity(Label = "PWMeditation", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.Light")]
    public class MainActivity : Activity
    {
        Spinner spinAccounts;
        ArrayAdapter array;

        TextView tvMeditationStatus;
        TextView tvLevel;
        TextView tvExperience;
        TextView tvMeditationToday;
        TextView tvMeditationHigh;
        TextView tvMeditationLow;
        ProgressBar pbExperience;

        public Account SelectedAccount;

        protected override void OnCreate(Bundle bundle)
        {
            Settings.Initialize();

            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            spinAccounts = FindViewById<Spinner>(Resource.Id.spinAccounts);

            tvMeditationStatus = FindViewById<TextView>(Resource.Id.tvMeditationStatus);
            tvLevel = FindViewById<TextView>(Resource.Id.tvLevel);
            tvExperience = FindViewById<TextView>(Resource.Id.tvExperience);
            tvMeditationToday = FindViewById<TextView>(Resource.Id.tvMeditationToday);
            tvMeditationHigh = FindViewById<TextView>(Resource.Id.tvMeditationHigh);
            tvMeditationLow = FindViewById<TextView>(Resource.Id.tvMeditationLow);
            pbExperience = FindViewById<ProgressBar>(Resource.Id.pbExperience);

            array = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, Android.Resource.Id.Text1);
            array.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinAccounts.Adapter = array;

            spinAccounts.ItemSelected += spinAccounts_ItemSelected;
            spinAccounts.NothingSelected += spinAccounts_NothingSelected;

            foreach(Account account in Settings.Accounts)
            {
                array.Add(account.ToString());
            }

            Settings.Main = this;

            int index = Intent.GetIntExtra("accountIndex", -1);
            if (index == -1)
                SelectedAccount = null;
            else
                SelectedAccount = Settings.Accounts[index];

            RefreshAccountInfo();
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add(0, 0, 0, GetString(Resource.String.connect));
            menu.Add(0, 1, 1, GetString(Resource.String.disconnect));

            IMenu accountMenu = menu.AddSubMenu(0, 2, 2, GetString(Resource.String.account));
            accountMenu.Add(0, 3, 3, GetString(Resource.String.add));
            accountMenu.Add(0, 4, 4, GetString(Resource.String.delete));
            accountMenu.Add(0, 5, 5, GetString(Resource.String.edit));

            menu.Add(0, 6, 6, GetString(Resource.String.settings));

            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnMenuOpened(int featureId, IMenu menu)
        {
            if (menu != null)
            {
                if (menu.FindItem(0) != null)
                {
                    menu.FindItem(0).SetEnabled(SelectedAccount != null ? !SelectedAccount.IsWork : false);
                    menu.FindItem(1).SetEnabled(SelectedAccount != null ? SelectedAccount.IsWork : false);
                }
                if (menu.FindItem(4) != null)
                {
                    bool serverListIsEmpty = Settings.GameServers.Count == 0;
                    bool selected = SelectedAccount != null;
                    bool connected = selected && SelectedAccount.IsWork;

                    menu.FindItem(3).SetEnabled(!serverListIsEmpty);
                    menu.FindItem(4).SetEnabled(!connected && selected);
                    menu.FindItem(5).SetEnabled(!connected && !serverListIsEmpty);
                }
            }
            return base.OnMenuOpened(featureId, menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case 0:
                    {
                        if (SelectedAccount != null)
                            SelectedAccount.Open();
                    }
                    return true;
                case 1:
                    {
                        if (SelectedAccount != null)
                            SelectedAccount.Close();
                    }
                    return true;
                case 3:
                    {
                        Account account = new Account();
                        account.ShowOnline = true;
                        account.Force = true;
                        account.AutoMessage = GetString(Resource.String.default_automessage);

                        Settings.Accounts.Insert(0, account);
                        array.Insert(account.ToString(), 0);

                        array.NotifyDataSetChanged();
                        spinAccounts.SetSelection(0);

                        ChangeAccount();
                    }
                    return true;
                case 4:
                    {
                        int index = (int)spinAccounts.SelectedItemId;
                        array.Remove(array.GetItem(index));
                        Settings.Accounts.RemoveAt(index);
                    }
                    return true;
                case 5:
                    {
                        ChangeAccount();
                    }
                    return true;
                case 6:
                    {
                        ServersSettingsActivity serversSettingsActivity = new ServersSettingsActivity();
                        Intent intent = new Intent(this, serversSettingsActivity.Class);
                        StartActivity(intent);
                    }
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        private void ChangeAccount()
        {
            int accountIndex = (int)spinAccounts.SelectedItemId;

            Intent intent = new Intent(this, new AccountSettingsActivity().Class);
            intent.PutExtra("accountIndex", accountIndex);

            StartActivity(intent);
        }
        public void RefreshAccount(int index)
        {
            array.Remove(array.GetItem(index));
            array.Insert(Settings.Accounts[index].ToString(), index);
            array.NotifyDataSetChanged();
        }
        public void RefreshAccountInfo(Account account)
        {
            if (account == SelectedAccount)
            {
                RunOnUiThread(RefreshAccountInfo);
            }
        }
        public void RefreshAccountInfo()
        {
            if (SelectedAccount == null)
            {
                RefreshAccountInfo(string.Empty, 0, 0, 0, 0, 0, 0, 0);
            }
            else
            {
                Account account = SelectedAccount;

                string status = GetString(Resource.String.disconnected);
                int level = 0;
                int exp = 0;
                int maxExp = 0;
                float expPercent = 0;
                int today = 0;
                int high = 0;
                int low = 0;

                if (account.Relogin != null)
                {
                    switch (account.Relogin.Status)
                    {
                        case ReloginStatus.Connected:
                            MeditationInformation meditation = account.Meditation.MeditationInformation;
                            RoleInfo roleInfo = account.Auth.AccountInformation.SelectedRole;
                            if (roleInfo != null)
                            {
                                status = meditation.MeditationEnabled ? GetString(Resource.String.meditation) : GetString(Resource.String.in_the_world);
                                level = roleInfo.Level;
                                exp = roleInfo.Experience;
                                maxExp = roleInfo.MaxExperience;
                                expPercent = roleInfo.ExpPercent;
                                today = (int)meditation.SecondsToday;
                                high = (int)(meditation.MeditationTypes[1] == null ? 0 : meditation.MeditationTypes[1].Seconds);
                                low = (int)(meditation.MeditationTypes[1] == null ? 0 : meditation.MeditationTypes[0].Seconds);
                            }
                            break;
                        case ReloginStatus.Connecting:
                            status = GetString(Resource.String.connecting);
                            break;
                        case ReloginStatus.Relogin:
                            status = GetString(Resource.String.relogin);
                            break;
                    }
                }
                RefreshAccountInfo(status, level, exp, maxExp, expPercent, today, high, low);
            }
        }
        public void RefreshAccountInfo(string status, int level, int exp, int maxExp, float expPercent, int today, int high, int low)
        {
            tvMeditationStatus.Text = status;
            pbExperience.Progress = (int)expPercent;

            tvLevel.Text = string.Format("{0}: {1}", GetString(Resource.String.level), level);
            tvExperience.Text = string.Format("{0}: {1}/{2} ({3}%)", GetString(Resource.String.experience), exp, maxExp, expPercent);
            tvMeditationToday.Text = string.Format("{0}: {1}", GetString(Resource.String.today), SecondsToString(today));
            tvMeditationHigh.Text = string.Format("{0}: {1}", GetString(Resource.String.meditation_high), SecondsToString(high));
            tvMeditationLow.Text = string.Format("{0}: {1}", GetString(Resource.String.meditation_low), SecondsToString(low));
        }

        static int[] timeTable = new int[] { 60, 60, 24, int.MaxValue };
        static string[] timeStringTable;
        private string SecondsToString(int seconds)
        {
            if (timeStringTable == null)
            {
                timeStringTable = new string[]
                { 
                    GetString(Resource.String.seconds),
                    GetString(Resource.String.minutes),
                    GetString(Resource.String.hours),
                    GetString(Resource.String.days)
                };
            }
            string res = string.Empty;
            for (int i = 0; i < timeTable.Length; i++)
            {
                int a = seconds % timeTable[i];
                seconds = seconds / timeTable[i];
                if (a != 0)
                {
                    res = a + " " + timeStringTable[i] + (res.Length > 0 ? " " + res : string.Empty);
                }
            }
            return res;
        }

        private void ShowToast(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }

        private void spinAccounts_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            SelectedAccount = Settings.Accounts[(int)e.Id];
            Intent.PutExtra("accountIndex", (int)e.Id);

            RefreshAccountInfo();
        }
        private void spinAccounts_NothingSelected(object sender, AdapterView.NothingSelectedEventArgs e)
        {
            SelectedAccount = null;
            Intent.PutExtra("accountIndex", -1);

            RefreshAccountInfo();
        }
    }
}