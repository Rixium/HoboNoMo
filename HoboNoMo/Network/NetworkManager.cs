using System;
using System.Collections.Generic;
using System.Linq;
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
            CreatePlayer,
            NewConnectionPlayer
        }

        public bool IsServer => _server != null;
        public bool IsClient => _client != null;
        
        private NetServer _server;
        private NetClient _client;

        private NetPeerConfiguration _config;
        private Player _myPlayer;

        private string AppIdentifier { get; } = Game1.GameTitle + Game1.Version;

        public string IpAddress =>
            IsServer ? _server?.Configuration?.BroadcastAddress?.ToString() : _client?.Configuration?.BroadcastAddress?.ToString();
        
        public int Port => 27411;
        public Action<Player> OnPlayerAdded { get; set; }

        public List<Player> Players { get; set; } = new List<Player>();
        public Action OnConnected { get; set; }

        public Player AddPlayer(Player player, bool isMine = false)
        {
            Players.Add(player);
            OnPlayerAdded?.Invoke(player);

            if (isMine)
                _myPlayer = player;
            
            return player;
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
                switch (first)
                {
                    case MessageType.RequestPlayers:
                        break;
                    case MessageType.PlayerInfo:
                        var playerId = msg.ReadString();
                        if (PlayerExists(playerId)) continue;
                        var playerName = msg.ReadString();
                        Players.Add(new Player
                        {
                            Id = new Guid(playerId),
                            Name = playerName
                        });
                        break;
                    case MessageType.CreatePlayer:
                        break;
                    case MessageType.NewConnectionPlayer:
                        var myPlayerId = msg.ReadString();
                        if (PlayerExists(myPlayerId)) continue;
                        var myPlayerName = msg.ReadString();
                        _myPlayer = AddPlayer(new Player
                        {
                            Id = new Guid(myPlayerId),
                            Name = myPlayerName
                        });
                        
                        var getAllPlayers = _client.CreateMessage();
                        getAllPlayers.Write((int)MessageType.RequestPlayers);
                        _client.SendMessage(getAllPlayers, NetDeliveryMethod.ReliableOrdered);
                        
                        break;
                }
            }
        }

        private bool PlayerExists(string playerId) => Players.FirstOrDefault(p => p.Id.ToString().Equals(playerId, StringComparison.Ordinal)) != null;

        private void ProcessServer()
        {
            NetIncomingMessage msg;
            while ((msg = _server.ReadMessage()) != null)
            {
                var first = (MessageType) msg.ReadInt32();

                switch (first)
                {
                    case MessageType.RequestPlayers:
                        foreach (var player in Players)
                        {
                            var outgoing = _server.CreateMessage();
                            outgoing.Write((int) MessageType.PlayerInfo);
                            outgoing.Write(player.Id.ToString());
                            outgoing.Write(player.Name);
                            _server.SendMessage(outgoing, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                        }
                        break;
                    case MessageType.PlayerInfo:
                        break;
                    case MessageType.CreatePlayer:
                        var newPlayer = AddPlayer(new Player
                        {
                            Id = Guid.NewGuid(),
                            Name = $"Player{Players.Count + 1}"
                        });
                        
                        var newPlayerMessage = _server.CreateMessage();
                        newPlayerMessage.Write((int) MessageType.NewConnectionPlayer);
                        newPlayerMessage.Write(newPlayer.Id.ToString());
                        newPlayerMessage.Write(newPlayer.Name);
                        _server.SendMessage(newPlayerMessage, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                        break;
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
            
            _client.Connect(ip, port);
            
            Thread.Sleep(500);
            
            OnConnected?.Invoke();
            
            var msg = _client.CreateMessage();
            msg.Write((int)MessageType.CreatePlayer);
        
            _client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void Disconnect()
        {
            _server?.Shutdown("Server shutting down");
            _client?.Shutdown("Client shutting down");
        }

        public Player GetMyPlayer()
        {
            return _myPlayer;
        }
    }
}