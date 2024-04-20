
using System;
using GameManager;
using Godot;
using UI;

namespace Scene;

public partial class MainScene : Node2D
{
	public readonly Random Random = new();

	public PlayerManager PlayerManager
	{
		get
		{
			return GetNode<PlayerManager>("/root/PlayerManager");
		}
	}

	public EnemyManager EnemyManager
	{
		get
		{
			return GetNode<EnemyManager>("/root/EnemyManager");
		}
	}

	public InputManager InputManager
	{
		get
		{
			return GetNode<InputManager>("/root/InputManager");
		}
	}

	private GameCursor _currentCursor;
	public GameCursor CurrentCursor
	{
		get
		{
			_currentCursor ??= GetNode<GameCursor>("DefaultCursor");
			return _currentCursor;
		}
	}

	public override void _Ready()
	{
		base._Ready();

		PlayerManager.InstantiatePlayer();
		GetWindow().GrabFocus();
	}
}
