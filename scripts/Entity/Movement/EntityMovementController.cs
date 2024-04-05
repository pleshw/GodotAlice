using System;
using System.Collections.Generic;
using Godot;

namespace Entity;


public class EntityMovementController(Entity entity, Vector2 initialPosition, int gridMapCellWidth = 16)
{
  public Entity entity = entity;

  public Vector2 initialPosition = initialPosition;

  protected Vector2 LastTrackedPosition { get; set; } = Vector2.Zero;

  /// <summary>
  /// What is the movement state of the player. Helper to identify the best way to draw the player.
  /// </summary>
  protected EntityStateList<MOVEMENT_STATE> _states = new(entity, Comparer<MOVEMENT_STATE>.Create((a, b) => EntityMovementConstraints.MovementStatesPriorities[a].CompareTo(EntityMovementConstraints.MovementStatesPriorities[b])))
  {
    { MOVEMENT_STATE.IDLE }
  };

  /// <summary>
  /// Where player is going.
  /// </summary>
  protected Vector2? _dashTargetPosition;

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
  private bool _isMovementDisabled = false;

  public bool IsMovementDisabled { get { return _isMovementDisabled; } }

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

  public Vector2 DashTargetPosition
  {
    get
    {
      return _dashTargetPosition ?? entity.Position;
    }

    set
    {
      _dashTargetPosition = value;
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

  public float MaxSpeed
  {
    get
    {
      return MoveSpeed * StepSize;
    }
  }

  public float DashSpeed
  {
    get
    {
      return entity.DashSpeedModifier * MoveSpeed * StepSize;
    }
  }

  public EntityStateList<MOVEMENT_STATE> States
  {
    get
    {
      return _states;
    }
  }


  public EntityMovementController SetState(EntityMovementInput playerMovementInput)
  {
    if (playerMovementInput.ForceMovementState)
    {
      _states.Add(playerMovementInput.MovementState);
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

  public EntityMovementController DashTo(EntityMovementInput playerMovementInput)
  {
    SetState(playerMovementInput);
    _dashTargetPosition = playerMovementInput.Position;
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

  public bool BlockStates { get; set; } = false;

  public void Idled()
  {
    if (BlockStates)
    {
      return;
    }

    _states.Add(MOVEMENT_STATE.IDLE);
    _states.Remove(MOVEMENT_STATE.WALKING);
    entity.EmitSignal(Entity.SignalName.EntityStopped);
  }

  public void Moved()
  {
    if (BlockStates)
    {
      return;
    }

    _states.Add(MOVEMENT_STATE.WALKING);
    _states.Remove(MOVEMENT_STATE.IDLE);
    entity.EmitSignal(Entity.SignalName.EntityMoved, entity.Position, TargetPosition);
  }

  public void Dashed()
  {
    _states.Add(MOVEMENT_STATE.DASHING);
    _states.Remove(MOVEMENT_STATE.IDLE);
    _states.Remove(MOVEMENT_STATE.WALKING);
    entity.EmitSignal(Entity.SignalName.EntityMoved, entity.Position, TargetPosition);
  }

  public EntityMovementController MovementProcess(double delta, out bool hasMoved)
  {
    hasMoved = false;
    if ((_targetPosition == null && _dashTargetPosition == null) || IsMovementDisabled)
    {
      Idled();
      return this;
    }

    // Calculate the direction vector towards the target position


    switch (States.Max)
    {
      case MOVEMENT_STATE.WALKING:
        entity.FacingDirectionVector = (TargetPosition - entity.Position).Normalized();
        DefaultMovement((float)delta);
        break;
      case MOVEMENT_STATE.DASHING:
        entity.FacingDirectionVector = (DashTargetPosition - entity.Position).Normalized();
        DashMovement((float)delta);
        break;
    }

    hasMoved = true;
    return this;
  }

  public void DisableMovement()
  {
    _isMovementDisabled = true;
  }

  public void EnableMovement()
  {
    _isMovementDisabled = false;
  }

  private void DashMovement(float delta)
  {
    float distanceToMove = DashSpeed * delta;
    float distanceToTarget = entity.Position.DistanceTo(DashTargetPosition);
    if (distanceToTarget <= distanceToMove)
    {
      LastTrackedPosition = entity.Position;
      entity.Position = DashTargetPosition;
      _dashTargetPosition = null;
      Dashed();
      BlockStates = false;
      _states.Remove(MOVEMENT_STATE.DASHING);
    }
    else
    {
      Vector2 displacement = entity.FacingDirectionVector * distanceToMove;
      LastTrackedPosition = entity.Position;
      Vector2 entityNewPosition = LastTrackedPosition + displacement;
      entity.Position = entityNewPosition;
      Dashed();
      BlockStates = true;
    }
  }

  public void DefaultMovement(float delta)
  {
    float distanceToMove = MaxSpeed * delta;
    float distanceToTarget = entity.Position.DistanceTo(TargetPosition);
    if (distanceToTarget <= distanceToMove)
    {
      LastTrackedPosition = entity.Position;
      entity.Position = TargetPosition;
      _targetPosition = null;

      Moved();
    }
    else
    {
      Vector2 displacement = entity.FacingDirectionVector * distanceToMove;
      LastTrackedPosition = entity.Position;
      Vector2 entityNewPosition = LastTrackedPosition + displacement;
      entity.Position = entityNewPosition;

      Moved();
    }
  }
}