using Godot;
using System;

namespace Entity;

public partial class Player : EntityMovement
{
	/// <summary>
	/// What is the movement state of the player. Helper to identify the best way to draw the player.
	/// </summary>
	protected MOVEMENT_STATE _movementState = EntityDefaults.MovementState;

	public AnimatedSprite2D sprite;

	public Player() : base(new Vector2 { X = 0, Y = 0 }) { }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("Sprite");
		TeleportTo(initialPosition);
	}


	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
		{
			// Calculate the target position based on the grid cell
			Vector2 clickedPosition = GetGlobalMousePosition();
			MoveTo(clickedPosition);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		DefaultMovementProcess(delta, out _);
	}
}
