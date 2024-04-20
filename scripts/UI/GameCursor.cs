using Godot;
using Scene;
using System;

namespace UI;

public partial class GameCursor : Node2D
{
	private MainScene _mainScene;
	public MainScene MainScene
	{
		get
		{
			_mainScene ??= GetTree().Root.GetNode<MainScene>("MainScene");
			return _mainScene;
		}
	}

	private Area2D _collisionArea;
	public Area2D CollisionArea
	{
		get
		{
			_collisionArea ??= GetNode<Area2D>("CollisionArea");
			return _collisionArea;
		}
	}

	private CollisionShape2D _collisionShape;
	public CollisionShape2D CollisionShape
	{
		get
		{
			_collisionShape ??= GetNode<CollisionShape2D>("CollisionShape");
			return _collisionShape;
		}
	}

	public override void _Ready()
	{
		base._Ready();
		TopLevel = true;
		ZIndex = 200;
		MainScene.Ready += () =>
		{
			Input.MouseMode = Input.MouseModeEnum.Hidden;
		};
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		GlobalPosition = GetGlobalMousePosition();
	}
}
