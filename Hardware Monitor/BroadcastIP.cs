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
            string ip_address = IPInformation.GetLocalIPAddress();

            string text_to_send = ip_address + ":" + Dns.GetHostName();

            string broadcast_ip = ip_address.Remove(ip_address.LastIndexOf('.')) + ".255";

            while (true)
            {
                bool done = false;

                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                IPAddress send_to = IPAddress.Parse(broadcast_ip);

                IPEndPoint sending_end_point = new IPEndPoint(send_to, 11000);

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
    }
}
