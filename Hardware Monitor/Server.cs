using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Hardware_Monitor
{
    public class Server
    {
        private TcpListener listener;
        private int port;
        private string IP;
        public volatile bool running = false;
        public Thread thread;

        public Server()
        {
            port = 9999;
            IP = IPInformation.GetLocalIPAddress();
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(IP),port);
            listener = new TcpListener(ipEnd);
        }

        public void startServer()
        {
            running = true;
            try
            {
                listener.Start();
                thread = new Thread(new ThreadStart(StartListen));
                thread.Start();
            }
            catch (Exception)
            {    
            }
        }

        public void stopServer()
        {
            running = false;
            if (thread != null)
                thread.Abort();
        }

        public void StartListen()
        {
            while(running)
            {
                Socket socket = getSocket();

                if (socket != null)
                {
                    if (socket.Connected)
                    {
                        string filepath = Path.Combine(Environment.ExpandEnvironmentVariables("%APPDATA%"), "HardwareMonitor", "Usage.txt");
                        string response = "";
                        string[] file = new string[1];

                        try
                        {
                            file = File.ReadAllLines(filepath);
                        }
                        catch (Exception)
                        {

                            Console.WriteLine("Error reading file");
                        }

                        response = file[0];

                        if (response != null)
                        {
                            byte[] sendData = Encoding.ASCII.GetBytes(response);

                            socket.Send(sendData, sendData.Length, 0);
                        }
                    }
                    socket.Close();
                }
            }
        }

        private Socket getSocket()
        {
            Socket socket = null;

            if (listener.Pending())
            {
                socket = listener.AcceptSocket();
            }

            return socket;
        }
    }
}
