using Godot;

namespace Entity.Commands.Movement;

public abstract class EntityMovementCommand(Entity entity) : IEntityCommand
{
  public Entity entity = entity;
  public EntityMovementController MovementController = entity.MovementController;


  public abstract void Execute(bool repeating);
}

public class WalkTopCommand(Entity entityToMove) : EntityMovementCommand(entityToMove)
{

  public override void Execute(bool repeating)
  {
    MovementController.WalkTo(new EntityMovementInput
    {
      Position = MovementController.TargetPosition with
      {
        Y = entity.Position.Y - MovementController.StepSize
      },
      IsRunning = false,
      ForceMovementState = true,
      MovementState = MOVEMENT_STATE.WALKING,
    });

    entity.LastCommandDirection = DIRECTIONS.TOP;
    entity.EmitSignal(Entity.SignalName.MovementInputTriggered);
  }
}

public class WalkRightCommand(Entity entityToMove) : EntityMovementCommand(entityToMove)
{
  public override void Execute(bool repeating)
  {
    MovementController.WalkTo(new EntityMovementInput
    {
      Position = MovementController.TargetPosition with
      {
        X = entity.Position.X + MovementController.StepSize
      },
      IsRunning = false,
      ForceMovementState = true,
      MovementState = MOVEMENT_STATE.WALKING,
    });

    entity.LastCommandDirection = DIRECTIONS.RIGHT;
    entity.EmitSignal(Entity.SignalName.MovementInputTriggered);
  }
}


public class WalkBottomCommand(Entity entityToMove) : EntityMovementCommand(entityToMove)
{
  public override void Execute(bool repeating)
  {
    MovementController.WalkTo(new EntityMovementInput
    {
      Position = MovementController.TargetPosition with
      {
        Y = entity.Position.Y + MovementController.StepSize
      },
      IsRunning = false,
      ForceMovementState = true,
      MovementState = MOVEMENT_STATE.WALKING,
    });

    entity.LastCommandDirection = DIRECTIONS.BOTTOM;
    entity.EmitSignal(Entity.SignalName.MovementInputTriggered);
  }
}

public class DashCommand(Entity entityToMove) : EntityMovementCommand(entityToMove)
{
  private float lastDashTime = -1.0f;
  private readonly float cooldownTime = 1f; // Set your cooldown time here

  public override void Execute(bool repeating)
  {
    float currentTime = Time.GetTicksMsec() / 300.0f; // Get current time in seconds
    if (repeating || currentTime - lastDashTime < cooldownTime || entity.MovementController.States.Contains(MOVEMENT_STATE.DASHING))
    {
      return; // Action is still on cooldown or entity is already dashing
    }

    MovementController.DashTo(new EntityMovementInput
    {
      Position = entity.Position + (entity.FacingDirectionVector.Normalized() * entity.DashDistance),
      IsRunning = entity.MovementController.States.Contains(MOVEMENT_STATE.RUNNING),
      ForceMovementState = true,
      MovementState = MOVEMENT_STATE.DASHING,
    });

    entity.EmitSignal(Entity.SignalName.MovementInputTriggered);

    lastDashTime = currentTime; // Record the time of this dash
  }
}

public class WalkLeftCommand(Entity entityToMove) : EntityMovementCommand(entityToMove)
{
  public override void Execute(bool repeating)
  {
    MovementController.WalkTo(new EntityMovementInput
    {
      Position = MovementController.TargetPosition with
      {
        X = entity.Position.X - MovementController.StepSize
      },
      IsRunning = false,
      ForceMovementState = true,
      MovementState = MOVEMENT_STATE.WALKING,
    });
    entity.LastCommandDirection = DIRECTIONS.LEFT;
    entity.EmitSignal(Entity.SignalName.MovementInputTriggered);
  }
}
