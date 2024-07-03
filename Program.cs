using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;


namespace NetworkScanner
{
    {

        static void Main()
        {
            UserInput();
            Console.Clear();
            Console.WriteLine("Scanning For Hosts");
            string DNSAddress = DNSAddressResolver.ResolveDNS();   
            PS ps = new PS();
            string[] IPAddresses = ps.RunArp(DNSAddress);
            DNSAddressResolver.GetHostNameDNS();
            Console.Clear();
            Console.WriteLine("IPAddresses    Host Name");
            foreach (string Results in SharedData.DataDump)
            {
                Console.WriteLine(Results);
            }
            Console.ReadLine();
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
    public class DNSAddressResolver
    {
        internal static string ResolveDNS()
        {
            string DNSAddress;
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface adapter in adapters)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                var dnsAddresses = properties.DnsAddresses;
                if (dnsAddresses.Any())
                {
                    var I = dnsAddresses.First();
                    DNSAddress = I.ToString();
                    return DNSAddress;
                }
            }
            DNSAddress = "Failed";
            return DNSAddress;
        }
        internal static string IPList(string DNSAddress)
        {
            string[] DNS = DNSAddress.Split('.');
            int octet1 = Int32.Parse(DNS[0]);
            int octet2 = Int32.Parse(DNS[1]);
            int octet3 = Int32.Parse(DNS[2]);
            int octet4 = Int32.Parse(DNS[3]);
            Console.WriteLine($"{octet1}.{octet2}.{octet3}.{octet4}");
            /*
                        for (int i = 0; octet1 != 254; i++)
                        {
                            SharedData.ArrayOctet4[i] = octet4;
                            octet1++;
                        }
                        foreach (int ArrayOctet in SharedData.ArrayOctet4)
                        {
                            Console.WriteLine(ArrayOctet);
                        }

                        return DNSAddress;
            */
            return null;
        }
        internal static void GetHostNameDNS()
        {
            int length = SharedData.IPaddressFromArp.Length;
            SharedData.DataDump = new string[length];
            int index = 0;
            foreach (string HostIP in SharedData.IPaddressFromArp)
            {
                try
                {
                    IPHostEntry host = Dns.GetHostEntry(HostIP);
                    string Name = host.HostName;
                    string Data = HostIP + " : " + Name;

                    SharedData.DataDump[index] = Data;
                    index++; // Increment index for next entry
                }
                catch (Exception)
                {
                    string Data = HostIP + " : " + "Error, Host Name Not Found";
                    //Console.WriteLine(Data);
                    SharedData.DataDump[index] = Data;
                    index++; // Increment index for next entry
                    return;
                }
            }
        }
    }
    public class SharedData
    {
        public static int[] ArrayOctet4 = new int[254];
        public static string[] IPaddressFromArp;
        public static string[] DataDump;
    }
    /*public class ICMP
    {
        internal static string SendPing(string DNSAddress)
        {
            Ping SendAttempt = new Ping();
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            var timeout = 10;
            PingReply response = SendAttempt.Send(DNSAddress, timeout);
            if (response.Status == IPStatus.Success)
            {
                Console.WriteLine("Address: {0}", response.Address.ToString());
                Console.WriteLine("RoundTrip time: {0}", response.RoundtripTime);
                Console.WriteLine("Time to live: {0}", response.Options.Ttl);
                Console.WriteLine("Don't fragment: {0}", response.Options.DontFragment);
                Console.WriteLine("Buffer size: {0}", response.Buffer.Length);
            }
            else
            {
                Console.WriteLine(response.Status);
            }
            return DNSAddress;
        }
    }
    */ // Possible faster solution found. ICMP Scanning option will be considered for the future
    public class PS
    {
        public string[] RunArp(string DNSAddress)
        {
            string[] DNS = DNSAddress.Split('.');
            int octet1 = Int32.Parse(DNS[0]);
            int octet2 = Int32.Parse(DNS[1]);
            int octet3 = Int32.Parse(DNS[2]);
            int octet4 = Int32.Parse(DNS[3]);
            Process process = new Process();
            process.StartInfo.FileName = "powershell.exe";
            process.StartInfo.Arguments = "arp -a";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            try
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                string pattern = $@"\b({octet1})\.(?:25[0-4]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(?:25[0-4]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(?:25[0-4]|2[0-4][0-8]|[01]?[0-9][0-9]?)\b";
                MatchCollection matches = Regex.Matches(output, pattern);
                SharedData.IPaddressFromArp = new string[matches.Count];
                int index = 0;
                foreach (Match match in matches)
                {
                    SharedData.IPaddressFromArp[index++] = match.Value;
                }
                int ArrayLength = SharedData.IPaddressFromArp.Length;
                return SharedData.IPaddressFromArp;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
   /* internal class UdpBroadcast
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
            finally
            {
                Console.ReadLine();
                listener.Close();
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



