using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using OOGLibrary.IO;
using OOGLibrary.IO.PacketBase.Server;
using OOGLibrary.Network;
using OOGLibrary.Network.Templates;

namespace PWMeditation
{
    internal partial class frmMain : Form
    {
        private int SizeMin = 225;
        private int SizeMax = 440;
        private string[] args;

        public frmMain(string[] args)
        {
            InitializeComponent();
            this.Size = new Size(SizeMin, 320);
            this.args = args;
        }

        private OOGAccountHost oog { get; set; }

        private string Status { get; set; }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Settings.Load();
            foreach (Settings set in Program.SettingsList)
            {
                string servername = String.Format("[{0}] ", set.ServerName);
                if (set.ServerName == string.Empty)
                    servername = string.Empty;
                string text = String.Format("{0}{1} - {2}", servername, set.Login, set.PlayerName);
                cbAccounts.Items.Add(text);
            }

            labAllMeditationTime.Text = String.Format("Сегодня: {0}", TimeToNormal());
            labDeepMeditationTime.Text = String.Format("Гл. медитация: {0}", TimeToNormal());
            labMeditationTime.Text = String.Format("Медитация: {0}", TimeToNormal());
            lblEstimatedExp.Text = "Будет: -";

            //Автоматический запуск медитации
            if (args.Length == 1)
            {
                var login = args[0].Trim();

                var index = 0;

                foreach (var set in Program.SettingsList)
                {
                    if (set.Login.ToLower() == login.ToLower())
                    {
                        cbAccounts.SelectedIndex = index;
                        bConnect_Click(null, null);
                        break;
                    }
                    index++;
                }
            }
        }
        //Lalala): <1><^ffffff[Большая микстура духа]>
        private void AddMessage(string name, string message)
        {
            int pos = rtbChat.Text.Length;
            rtbChat.AppendText(name + ": ");
            rtbChat.Select(pos, name.Length + 1);
            rtbChat.SelectionFont = new Font(rtbChat.Font, FontStyle.Bold);

            WriteText(message);
            rtbChat.AppendText("\n");
            rtbChat.Select(rtbChat.Text.Length - 1, 0);
            rtbChat.ScrollToCaret();
        }
        private void WriteText(string message)
        {
            while (message.Length != 0)
            {
                int pos = -1;
                for (int i = 0; i < message.Length; i++)
                {
                    if (message[i] > 57300 && message[i] < 57400)//Ищем непонятный пвшный символ, после которого идет шифт/смайл
                    {
                        pos = i;
                        break;
                    }
                }

                if (pos == -1)//Если шифта или смайла нет - пишем весь текст
                {
                    rtbChat.AppendText(message);
                    return;
                }
                rtbChat.AppendText(message.Substring(0, pos));//Записываем все, что находится перед смайлом/шифтом
                message = message.Substring(pos);
                try
                {
                    char type = message[2];// 0 - смайл, 1 - итем

                    message = message.Substring(5); // обрезаем <1><

                    pos = message.IndexOf('>');
                    if (pos == -1) continue;

                    string text = message.Substring(0, pos); // Текст внутри скобок <>
                    message = message.Substring(pos + 1); // Текст после смайла/шифта

                    if (type == '0') // Смайлов у нас пока нет:(
                        continue;

                    WriteItem(text.Substring(1));//^ffffff[Большая микстура духа]
                }
                catch
                {
                    if (message.Length > 1)
                        message = message.Substring(1);
                    else return;
                }
            }
        }
        private void WriteItem(string text)
        {
            // ffffff[Большая микстура духа]
            string name = text.Substring(6);
            int RGB = Convert.ToInt32(text.Substring(0, 6), 16);
            Color col = Color.FromArgb(RGB);

            // if (col.ToArgb() == 0xFFFFFF) col = Color.Black;

            int pos = rtbChat.Text.Length;
            rtbChat.AppendText(name);
            rtbChat.Select(pos, name.Length);
            rtbChat.SelectionFont = new Font(rtbChat.Font, FontStyle.Bold);
            rtbChat.SelectionColor = col;

            rtbChat.Select(rtbChat.Text.Length, 0);
            rtbChat.SelectionFont = new Font(rtbChat.Font, FontStyle.Regular);
            rtbChat.SelectionColor = rtbChat.ForeColor;
        }

        private string TimeToNormal(int time)
        {
            return TimeToNormal((uint)time);
        }

