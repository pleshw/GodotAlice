using Godot;
using System;

namespace Alice;


public partial class Player(Vector2 initialPosition) : Node2D
{
	public readonly Vector2 initialPosition = initialPosition;

	/// <summary>
	/// Player movement speed in grid cells per second.
	/// </summary>
	protected int _moveSpeed = 4;

	/// <summary>
	/// Grid cell width used for speed reference on player movement.
	/// </summary>
	protected int _cellWidth = 64;

	protected Vector2? _lastTrackedPosition;

	/// <summary>
	/// Where player is going.
	/// </summary>
	protected Vector2? _targetPosition;

	/// <summary>
	/// What is the movement state of the player. Helper to identify the best way to draw the player.
	/// </summary>
	protected Entity.MOVEMENT_STATE _movementState = Entity.MOVEMENT_STATE.IDLE;

	public AnimatedSprite2D sprite;

	/// <summary>
	/// Half grid cell width. Used as reference to get the center of a cell, for example. 
	/// </summary>
	protected int HalfCellWidth
	{
		get
		{
			return _cellWidth / 2;
		}
	}

	public Vector2 TargetPosition
	{
		get
		{
			if (_targetPosition is null)
			{
				throw new Exception("Invalid target position.");
			}

			return (Vector2)_targetPosition;
		}
	}

	/// <summary>
	/// How much the player moves per interaction.
	/// </summary>
	public int StepSize
	{
		get
		{
			return _moveSpeed * _cellWidth;
		}
	}

	public int MoveSpeed
	{
		get
		{
			return _moveSpeed;
		}
	}

	public Player() : this(new Vector2 { X = 0, Y = 0 })
	{

	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("Sprite");
	}


	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
		{
			// Calculate the target position based on the grid cell
			Vector2 clickedPosition = GetGlobalMousePosition();
			_targetPosition = new Vector2
			{
				X = Mathf.Round(clickedPosition.X / _cellWidth) * _cellWidth + HalfCellWidth,
				Y = Mathf.Round(clickedPosition.Y / _cellWidth) * _cellWidth + HalfCellWidth
			};
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_targetPosition != null)
		{
			// Calculate the direction vector towards the target position
			Vector2 direction = (TargetPosition - Position).Normalized();
			// Calculate the distance to move this frame based on the moveSpeed
			float distanceToMove = StepSize * (float)delta;
			// Check if we're close enough to the target position to snap to it
			if (Position.DistanceTo(TargetPosition) <= distanceToMove)
			{
				Position = TargetPosition;
				_lastTrackedPosition = TargetPosition;
				_targetPosition = null;
			}
			else
			{
				// Move towards the target position
				Position += direction * distanceToMove;
			}
		}
	}
}
