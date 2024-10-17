using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

class TcpServer
{
    static void Main(string[] args)
    {
        var server = new TcpListener(IPAddress.Parse("127.0.0.1"), 58758);
        server.Start();
        Console.WriteLine("Server is running...");

        while (true)
        {
            using (var client = server.AcceptTcpClient())
            using (var stream = client.GetStream())
            {
                var fileNameBuffer = new byte[1024];
                int fileNameLength = stream.Read(fileNameBuffer, 0, fileNameBuffer.Length);
                var fileName = Encoding.UTF8.GetString(fileNameBuffer, 0, fileNameLength);

                var fileLengthBuffer = new byte[1024];
                int fileLengthBytes = stream.Read(fileLengthBuffer, 0, fileLengthBuffer.Length);
                int fileLength = int.Parse(Encoding.UTF8.GetString(fileLengthBuffer, 0, fileLengthBytes));

                var path1 = "C:\\Users\\ASUS\\Desktop\\NewFolderForAllFiles"; //Change Path For You Comp
                if (!Directory.Exists(path1)) {
                    Directory.CreateDirectory(path1);
                }

                var path = $"{path1}\\{DateTime.Now:HH.mm.ss}{Path.GetExtension(fileName)}";

                using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    var totalReceivedBytes = 0;
                    var buffer = new byte[1024];

                    while (totalReceivedBytes < fileLength)
                    {
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        fileStream.Write(buffer, 0, bytesRead);
                        totalReceivedBytes += bytesRead;
                    }
                }

                Console.WriteLine($"File downloaded: {path}");
            }
        }
    }
}
