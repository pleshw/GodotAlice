using System;
using System.Collections.Generic;
using Godot;

namespace Entity;


public class EntityMovementController(Entity entity, Vector2 initialPosition, int gridMapCellWidth = 32)
{
  public Entity entity = entity;

  public Vector2 initialPosition = initialPosition;

  protected Vector2 LastTrackedPosition { get; set; } = Vector2.Zero;

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
  private bool _movementDisabled = false;

  public bool MovementDisabled { get { return _movementDisabled; } }

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
  public int HalfCellWidth
  {
    get
    {
      return _cellWidth / 2;
    }
  }

  /// <summary>
  /// How much the player moves per interaction.
  /// </summary>
  public int CellWidth
  {
    get
    {
      return _cellWidth;
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

  public EntityMovementController SetState(EntityGameState newState, bool lockAfter = false)
  {
    if (LockState)
    {
      return this;
    }

    entity.GameState = newState;

    if (lockAfter)
    {
      LockState = true;
      entity.LockGameState = true;
    }

    return this;
  }

  public EntityMovementController TeleportTo(EntityMovementInput playerMovementInput)
  {
    SetState(playerMovementInput.GameState);
    entity.Position = playerMovementInput.Position;
    return this;
  }

  public EntityMovementController WalkTo(EntityMovementInput playerMovementInput)
  {
    SetState(playerMovementInput.GameState);
    _targetPosition = playerMovementInput.Position;
    return this;
  }

  public EntityMovementController DashTo(EntityMovementInput playerMovementInput)
  {
    SetState(playerMovementInput.GameState, true);
    _dashTargetPosition = playerMovementInput.Position;
    _targetPosition = _dashTargetPosition;
    return this;
  }

  public EntityMovementController WalkToNearestCell(EntityMovementInput playerMovementInput)
  {
    SetState(playerMovementInput.GameState);
    _targetPosition = new Vector2
    {
      X = Mathf.Round(playerMovementInput.Position.X / _cellWidth) * _cellWidth + HalfCellWidth,
      Y = Mathf.Round(playerMovementInput.Position.Y / _cellWidth) * _cellWidth + HalfCellWidth
    };

    return this;
  }

  public EntityMovementController TeleportToNearestCell(EntityMovementInput playerMovementInput)
  {
    SetState(playerMovementInput.GameState);
    entity.Position = new Vector2
    {
      X = Mathf.Round(playerMovementInput.Position.X / _cellWidth) * _cellWidth + HalfCellWidth,
      Y = Mathf.Round(playerMovementInput.Position.Y / _cellWidth) * _cellWidth + HalfCellWidth
    };

    return this;
  }

  public bool LockState { get; set; } = false;

  public bool EndMovementOnNextIteration { get; set; } = false;

  public void Idled()
  {
    SetState(EntityGameState.IDLE);
    entity.EntityStoppedEvent();
  }

  public void Moved()
  {
    SetState(EntityGameState.MOVING);
    entity.EntityMovedEvent(entity.Position, TargetPosition);
  }

  public void Dashed()
  {
    SetState(EntityGameState.DASHING);
    entity.EntityDashedEvent(entity.Position, TargetPosition);
  }

  public void ForceIdle()
  {
    entity.LockGameState = false;
    LockState = false;
    Idled();
  }

  public EntityMovementController MovementProcess(double delta, out bool hasMoved)
  {
    hasMoved = false;
    if (EndMovementOnNextIteration)
    {
      _targetPosition = null;
      _dashTargetPosition = null;
      EndMovementOnNextIteration = false;
    }

    if ((_targetPosition == null && _dashTargetPosition == null) || MovementDisabled)
    {
      Idled();
      return this;
    }

    switch (entity.GameState)
    {
      case EntityGameState.DASHING:
        DashProcess((float)delta);
        break;
      case EntityGameState.MOVING:
        WalkProcess((float)delta);
        break;
    }

    hasMoved = true;
    return this;
  }

  public void DashProcess(float delta)
  {
    Vector2 displacementDirection = (DashTargetPosition - entity.Position).Normalized();
    if (displacementDirection == Vector2.Zero)
    {
      ForceIdle();
      return;
    }
    entity.directionState.FacingDirectionVector = displacementDirection;
    ExecuteDash(delta);
  }

  public void WalkProcess(float delta)
  {
    Vector2 displacementDirection = (TargetPosition - entity.Position).Normalized();
    if (displacementDirection == Vector2.Zero)
    {
      ForceIdle();
      return;
    }
    entity.directionState.FacingDirectionVector = displacementDirection;
    ExecuteWalk(delta);
  }

  public void DisableMovement()
  {
    _movementDisabled = true;
  }

  public void EnableMovement()
  {
    _movementDisabled = false;
  }

  private void ExecuteDash(float delta)
  {
    if (EndMovementOnNextIteration)
    {
      _targetPosition = null;
      _dashTargetPosition = null;
      EndMovementOnNextIteration = false;
      return;
    }

    float distanceToMove = DashSpeed * delta;
    float distanceToTarget = entity.Position.DistanceTo(DashTargetPosition);
    if (distanceToTarget <= distanceToMove)
    {
      LastTrackedPosition = entity.Position;
      entity.Position = DashTargetPosition;
      _dashTargetPosition = null;
      Dashed();
      GD.Print("unlocked: ", LockState);
      LockState = false;
      entity.LockGameState = false;
    }
    else
    {
      Vector2 displacement = entity.directionState.FacingDirectionVector * distanceToMove;
      LastTrackedPosition = entity.Position;
      Vector2 entityNewPosition = LastTrackedPosition + displacement;
      entity.Position = entityNewPosition;
      Dashed();
      GD.Print("locked: ", LockState);
      entity.LockGameState = true;
      LockState = true;
    }
  }

  public void ExecuteWalk(float delta)
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
      Vector2 displacement = entity.directionState.FacingDirectionVector * distanceToMove;
      LastTrackedPosition = entity.Position;
      Vector2 entityNewPosition = LastTrackedPosition + displacement;
      entity.Position = entityNewPosition;

      Moved();
    }
  }
}