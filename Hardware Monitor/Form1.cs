using System;
using System.Windows.Forms;
using System.Threading;

namespace Hardware_Monitor
{
    public partial class Form1 : Form
    {
        public Thread thread;
        public Thread udp;
        public Thread server;
        public Server myserver;

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

            BroadcastIP ip = new BroadcastIP();

            udp = new Thread(new ThreadStart(ip.startBroadcast));

            udp.Start();

            while (!udp.IsAlive) ;


            myserver = new Server();

            server = new Thread(new ThreadStart(myserver.startServer));

            server.Start();

            while (!server.IsAlive) ;

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
            if (server != null)
            {
                myserver.running = false;
                server.Abort();
                server.Join();
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

            myserver.running = false;
            server.Abort();
            server.Join();

            start.Enabled = true;
            stop.Enabled = false;
            status.Text = "Stopped";
        }
    }
}
