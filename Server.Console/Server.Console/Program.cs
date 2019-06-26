using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using Server.DataAcces;

namespace ServerConsole
{
    class Program
    {
        private static TcpListener _socketServer;
        private static Thread _threadServer;
        private static ManualResetEvent _eventStop;

        static void Main(string[] args)
        {
            _eventStop = new ManualResetEvent(false);
            _socketServer = null;
            _threadServer = null;

            try
            {
                _socketServer = new TcpListener(IPAddress.Parse("0.0.0.0"), 12345);
                _socketServer.Start(100);
                _threadServer = new Thread(ServerThreadProcedure);
                _threadServer.Start(_socketServer);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return;
            }
        }

        private static void ServerThreadProcedure(object socket)
        {
            while (true)
            {
                try
                {
                    TcpListener serverSocket = (TcpListener)socket;
                    var result = serverSocket.BeginAcceptTcpClient(AcceptClientProcedure, serverSocket);
                    while (result.AsyncWaitHandle.WaitOne(200) == false)
                    {
                        if (_eventStop.WaitOne(0) == true)
                        {
                            _eventStop.Reset();
                            return;
                        }
                    }

                }
                catch (Exception exception)
                {
                    _eventStop.Reset();
                    Console.WriteLine(exception.Message);
                    return;
                }
            }
        }

        private static void AcceptClientProcedure(IAsyncResult ar)
        {
            TcpListener serverSocket = (TcpListener)ar.AsyncState;
            TcpClient client = serverSocket.EndAcceptTcpClient(ar);

            Console.WriteLine("Client is conected");
            Console.WriteLine("Client's address: " + client.Client.RemoteEndPoint.ToString());

            ThreadPool.QueueUserWorkItem(ClientThreadProcedure, client);
        }

        private static void ClientThreadProcedure(object obj)
        {
            TcpClient clientSocket = (TcpClient)obj;
            Console.WriteLine("Client's thread is started");
            byte[] buffer = new byte[4 * 1024];

            while (true)
            {
                try
                {
                    int recSize = clientSocket.Client.Receive(buffer);
                    string message = Encoding.UTF8.GetString(buffer, 0, recSize);

                    using (var context = new DataContext())
                    {
                        if (context.Cities.Where(c => c.Name == message).FirstOrDefault() == null)
                        {
                            clientSocket.Client.Send(Encoding.ASCII.GetBytes("City is not found"));
                        }
                        else
                        {
                            foreach (var street in context.Cities.Where(c => c.Name == message).FirstOrDefault().Streets.ToList())
                            {
                                clientSocket.Client.Send(Encoding.ASCII.GetBytes(street.Name));
                            }
                        }
                    }
                    if (recSize <= 0) break;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
                
            }
        }
    }
}
