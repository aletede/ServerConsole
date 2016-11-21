using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ServerConsole
{
    class Program
    {
        private const int BACKLOG = 5;
        private const int BUFSIZE = 32;

        static void Main(string[] args)
        {
            int serverPort = 9000;

            Socket server = null;

            try
            {
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(new IPEndPoint(IPAddress.Loopback, serverPort));
                server.Listen(BACKLOG);
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.ErrorCode + ": " + se.Message);
                Environment.Exit(se.ErrorCode);
            }

            byte[] rcvBuffer = new byte[BUFSIZE];
            int byteRcvd;

            for (; ; )
            {
                Socket client = null;

                try
                {
                    Console.WriteLine("Waiting for client connection...");
                    client = server.Accept();
                    Console.WriteLine("Handling client at " + client.RemoteEndPoint + " - ");

                    int totalBytesRcvd = 0;
                    while ((byteRcvd = client.Receive(rcvBuffer, totalBytesRcvd, rcvBuffer.Length - totalBytesRcvd, SocketFlags.None)) != 0)
                    {
                        totalBytesRcvd += byteRcvd;
                    }
                    Console.WriteLine("Received {0} bytes from client: {1}", totalBytesRcvd, Encoding.ASCII.GetString(rcvBuffer, 0, totalBytesRcvd));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    client.Close();
                }
            }
        }
    }
}
