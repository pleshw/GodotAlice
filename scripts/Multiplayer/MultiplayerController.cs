using Entity;
using Extras;
using Godot;
using System;
using System.Collections.Generic;

namespace Multiplayer;

public partial class MultiplayerController : Node
{
	public bool IsHost = false;
	public long Id = -1;

	public readonly Dictionary<long, StringName> PlayerNameByConnectionId = [];
	public readonly Dictionary<StringName, Player> PlayerCharacterByName = [];
	private readonly HashSet<long> ClientPeers = [];

	private ENetMultiplayerPeer _hostPeer;
	public ENetMultiplayerPeer HostPeer
	{
		get
		{
			_hostPeer ??= new();
			return _hostPeer;
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Multiplayer.PeerConnected += PeerConnectedEvent;
		Multiplayer.PeerDisconnected += PeerDisconnectedEvent;
		Multiplayer.ConnectedToServer += ConnectedToServerEvent;
		Multiplayer.ConnectionFailed += ConnectionFailedEvent;
	}

	public bool TryCreateLobby(out Error err)
	{
		err = HostPeer.CreateServer(NetworkController.DefaultPort, 8);
		if (err is not Error.Ok)
		{
			GD.Print("ConnectionError: " + err.ToString());
			return false;
		}

		HostPeer.Host.Compress(ENetConnection.CompressionMode.Fastlz);

		Multiplayer.MultiplayerPeer = HostPeer;
		IsHost = true;

		GD.Print("Hosting Server");

		LobbyHostedEvent();

		return true;
	}

	public bool TryJoinLobby()
	{
		ENetMultiplayerPeer peer = new();
		var err = peer.CreateClient(NetworkController.Address, NetworkController.DefaultPort, 8);

		if (err is not Error.Ok)
		{
			GD.Print("ConnectionError: " + err.ToString());
			return false;
		}

		peer.Host.Compress(ENetConnection.CompressionMode.Fastlz);

		Multiplayer.MultiplayerPeer = peer;

		GD.Print("Connected to server.");

		LobbyJoinedEvent();

		return true;
	}

	public void PeerConnectedEvent(long id)
	{
		if (id != 1)
		{
			ClientPeers.Add(id);
			GD.Print($"A new peer with id:{id} connected to the server");
			return;
		}
	}

	public void PeerDisconnectedEvent(long id)
	{
		if (id == 1)
		{
			GD.Print("Server has disconnected");
			ClientPeers.Clear();
		}
		else
		{
			GD.Print($"The peer with id:{id} disconnected from the server");
		}
	}

	public void ConnectedToServerEvent()
	{
	}

	public void ConnectionFailedEvent()
	{

	}
}
