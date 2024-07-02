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
            UserInput();
        }
        static void UserInput()
        {

            Console.WriteLine("Do you wish to perform a network scan?\n Y/N");
            string I = Console.ReadLine();
            string Input = I.ToUpper();
            if (Input == "Y")
            {
                
                return;
            }
            else if (Input == "N")
            {
                return;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Please enter a valid option");

                UserInput();

            }
        }
    }
    public class UserInput
    {
    }
    public class ICMP
    {

    }
    public class UdpBroadcast
    {
        public static void SendBroadcast()
        {
            using (UdpClient UdpClient = new UdpClient())
            {
                string IP = "10.0.0.255";
                int Port = 55000;
                string message = "Hello";

                byte[] messageBytes = Encoding.UTF8.GetBytes(message);

                using (UdpClient udpClient = new UdpClient())
                    try
                    {
                        udpClient.EnableBroadcast = true;
                        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(IP), Port);
                        udpClient.Send(messageBytes, messageBytes.Length, endPoint);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }

                StartListener(Port);
            }
        }
        public static void StartListener(int Port)
        {
            UdpClient listener = new UdpClient(Port);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, Port);
            listener.Client.ReceiveTimeout = 50000;
            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for broadcast");
                    byte[] bytes = listener.Receive(ref groupEP);

                    Console.WriteLine($"Received broadcast from {groupEP} :");
                    Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");
                    Console.ReadLine();
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.ReadLine();
                listener.Close();
            }
        }
    }// Unable to test if this is successful. Will reinvestigate at a later date
}
