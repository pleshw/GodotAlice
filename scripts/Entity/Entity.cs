using System.Collections.Generic;
using System.Linq;
using Entity.Commands.Movement;
using Godot;

namespace Entity;

public partial class Entity : EntityMovement
{
	public AnimatedSprite2D sprite;
	public DIRECTIONS facing = DIRECTIONS.BOTTOM;
	public MovementCommandKeybind movementKeyBind;
	public HashSet<Key> keysPressed = [];

	public Entity() : base(new Vector2 { X = 0, Y = 0 })
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
		sprite = GetNode<AnimatedSprite2D>("Sprite");
		TeleportTo(new PlayerMovementInput
		{
			Position = initialPosition,
			IsRunning = false,
			ForceMovementState = true,
			MovementState = MOVEMENT_STATE.IDLE,
		});

		movementKeyBind.BindDefaults();
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
