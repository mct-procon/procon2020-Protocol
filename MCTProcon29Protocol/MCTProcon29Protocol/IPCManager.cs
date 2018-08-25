using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Diagnostics.Contracts;
using System.IO;
using MessagePack;

namespace MCTProcon29Protocol
{
    public class IPCManager
    {
        TcpListener listener;
        TcpClient client;
        NetworkStream stream;
        Thread IPCThread;     // このスレッドは頻繁に作成・削除されない．

        IIPCClientReader clientReader;
        IIPCServerReader serverReader;

        Queue<byte[]> writeQueue = new Queue<byte[]>();


        int _port = 0;
        bool isClient;
        bool isStopRequired = false;

        public event Action<Exception> OnExceptionThrown;

        public void Start(int port)
        {
            var ipAddress = Dns.GetHostEntry("localhost").AddressList[0];

            if (isClient)
                _port = port;
            else
            {
                listener = new TcpListener(ipAddress, port);
                listener.Start();
            }
            IPCThread = new Thread(ServerMainAction);
            IPCThread.Start();
        }

        public IPCManager(IIPCClientReader client)
        {
            Contract.Requires(client != null);
            isClient = true;
            this.clientReader = client;
        }

        public IPCManager(IIPCServerReader server)
        {
            Contract.Requires(server != null);
            isClient = false;
            this.serverReader = server;
        }

        public void ServerMainAction()
        {
            try
            {
                client = isClient ? new TcpClient("localhost", _port) : listener.AcceptTcpClient();
            }catch(SocketException ex)
            {
                if (ex.ErrorCode != 10004)
                    System.Diagnostics.Debugger.Break();
            }
            stream = client.GetStream();
            stream.ReadTimeout = 800;

            int bufferSize = 0;
            int messageSize = 0;
            Methods.DataKind currentKind = 0;
            int current = 0;
            byte[] headBuffer = new byte[4];
            byte[] messageBuffer = new byte[1024];
            while (true)
            {
                try
                {
                    while (true)
                    {
                        bufferSize = stream.Read(headBuffer, 0, 4);
                        if (isStopRequired)
                            return;
                        if (bufferSize == 4)
                        {
                            messageSize = BitConverter.ToInt32(headBuffer, 0); // little endian
                            current = 0;
                            goto data_kind_read_start;
                        }
                    }
                    data_kind_read_start:
                    while (true)
                    {
                        bufferSize = stream.Read(headBuffer, 0, 4);
                        if (isStopRequired)
                            return;
                        if (bufferSize == 4)
                        {
                            currentKind = (Methods.DataKind)BitConverter.ToInt32(headBuffer, 0); // little endian
                            goto message_read_start;
                        }
                    }
                    message_read_start:

                    byte[] currentBuffer = messageBuffer.Length < messageSize ? new byte[messageSize] : messageBuffer;
                    while (current < messageSize)
                    {
                        bufferSize = stream.Read(currentBuffer, current, messageSize);
                        current += bufferSize;
                        if (isStopRequired)
                            return;
                    }
                    if (isClient)
                    {
                        switch (currentKind)
                        {
                            case Methods.DataKind.GameInit:
                                clientReader.OnGameInit(MessagePackSerializer.Deserialize<Methods.GameInit>(currentBuffer));
                                break;
                            case Methods.DataKind.TurnStart:
                                clientReader.OnTurnStart(MessagePackSerializer.Deserialize<Methods.TurnStart>(currentBuffer));
                                break;
                            case Methods.DataKind.TurnEnd:
                                clientReader.OnTurnEnd(MessagePackSerializer.Deserialize<Methods.TurnEnd>(currentBuffer));
                                break;
                            case Methods.DataKind.GameEnd:
                                clientReader.OnGameEnd(MessagePackSerializer.Deserialize<Methods.GameEnd>(currentBuffer));
                                break;
                            case Methods.DataKind.Pause:
                                clientReader.OnPause(MessagePackSerializer.Deserialize<Methods.Pause>(currentBuffer));
                                break;
                            case Methods.DataKind.Interrupt:
                                clientReader.OnInterrupt(MessagePackSerializer.Deserialize<Methods.Interrupt>(currentBuffer));
                                break;
                            case Methods.DataKind.RebaseByUser:
                                clientReader.OnRebaseByUser(MessagePackSerializer.Deserialize<Methods.RebaseByUser>(currentBuffer));
                                break;
                            default:
                                throw new FormatException();
                        }
                    }
                    else
                    {
                        switch(currentKind)
                        {
                            case Methods.DataKind.Connect:
                                serverReader.OnConnect(MessagePackSerializer.Deserialize<Methods.Connect>(currentBuffer));
                                break;
                            case Methods.DataKind.Decided:
                                serverReader.OnDecided(MessagePackSerializer.Deserialize<Methods.Decided>(currentBuffer));
                                break;
                            case Methods.DataKind.Interrupt:
                                serverReader.OnInterrupt(MessagePackSerializer.Deserialize<Methods.Interrupt>(currentBuffer));
                                break;
                            default:
                                throw new FormatException();
                        }
                    }
                }
                catch (Exception ex)
                {
                    OnExceptionThrown?.Invoke(ex);
                }
            }
        }

        public void Write<T>(Methods.DataKind datakind, T data)
        {
            byte[] message = MessagePackSerializer.Serialize(data);
            byte[] cache = BitConverter.GetBytes(message.Length);
            stream.Write(cache, 0, cache.Length);
            cache = BitConverter.GetBytes((int)datakind);
            stream.Write(cache, 0, cache.Length);
            stream.Write(message, 0, message.Length);
        }

        public void ShutdownServer()
        {
            isStopRequired = true;
            Thread.Sleep(800);
            stream?.Close();
            client?.Close();
            listener?.Stop();
        }
    }
}
