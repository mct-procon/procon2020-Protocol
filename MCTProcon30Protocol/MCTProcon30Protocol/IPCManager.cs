using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Diagnostics.Contracts;
using System.IO;
using MessagePack;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MCTProcon30Protocol
{
    public class IPCManager
    {
        static IPCManager()
        {
            MessagePackSerializer.DefaultOptions = MessagePackSerializerOptions.Standard.WithResolver(
                MessagePack.Resolvers.CompositeResolver.Create(new[] { new ColoredBoardNormalSmallerFormatter() }, new[] { MessagePack.Resolvers.StandardResolver.Instance })
                );
        }

        TcpListener listener;
        TcpClient client;
        NetworkStream stream;

        IIPCClientReader clientReader;
        IIPCServerReader serverReader;

        Process AIProcess = null;

        CancellationTokenSource Canceller;

        int _port = 0;
        bool isClient;

        public event Action<Exception> OnExceptionThrown;

        public bool IsReady => stream != null;

        public async Task StartAsync()
        {
            await ServerMainAction();
        }

        public async Task Connect(int port)
        {
            var ipAddress = Dns.GetHostEntry("localhost").AddressList[0];

            if (isClient)
                _port = port;
            else
            {
                listener = new TcpListener(ipAddress, port);
                listener.Start();
            }
            Canceller = new CancellationTokenSource();
            await Task.Run( async () =>
            { 
                begin:
                if (Canceller.Token.IsCancellationRequested)
                    return;
                try
                {
                    client = isClient ? new TcpClient("localhost", _port) : listener.AcceptTcpClient();
                }
                catch (SocketException ex)
                {
                    if (ex.ErrorCode != 10004)
                    {
                        await Task.Delay(200, Canceller.Token);
                        goto begin;
                    }
                    return;
                }
                stream = client.GetStream();
            }, Canceller.Token);
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

        public async Task ServerMainAction()
        {
            while(stream == null)
            {
                await Task.Delay(200);
            }
            client.ReceiveTimeout = Timeout.Infinite;

            int bufferSize = 0;
            int messageSize = 0;
            Methods.DataKind currentKind = 0;
            int current = 0;
            byte[] headBuffer = new byte[4];
            byte[] messageBuffer = new byte[1024];

            CancellationToken cancelToken = Canceller.Token;

            while (true)
            {
                try
                {
                    while (true)
                    {
                        bufferSize = await stream.ReadAsync(headBuffer, 0, 4, cancelToken);
                        if (Canceller.IsCancellationRequested)
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
                        bufferSize = await stream.ReadAsync(headBuffer, 0, 4, cancelToken);
                        if (Canceller.IsCancellationRequested)
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
                        bufferSize = await stream.ReadAsync(currentBuffer, current, messageSize, cancelToken);
                        current += bufferSize;
                        if (Canceller.IsCancellationRequested)
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
                                var Data = MessagePackSerializer.Deserialize<Methods.Connect>(currentBuffer);
                                AIProcess = Process.GetProcessById(Data.ProcessId);
                                AIProcess.EnableRaisingEvents = true;
                                AIProcess.Exited += __onAIProcessExited;
                                serverReader.OnConnect(Data);
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
                    if (ex is TimeoutException) continue;
                    if (ex is ObjectDisposedException) return;
                    if (ex.InnerException is SocketException && ((SocketException)(ex.InnerException)).ErrorCode == 10060) continue;
                    OnExceptionThrown?.Invoke(ex);
                    if (ex is IOException) return;
                }
            }
        }

        private void __onAIProcessExited(object sender, EventArgs e)
        {
            serverReader.OnAIProcessExited(serverReader, e);
        }

        public void Write<T>(Methods.DataKind datakind, T data)
        {
            try
            {
                byte[] message = MessagePackSerializer.Serialize(data);
                byte[] cache = BitConverter.GetBytes(message.Length);
                stream.Write(cache, 0, cache.Length);
                cache = BitConverter.GetBytes((int)datakind);
                stream.Write(cache, 0, cache.Length);
                stream.Write(message, 0, message.Length);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
                System.Diagnostics.Debugger.Break();
            }
        } 

        public void Shutdown()
        {
            Canceller?.Cancel();
            Thread.Sleep(800);
            stream?.Close();
            client?.Close();
            listener?.Stop();
            Canceller.Dispose();
            if (AIProcess != null)
                AIProcess.Exited -= __onAIProcessExited;
        }

    }
}
