using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Hardware_Monitor
{
    class BroadcastIP  
    {
        public BroadcastIP()
        {

        }

        public void startBroadcast()
        {
            while (true)
            {
                bool done = false;

                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                IPAddress send_to = IPAddress.Parse("10.0.0.255");

                IPEndPoint sending_end_point = new IPEndPoint(send_to, 11000);

                string text_to_send = GetLocalIPAddress();

                byte[] send_buffer = Encoding.ASCII.GetBytes(text_to_send);

                while (!done)
                {
                    try
                    {
                        sending_socket.SendTo(send_buffer, sending_end_point);
                        done = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception Caught");
                    }
                }
                Thread.Sleep(3000);
            }
        }

        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}
