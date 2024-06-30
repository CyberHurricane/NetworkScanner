using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace NetworkScanner
{
    internal class NetworkScanner
    {
        static void Main(string[] args)
        { 
            
        }
    }
    public class UdpBroadcast
    {
        public static void SendBroadcast(string message, int port)
        {
            using (UdpClient UdpClient = new UdpClient())
            {
                UdpClient.EnableBroadcast = true;
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, port);
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                UdpClient.Send(bytes, bytes.Length, endPoint);
                Console.WriteLine("Broadcast message sent: " + message);
            }
        }
    }
}
