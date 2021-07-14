using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Client;
using Client.Data;

namespace ghostrootParadise
{
    class Program
    {
        private static Socket socket;
        public static void ComradeShip(string comrade)
        {
            //Socket oluşturma ve lokal bazlı dinleme için gerekli tanımlamalar
            //ipv6 kullanılıyor
            Socket socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 14568); // lokal bazlı dinleme için 127.0.0.1, WAN bazlı dinleme için 0.0.0.0
            Byte[] data = Encoding.ASCII.GetBytes(comrade.Ghoster()); // Aes ile şifreli bir şekilde komut gönderimi
            socket.SendTo(data, iPEndPoint);
        }
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Paradise ! ");
            Console.WriteLine("Press ESC to exit");
            Console.WriteLine("______________________________________________");
            Console.WriteLine("");
            Console.WriteLine("");
        
            socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            socket.Bind(new IPEndPoint(IPAddress.IPv6Any, 14567));

            Task task = Task.Factory.StartNew(Listener);
        ComradeShip:
            Console.WriteLine("Enter Command : ");
                ComradeShip(Console.ReadLine());
           
            goto ComradeShip;

        }

        public  static void Listener()
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.IPv6Any, 0);
            EndPoint endPoint = iPEndPoint as EndPoint;
            Byte[] data = new Byte[65535];
            socket.ReceiveFrom( data, ref endPoint);
            List<Byte> bytes =  new List<Byte>(data);
            bytes.RemoveAll(b => b == 0);
            Console.WriteLine("Got str : " + Encoding.ASCII.GetString(bytes.ToArray()).DeGhoster());
           
            Listener();

        }


    }
}
