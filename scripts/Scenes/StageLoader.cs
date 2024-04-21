
using System;
using Extras;
using GameManager;
using Godot;
using UI;

namespace Scene;

public partial class StageLoader() : GameResourceManager<Node2D>(GodotFolderPath.Stages, "stage_1.tscn")
{
	public readonly Random Random = new();

	public PlayerManager PlayerManager
	{
		get
		{
			return GetNode<PlayerManager>("/root/PlayerManager");
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
	}
}
