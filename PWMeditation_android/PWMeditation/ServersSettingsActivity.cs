using System;

using SiviliaFramework.Network;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace PWMeditation
{
    [Activity(Label = "Settings", Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.Light")]
    public class ServersSettingsActivity : Activity
    {
        Button bSaveServer;
        EditText etServerNameSettings;
        EditText etServerAddressSettings;

        Spinner spinServers;
        ArrayAdapter serverList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ServerSettings);

            bSaveServer = FindViewById<Button>(Resource.Id.bSaveServer);
            etServerNameSettings = FindViewById<EditText>(Resource.Id.etServerNameSettings);
            etServerAddressSettings = FindViewById<EditText>(Resource.Id.etServerAddressSettings);

            spinServers = FindViewById<Spinner>(Resource.Id.spinServerListSettings);

            serverList = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, Android.Resource.Id.Text1);
            serverList.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinServers.Adapter = serverList;


            if (Settings.GameServers.Count == 0)
            {
                RefreshEnabled(false);
            }
            else
            {
                foreach (GameServer gameServer in Settings.GameServers)
                {
                    serverList.Add(gameServer.ToShortString());
                    serverList.NotifyDataSetChanged();
                }
            }
            spinServers.ItemSelected += spinServers_ItemSelected;
            spinServers.NothingSelected += spinServers_NothingSelected;
            bSaveServer.Click += bSaveServer_Click;
        }

        protected override void OnStop()
        {
            Settings.Save();
            base.OnStop();
        }

        private void RefreshEnabled(bool enabled)
        {
            bSaveServer.Enabled = enabled;
            etServerNameSettings.Enabled = enabled;
            etServerAddressSettings.Enabled = enabled;

            if (!enabled)
            {
                etServerAddressSettings.Text = string.Empty;
                etServerNameSettings.Text = string.Empty;
            }
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add(0, 0, 0, GetString(Resource.String.add));
            menu.Add(0, 1, 1, GetString(Resource.String.delete));

            return base.OnCreateOptionsMenu(menu);
        }

        private void bSaveServer_Click(object sender, EventArgs e)
        {
            if (spinServers.SelectedItemId == -1)
            {
                return;
            }
            int index = (int)spinServers.SelectedItemId;
            GameServer gameServer = Settings.GameServers[index];
            gameServer.Name = etServerNameSettings.Text;
            gameServer.SetServer(etServerAddressSettings.Text);


            serverList.Remove(serverList.GetItem(index));
            serverList.Insert(Settings.GameServers[index].ToShortString(), index);
            serverList.NotifyDataSetChanged();
        }

        private void spinServers_NothingSelected(object sender, AdapterView.NothingSelectedEventArgs e)
        {
            RefreshEnabled(false);
        }

        private void spinServers_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (e.Id == -1)
            {
                RefreshEnabled(false);
            }
            else
            {
                RefreshEnabled(true);
                RefreshSelected(Settings.GameServers[(int)e.Id]);
            }
        }
        private void RefreshSelected(GameServer gameServer)
        {
            etServerNameSettings.Text = gameServer.Name;
            etServerAddressSettings.Text = gameServer.Host + ":" + gameServer.Port;
        }

        public override bool OnMenuOpened(int featureId, IMenu menu)
        {
            if (menu != null)
            {
                menu.GetItem(1).SetEnabled(spinServers.SelectedItemId >= 0);
            }
            return base.OnMenuOpened(featureId, menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case 0:
                    {
                        GameServer gameServer = new GameServer("127.0.0.1", 29000, "New server");

                        Settings.GameServers.Insert(0, gameServer);
                        serverList.Insert(gameServer.ToShortString(), 0);

                        serverList.NotifyDataSetChanged();
                        spinServers.SetSelection(0);

                        RefreshSelected(Settings.GameServers[0]);
                    }
                    return true;
                case 1: 
                    {
                        int index = (int)spinServers.SelectedItemId;

                        Settings.GameServers.RemoveAt(index);
                        serverList.Remove(serverList.GetItem(index));
                        serverList.NotifyDataSetChanged();

                        if (index == 0 && Settings.GameServers.Count > 0)
                        {
                            RefreshSelected(Settings.GameServers[(int)spinServers.SelectedItemId]);
                        }
                    }
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}
