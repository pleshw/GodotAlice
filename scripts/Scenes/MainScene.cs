
using GameManagers;
using Godot;

namespace Scene;

public partial class MainScene : Node2D
{

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

	public override void _Ready()
	{
		base._Ready();

		PlayerManager.InstantiatePlayer();
		EnemyManager.InstantiateEnemies();
	}
}
