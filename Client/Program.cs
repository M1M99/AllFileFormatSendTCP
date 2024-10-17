using System.Net;
using System.Net.Sockets;
using System.Text;

class TcpClientProj
{
    static void Main(string[] args)
    {
        var endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 58758);//change port
        using (var client = new TcpClient())
        {
            client.Connect(endPoint);
            if (client.Connected)
            {
                using (var stream = client.GetStream())
                {
                    var path = @"C:\Users\ASUS\Desktop\MAUI.txt";

                    if (File.Exists(path))
                    {
                        var file = new FileInfo(path);

                        var fileNameBytes = Encoding.UTF8.GetBytes(file.Name);
                        stream.Write(fileNameBytes, 0, fileNameBytes.Length);

                        var fileLengthBytes = Encoding.UTF8.GetBytes(file.Length.ToString());
                        stream.Write(fileLengthBytes, 0, fileLengthBytes.Length);

                        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            var bytes = new byte[1024];
                            int len;
                            while ((len = fs.Read(bytes, 0, bytes.Length)) > 0)
                            {
                                stream.Write(bytes, 0, len);
                            }
                        }
                        Console.WriteLine($"File sent: {file.Name}");
                    }
                    else
                    {
                        Console.WriteLine($"File not found: {path}");
                    }
                }
            }
            else { Console.WriteLine("Try Again Not Connected"); }
        }
    }
}
