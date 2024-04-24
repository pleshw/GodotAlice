
using System;
using Extras;
using GameManager;
using Godot;
using UI;

namespace Scene;

public partial class StageLoader(Node parent) : GameNodeManagerBase<Node2D>()
{
	public readonly Random Random = new();

	public Node Parent = parent;

	public PlayerManager PlayerManager
	{
		get
		{
			return Parent.GetNode<PlayerManager>("/root/PlayerManager");
		}
	}

	public InputManager InputManager
	{
		get
		{
			return Parent.GetNode<InputManager>("/root/InputManager");
		}
	}

	public SceneManager SceneManager
	{
		get
		{
			return Parent.GetNode<SceneManager>("/root/SceneManager");
		}
	}

	private GameCursor _currentCursor;

	public GameCursor CurrentCursor
	{
		get
		{
			_currentCursor ??= Parent.GetNode<GameCursor>("DefaultCursor");
			return _currentCursor;
		}
	}
}
