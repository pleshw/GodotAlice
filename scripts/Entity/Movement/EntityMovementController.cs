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
  public Entity entity = entity;

  public Vector2 initialPosition = initialPosition;

  protected Vector2 LastTrackedPosition { get; set; } = Vector2.Zero;

  /// <summary>
  /// What is the movement state of the player. Helper to identify the best way to draw the player.
  /// </summary>
  protected MOVEMENT_STATE _lastState = EntityDefaults.MovementState;

  /// <summary>
  /// What is the movement state of the player. Helper to identify the best way to draw the player.
  /// </summary>
  protected MOVEMENT_STATE _state = EntityDefaults.MovementState;

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
      return _targetPosition ?? entity.Position;
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

  public MOVEMENT_STATE State
  {
    get
    {
      return _state;
    }

    set
    {
      _lastState = _state;
      _state = value;
    }
  }


  public MOVEMENT_STATE LastState
  {
    get
    {
      return _lastState;
    }
  }

  public bool StateUpdated
  {
    get
    {
      return LastState != State;
    }
  }

  public EntityMovementController SetState(EntityMovementInput playerMovementInput)
  {
    if (playerMovementInput.ForceMovementState)
    {
      State = playerMovementInput.MovementState;
      return this;
    }

    return this;
  }

  public EntityMovementController TeleportTo(EntityMovementInput playerMovementInput)
  {
    SetState(playerMovementInput);
    entity.Position = playerMovementInput.Position;
    return this;
  }

  public EntityMovementController WalkTo(EntityMovementInput playerMovementInput)
  {
    SetState(playerMovementInput);
    _targetPosition = playerMovementInput.Position;
    return this;
  }

  public EntityMovementController WalkToNearestCell(EntityMovementInput playerMovementInput)
  {
    SetState(playerMovementInput);
    _targetPosition = new Vector2
    {
      X = Mathf.Round(playerMovementInput.Position.X / _cellWidth) * _cellWidth + HalfCellWidth,
      Y = Mathf.Round(playerMovementInput.Position.Y / _cellWidth) * _cellWidth + HalfCellWidth
    };

    return this;
  }

  public EntityMovementController TeleportToNearestCell(EntityMovementInput playerMovementInput)
  {
    SetState(playerMovementInput);
    entity.Position = new Vector2
    {
      X = Mathf.Round(playerMovementInput.Position.X / _cellWidth) * _cellWidth + HalfCellWidth,
      Y = Mathf.Round(playerMovementInput.Position.Y / _cellWidth) * _cellWidth + HalfCellWidth
    };

    return this;
  }


  public EntityMovementController MovementProcess(double delta, out bool hasMoved)
  {
    hasMoved = false;
    if (_targetPosition == null)
    {
      State = MOVEMENT_STATE.IDLE;
      entity.EmitSignal(Entity.SignalName.EntityStopped);
      return this;
    }

    // Calculate the direction vector towards the target position
    entity.FacingDirectionVector = (TargetPosition - entity.Position).Normalized().Inverse();

    float distanceToMove = MaxSpeed * (float)delta;
    float distanceToTarget = entity.Position.DistanceTo(TargetPosition);
    if (distanceToTarget <= distanceToMove)
    {
      LastTrackedPosition = entity.Position;
      entity.Position = TargetPosition;
      _targetPosition = null;

      entity.EmitSignal(Entity.SignalName.EntityMoved, entity.Position, TargetPosition);
    }
    else
    {
      Vector2 displacement = entity.FacingDirectionVector.Inverse() * distanceToMove;
      LastTrackedPosition = entity.Position;

      Vector2 entityNewPosition = LastTrackedPosition + displacement;

      entity.Position = entityNewPosition;

      entity.EmitSignal(Entity.SignalName.EntityMoved, LastTrackedPosition, entity.Position);
    }

    hasMoved = true;
    return this;
  }
}