using Godot;

namespace Entity;


public record struct EntityMovementInput
{
  public required Vector2 Position;
  public required bool IsRunning;
  public required bool ForceMovementState;
  public required MOVEMENT_STATE MovementState;
}

public class EntityMovementController(Entity entity, Vector2 initialPosition, int gridMapCellWidth = 16)
{
  public Entity Entity { get; set; } = entity;

  public Vector2 initialPosition = initialPosition;

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
      return _targetPosition ?? Entity.Position;
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

  public int MaxSpeed
  {
    get
    {
      return MoveSpeed * StepSize;
    }
  }

  public MOVEMENT_STATE MovementState
  {
    get
    {
      return _movementState;
    }

    set
    {
      _movementState = value;
    }
  }


  public MOVEMENT_STATE LastMovementState
  {
    get
    {
      return _lastMovementState;
    }
  }

  public bool MovementStateUpdated
  {
    get
    {
      return LastMovementState != MovementState;
    }
  }

  public EntityMovementController ControlMovementState(EntityMovementInput playerMovementInput)
  {
    if (playerMovementInput.ForceMovementState)
    {
      _lastMovementState = _movementState;
      _movementState = playerMovementInput.MovementState;
      return this;
    }

    return this;
  }

  public EntityMovementController TeleportTo(EntityMovementInput playerMovementInput)
  {
    ControlMovementState(playerMovementInput);
    Entity.Position = playerMovementInput.Position;
    return this;
  }

  public EntityMovementController MoveTo(EntityMovementInput playerMovementInput)
  {
    ControlMovementState(playerMovementInput);
    _targetPosition = playerMovementInput.Position;
    return this;
  }

  public EntityMovementController MoveToNearestCell(EntityMovementInput playerMovementInput)
  {
    ControlMovementState(playerMovementInput);
    _targetPosition = new Vector2
    {
      X = Mathf.Round(playerMovementInput.Position.X / _cellWidth) * _cellWidth + HalfCellWidth,
      Y = Mathf.Round(playerMovementInput.Position.Y / _cellWidth) * _cellWidth + HalfCellWidth
    };

    return this;
  }

  public EntityMovementController TeleportToNearestCell(EntityMovementInput playerMovementInput)
  {
    ControlMovementState(playerMovementInput);
    Entity.Position = new Vector2
    {
      X = Mathf.Round(playerMovementInput.Position.X / _cellWidth) * _cellWidth + HalfCellWidth,
      Y = Mathf.Round(playerMovementInput.Position.Y / _cellWidth) * _cellWidth + HalfCellWidth
    };

    return this;
  }


  public EntityMovementController DefaultMovementProcess(double delta, out bool hasMoved)
  {
    hasMoved = false;
    if (_targetPosition == null)
    {
      return this;
    }

    // Calculate the direction vector towards the target position
    Vector2 direction = (TargetPosition - Entity.Position).Normalized();

    float distanceToMove = MaxSpeed * (float)delta;
    float distanceToTarget = Entity.Position.DistanceTo(TargetPosition);
    if (distanceToTarget <= distanceToMove)
    {
      Entity.Position = TargetPosition;
      _lastTrackedPosition = TargetPosition;
      _targetPosition = null;
    }
    else
    {
      Entity.Position += direction * distanceToMove;
    }

    hasMoved = true;
    return this;
  }
}