        private string TimeToNormal(uint time = 0)
        {
            var str = "";

            if (time > 86400)
            {
                var days = Math.Floor((decimal)time / 86400);
                time -= (uint)days * 86400;
                str += days + " д ";
            }

            if (time > 3600)
            {
                var h = Math.Floor((decimal)time / 3600);
                time -= (uint)h * 3600;
                str += h + " ч ";
            }

            if (time > 60)
            {
                var h = Math.Floor((decimal)time / 60);
                //time -= (uint) h*60;
                str += h + " м ";
            }

            //str += time + " c";

            if (str == "") str = "-";

            return str;
        }

        private void bSettings_Click(object sender, EventArgs e)
        {
            frmSettings frm = new frmSettings();
            frm.ShowDialog();
            cbAccounts.Items.Clear();
            foreach (Settings set in Program.SettingsList)
            {
                string servername = String.Format("[{0}] ", set.ServerName);
                if (set.ServerName == string.Empty)
                    servername = string.Empty;
                string text = String.Format("{0}{1} - {2}", servername, set.Login, set.PlayerName);
                cbAccounts.Items.Add(text);
            }
            Settings.Save();
        }

        private void bConnect_Click(object sender, EventArgs e)
        {
            if (cbAccounts.SelectedIndex == -1) return;
            MeditationLogic.Index = cbAccounts.SelectedIndex;
            bool value = MeditationLogic.Enabled;
            MeditationLogic.Enabled = !value;

            cbAccounts.Enabled = value;
            bSettings.Enabled = value;
            bConnect.Text = value ? "Подключиться" : "Отключиться";
            rtbChat.Text = string.Empty;
        }

        private void tmUpdate_Tick(object sender, EventArgs e)
        {
            labStatus.Text = MeditationLogic.MeditationEnabled;
            labExp.Text = String.Format("Опыт: {0}/{1}({2}%)", RoleInfo.Experience, RoleInfo.MaxExperience,
                                        RoleInfo.ExpPercent.ToString("0.0"));
            pbExp.Value = (int)RoleInfo.ExpPercent;
            labLevel.Text = "Лвл: " + RoleInfo.Level;


            labAllMeditationTime.Text = String.Format("Сегодня: {0}", TimeToNormal(MeditationLogic.MeditationTotal));
            labDeepMeditationTime.Text = String.Format("Гл. медитация: {0}", TimeToNormal(MeditationLogic.MeditationDeep));
            labMeditationTime.Text = String.Format("Медитация: {0}", TimeToNormal(MeditationLogic.MeditationNormal));


            if (MeditationLogic.Enabled)
            {
                if (MeditationLogic.PrivateMessages.Count != 0)
                {
                    ReceiveMessageEventArgs message = MeditationLogic.PrivateMessages[0];
                    MeditationLogic.PrivateMessages.RemoveAt(0);
                    AddMessage(message.Name, message.Message);
                }

                nIcon.Text = String.Format("PWMeditation - {0}\r\n  {1}\r\n  Опыт: {2}%", MeditationLogic.Settings.PlayerName,
                                           labLevel.Text, RoleInfo.ExpPercent.ToString("0.0"));


                if (MeditationLogic.ExpDelta != 0)
                {
                    //labExp.Text += String.Format(" (в минуту {0})",MeditationLogic.ExpDelta);

                    //Считаем время до апа в медитации
                    var ExpNeeded = RoleInfo.MaxExperience - RoleInfo.Experience;
                    var MinutesRemaining = ExpNeeded / MeditationLogic.ExpDelta;
                    labLevel.Text += ". До апа: " + TimeToNormal(MinutesRemaining * 60);


                    if (MeditationLogic.oog != null)
                    {
                        //Считаем сколько опыта будет после медитации
                        var MeditationTime = MeditationLogic.oog.Meditation.UsedType == 0
                                               ? MeditationLogic.MeditationNormal
                                               : MeditationLogic.MeditationDeep;
                        var time = (MeditationTime < MeditationLogic.MeditationTotal
                                      ? MeditationTime
                                      : MeditationLogic.MeditationTotal) / 60;

                        double EstExp = RoleInfo.Experience + time * MeditationLogic.ExpDelta;
                        lblEstimatedExp.Text = String.Format("Будет: {0}/{1}({2}%)", EstExp, RoleInfo.MaxExperience,
                                                             (100 * EstExp / RoleInfo.MaxExperience).ToString("0.0"));
                    }
                }
            }
            else nIcon.Text = "PWMeditation";
        }

        private void nIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            ShowInTaskbar = WindowState != FormWindowState.Minimized;
        }

        private void bShowInfo_Click(object sender, EventArgs e)
        {
            // 229; 346 - http://i.imgur.com/cz4avh5.png
            // 428; 320 - http://i.imgur.com/RGeFhl3.png

            if (this.Size.Width == SizeMin)
            {
                this.Size = new Size(SizeMax, 320);
                return;
            }
            else this.Size = new Size(SizeMin, 320);
        }
    }
}