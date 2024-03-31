using Godot;
using System;

namespace Entity;


public record struct PlayerMovementInput
{
  public required Vector2 Position;
  public required bool IsRunning;
  public required bool ForceMovementState;
  public required MOVEMENT_STATE MovementState;
}

public partial class EntityMovement(Vector2 initialPosition, int gridMapCellWidth = 30) : Node2D
{
  public readonly Vector2 initialPosition = initialPosition;

  protected Vector2? _lastTrackedPosition;


  /// <summary>
  /// What is the movement state of the player. Helper to identify the best way to draw the player.
  /// </summary>
  protected MOVEMENT_STATE _lastMovementState = EntityDefaults.MovementState;

  /// <summary>
  /// What is the movement state of the player. Helper to identify the best way to draw the player.
  /// </summary>
  protected MOVEMENT_STATE _movementState = EntityDefaults.MovementState;

  /// <summary>
  /// Where player is going.
  /// </summary>
  protected Vector2? _targetPosition;

  /// <summary>
  /// Player movement speed in grid cells per second.
  /// </summary>
  protected int _moveSpeed = EntityDefaults.MovementSpeed;

  /// <summary>
  /// Grid cell width used for speed reference on player movement.
  /// </summary>
  protected int _cellWidth = gridMapCellWidth;

  public bool HasTargetPosition
  {
    get
    {
      return _targetPosition != null;
    }
  }

  public Vector2 TargetPosition
  {
    get
    {
      return _targetPosition ?? Position;
    }

    set
    {
      _targetPosition = value;
    }
  }

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

  /// <summary>
  /// How much the player moves per interaction.
  /// </summary>
  public int StepSize
  {
    get
    {
      return _cellWidth;
    }
  }

  public int MoveSpeed
  {
    get
    {
      return _moveSpeed;
    }
  }

  public EntityMovement ControlMovementState(PlayerMovementInput playerMovementInput)
  {
    if (playerMovementInput.ForceMovementState)
    {
      _lastMovementState = _movementState;
      _movementState = playerMovementInput.MovementState;
      return this;
    }

    return this;
  }

  public EntityMovement TeleportTo(PlayerMovementInput playerMovementInput)
  {
    ControlMovementState(playerMovementInput);
    Position = playerMovementInput.Position;
    return this;
  }

  public EntityMovement MoveTo(PlayerMovementInput playerMovementInput)
  {
    ControlMovementState(playerMovementInput);
    _targetPosition = playerMovementInput.Position;
    return this;
  }

  public EntityMovement MoveToNearestCell(PlayerMovementInput playerMovementInput)
  {
    ControlMovementState(playerMovementInput);
    _targetPosition = new Vector2
    {
      X = Mathf.Round(playerMovementInput.Position.X / _cellWidth) * _cellWidth + HalfCellWidth,
      Y = Mathf.Round(playerMovementInput.Position.Y / _cellWidth) * _cellWidth + HalfCellWidth
    };

    return this;
  }

  public EntityMovement TeleportToNearestCell(PlayerMovementInput playerMovementInput)
  {
    ControlMovementState(playerMovementInput);
    Position = new Vector2
    {
      X = Mathf.Round(playerMovementInput.Position.X / _cellWidth) * _cellWidth + HalfCellWidth,
      Y = Mathf.Round(playerMovementInput.Position.Y / _cellWidth) * _cellWidth + HalfCellWidth
    };

    return this;
  }


  protected EntityMovement DefaultMovementProcess(double delta, out bool hasMoved)
  {
    hasMoved = false;
    if (_targetPosition == null)
    {
      return this;
    }

    // Calculate the direction vector towards the target position
    Vector2 direction = (TargetPosition - Position).Normalized();

    float distanceToMove = StepSize * MoveSpeed * (float)delta;

    if (Position.DistanceTo(TargetPosition) <= distanceToMove)
    {
      Position = TargetPosition;
      _lastTrackedPosition = TargetPosition;
      _targetPosition = null;
    }
    else
    {
      Position += direction * distanceToMove;
    }

    hasMoved = true;
    return this;
  }
}