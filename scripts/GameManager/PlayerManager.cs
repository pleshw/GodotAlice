using Entity;
using Godot;
using System;

namespace GameManager;

public partial class PlayerManager : Node
{
	protected Vector2 playerLastSavedPoint = new()
	{
		X = 0,
		Y = 0
	};

	PackedScene PlayerPrefab = ResourceLoader.Load("res://prefabs/player.tscn") as PackedScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SpawnPlayerAtNearestCellFrom(playerLastSavedPoint);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SpawnPlayerAtNearestCellFrom(Vector2 position)
	{
		// Instantiate the player scene
		Player playerInstance = PlayerPrefab.Instantiate() as Player;

		// Set player's initial position
		playerInstance.initialPosition = position;

		// Add player to the scene
		GetTree().Root.CallDeferred("add_child", playerInstance);
	}
}
