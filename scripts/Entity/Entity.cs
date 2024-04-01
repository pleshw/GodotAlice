using System.Collections.Generic;
using System.Linq;
using Entity.Commands.Movement;
using Godot;

namespace Entity;

public partial class Entity : EntityMovement, IEntityBaseNode
{
	public Camera2D Camera { get; set; }
	public AnimatedSprite2D Sprite { get; set; }
	public CharacterBody2D CollisionBody { get; set; }
	public CollisionShape2D[] CollisionShapes { get; set; }

	public DIRECTIONS facing = DIRECTIONS.BOTTOM;
	public MovementCommandKeybind movementKeyBind;
	public HashSet<Key> keysPressed = [];

	public Entity() : base(Vector2.Zero)
	{
		movementKeyBind = new MovementCommandKeybind(this);
	}

	public Entity(Vector2 initialPosition) : base(initialPosition)
	{
		movementKeyBind = new MovementCommandKeybind(this);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		Camera = GetNode<Camera2D>("Camera");
		Sprite = GetNode<AnimatedSprite2D>("Sprite");
		CollisionBody = GetNode<CharacterBody2D>("CollisionBody");
		CollisionShapes = CollisionBody.GetChildren().Select(c => c as CollisionShape2D).ToArray();
	}


	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey inputEventKey)
		{
			if (inputEventKey.Pressed)
			{
				keysPressed.Add(inputEventKey.Keycode);
			}
			else
			{
				keysPressed.Remove(inputEventKey.Keycode);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		foreach (var key in keysPressed)
		{
			movementKeyBind.Execute(key);
		}

		DefaultMovementProcess(delta, out _);
	}
}
