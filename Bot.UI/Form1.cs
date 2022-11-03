using Bot.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Keys = Bot.Core.Keys;

namespace Bot.UI
{
    public partial class WeChat : Form
    {
        PvpBot pvpbot;
        Thread botThread;

        public WeChat()
        {
            InitializeComponent();
            this.FormClosing += WeChat_FormClosing;
        }

        private void WeChat_FormClosing(object sender, FormClosingEventArgs e)
        {
            botThread?.Abort();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pvpbot == null)
            {
                botThread = new Thread(new ThreadStart(this.BotThreadStarter));
                botThread.Start();
            }
        }

        public void BotThreadStarter()
        {
            pvpbot = new PvpBot();
            pvpbot?.Start();
            pvpbot = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pvpbot?.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread.Sleep(30);
            var p = new IntPtr(398874);
            WowProcess.PostMessage(p, Keys.WM_LBUTTONDOWN, 0, MakeLong(325,484));
            Thread.Sleep(30);
            WowProcess.PostMessage(p, Keys.WM_LBUTTONUP, 0, MakeLong(325, 484));
        }
        public static int MakeLong(int a, int b)
        {

            return (int)((uint)((ushort)(a)) | ((uint)((ushort)(b) << 16)));

        }
    }
}
