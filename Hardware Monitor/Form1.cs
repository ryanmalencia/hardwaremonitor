using System;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace Hardware_Monitor
{
    public partial class Form1 : Form
    {
        public Thread thread;
        public Thread udp;
        public Process java;
        public Form1()
        {
            InitializeComponent();
        }

        private void start_Click(object sender, EventArgs e)
        {
            AsyncWriteData write = new AsyncWriteData();

            Application.ApplicationExit += Application_ApplicationExit;

            thread = new Thread(new ThreadStart(write.writeData));

            thread.Start();

            while (!thread.IsAlive) ;

            if (java == null)
            {
                java = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                string args = "-jar " + Path.Combine(Environment.GetEnvironmentVariable("HardwareServer",EnvironmentVariableTarget.Machine), "MyServer.jar") + " 9999";
                startInfo.Arguments = args;
                startInfo.FileName = "java";
                java.StartInfo = startInfo;
                java.Start();
            }

            BroadcastIP ip = new BroadcastIP();

            udp = new Thread(new ThreadStart(ip.startBroadcast));

            udp.Start();

            while (!thread.IsAlive) ;

            start.Enabled = false;
            stop.Enabled = true;
            status.Text = "Running";
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            if(udp !=null)
            {
                udp.Abort();
                udp.Join();
            }
            if(java != null)
            {
                java.CloseMainWindow();
                java.Close();
            }
            if(thread != null)
            {
                thread.Abort();
                thread.Join();
            }
        }

        private void stop_Click(object sender, EventArgs e)
        {
            udp.Abort();
            udp.Join();

            thread.Abort();
            thread.Join();

            if (!java.HasExited)
            {
                java.CloseMainWindow();
                java.Close();
            }

            java = null;

            start.Enabled = true;
            stop.Enabled = false;
            status.Text = "Stopped";
        }
    }
}
