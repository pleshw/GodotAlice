using Godot;
using System;

namespace Entity;


public partial class EntityMovement(Vector2 initialPosition, int gridMapCellWidth = 64) : Node2D
{
  public readonly Vector2 initialPosition = initialPosition;

  protected Vector2? _lastTrackedPosition;

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

  public EntityMovement TeleportTo(Vector2 position)
  {
    Position = position;
    return this;
  }

  public EntityMovement MoveTo(Vector2 position)
  {
    _targetPosition = position;
    return this;
  }

  public EntityMovement MoveToNearestCell(Vector2 position)
  {
    _targetPosition = new Vector2
    {
      X = Mathf.Round(position.X / _cellWidth) * _cellWidth + HalfCellWidth,
      Y = Mathf.Round(position.Y / _cellWidth) * _cellWidth + HalfCellWidth
    };

    return this;
  }

  public EntityMovement TeleportToNearestCell(Vector2 position)
  {
    Position = new Vector2
    {
      X = Mathf.Round(position.X / _cellWidth) * _cellWidth + HalfCellWidth,
      Y = Mathf.Round(position.Y / _cellWidth) * _cellWidth + HalfCellWidth
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

    float distanceToMove = StepSize * (float)delta;

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