using System;
using System.Collections.Generic;
using System.Threading;
using Lidgren.Network;

namespace HoboNoMo.Network
{
    public class NetworkManager
    {
        public enum MessageType
        {
            RequestPlayers,
            PlayerInfo,
        }

        public bool IsServer => _server != null;
        public bool IsClient => _client != null;
        
        private NetServer _server;
        private NetClient _client;

        private NetPeerConfiguration _config;

        private string AppIdentifier { get; } = Game1.GameTitle + Game1.Version;

        public string IpAddress =>
            IsServer ? _server?.Configuration?.BroadcastAddress?.ToString() : _client?.Configuration?.BroadcastAddress?.ToString();
        
        public int Port => 27411;
        public Action<Player> OnPlayerAdded { get; set; }

        public List<Player> Players { get; set; } = new List<Player>();
        public Action OnConnected { get; set; }

        public void AddPlayer(Player player)
        {
            Players.Add(player);
            OnPlayerAdded?.Invoke(player);
        }

        public void Update(float delta)
        {
            if (_server == null)
            {
                ProcessClient();
            }
            else ProcessServer();
        }

        private void ProcessClient()
        {
            NetIncomingMessage msg;
            while ((msg = _client.ReadMessage()) != null)
            {
                var first = (MessageType) msg.ReadInt32();
                if (first == MessageType.PlayerInfo)
                {
                    var playerId = msg.ReadString();
                    var playerName = msg.ReadString();
                    Players.Add(new Player
                    {
                        Id = new Guid(playerId),
                        Name = playerName
                    });
                }
            }
        }

        private void ProcessServer()
        {
            NetIncomingMessage msg;
            while ((msg = _server.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.Error:
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        break;
                    case NetIncomingMessageType.UnconnectedData:
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        break;
                    case NetIncomingMessageType.Data:
                        break;
                    case NetIncomingMessageType.Receipt:
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                        break;
                    case NetIncomingMessageType.DebugMessage:
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        break;
                    case NetIncomingMessageType.NatIntroductionSuccess:
                        break;
                    case NetIncomingMessageType.ConnectionLatencyUpdated:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                var first = (MessageType) msg.ReadInt32();
                
                if (first == MessageType.RequestPlayers)
                {
                    foreach (var player in Players)
                    {
                        var outgoing = _server.CreateMessage();
                        outgoing.Write((int) MessageType.PlayerInfo);
                        outgoing.Write(player.Id.ToString());
                        outgoing.Write(player.Name);
                        _server.SendMessage(outgoing, msg.SenderConnection, NetDeliveryMethod.Unreliable);
                    }
                }
            }
        }

        public void Create()
        {
            _config = new NetPeerConfiguration(AppIdentifier)
            {
                Port = Port
            };
            _server = new NetServer(_config);

            _server.Start();
        }

        public void Join(string ip, int port)
        {
            _config = new NetPeerConfiguration(AppIdentifier);

            _client = new NetClient(_config);
            _client.Start();
            
            Thread.Sleep(500);
            
            _client.Connect("localhost", port);
            
            Thread.Sleep(500);
            
            OnConnected?.Invoke();
            
            var msg = _client.CreateMessage();
            msg.Write((int)MessageType.RequestPlayers);
        
            _client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void Disconnect()
        {
            _server?.Shutdown("Server shutting down");
            _client?.Shutdown("Client shutting down");
        }
    }
}