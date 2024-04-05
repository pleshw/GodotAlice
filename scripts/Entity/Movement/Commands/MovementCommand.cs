using Godot;

namespace Entity.Commands.Movement;

public abstract class EntityMovementCommand(Entity entityToMove) : IEntityCommand
{
  public Entity entityToMove = entityToMove;

  public EntityMovementController MovementController = entityToMove.MovementController;

  public abstract void Execute();
}

public class WalkTopCommand(Entity entityToMove) : EntityMovementCommand(entityToMove)
{
  public override void Execute()
  {
    MovementController.WalkTo(new EntityMovementInput
    {
      Position = MovementController.TargetPosition with
      {
        Y = entityToMove.Position.Y - MovementController.StepSize
      },
      IsRunning = false,
      ForceMovementState = true,
      MovementState = MOVEMENT_STATE.WALKING,
    });

    entityToMove.LastCommandDirection = DIRECTIONS.TOP;
    entityToMove.EmitSignal(Entity.SignalName.MovementInputTriggered);
  }
}

public class WalkRightCommand(Entity entityToMove) : EntityMovementCommand(entityToMove)
{
  public override void Execute()
  {
    MovementController.WalkTo(new EntityMovementInput
    {
      Position = MovementController.TargetPosition with
      {
        X = entityToMove.Position.X + MovementController.StepSize
      },
      IsRunning = false,
      ForceMovementState = true,
      MovementState = MOVEMENT_STATE.WALKING,
    });

    entityToMove.LastCommandDirection = DIRECTIONS.RIGHT;
    entityToMove.EmitSignal(Entity.SignalName.MovementInputTriggered);
  }
}


public class WalkBottomCommand(Entity entityToMove) : EntityMovementCommand(entityToMove)
{
  public override void Execute()
  {
    MovementController.WalkTo(new EntityMovementInput
    {
      Position = MovementController.TargetPosition with
      {
        Y = entityToMove.Position.Y + MovementController.StepSize
      },
      IsRunning = false,
      ForceMovementState = true,
      MovementState = MOVEMENT_STATE.WALKING,
    });

    entityToMove.LastCommandDirection = DIRECTIONS.BOTTOM;
    entityToMove.EmitSignal(Entity.SignalName.MovementInputTriggered);
  }
}

public class DashCommand(Entity entityToMove) : EntityMovementCommand(entityToMove)
{
  private float lastDashTime = -1.0f;
  private readonly float cooldownTime = 1f; // Set your cooldown time here

  public override void Execute()
  {
    float currentTime = Time.GetTicksMsec() / 1000.0f; // Get current time in seconds
    if (currentTime - lastDashTime < cooldownTime || entityToMove.MovementController.States.Contains(MOVEMENT_STATE.DASHING))
    {
      return; // Action is still on cooldown or entity is already dashing
    }

    MovementController.WalkTo(new EntityMovementInput
    {
      Position = entityToMove.Position + (entityToMove.FacingDirectionVector.Normalized() * entityToMove.DashDistance),
      IsRunning = entityToMove.MovementController.States.Contains(MOVEMENT_STATE.RUNNING),
      ForceMovementState = true,
      MovementState = MOVEMENT_STATE.DASHING,
    });

    entityToMove.EmitSignal(Entity.SignalName.MovementInputTriggered);


    lastDashTime = currentTime; // Record the time of this dash
  }
}

public class WalkLeftCommand(Entity entityToMove) : EntityMovementCommand(entityToMove)
{
  public override void Execute()
  {
    MovementController.WalkTo(new EntityMovementInput
    {
      Position = MovementController.TargetPosition with
      {
        X = entityToMove.Position.X - MovementController.StepSize
      },
      IsRunning = false,
      ForceMovementState = true,
      MovementState = MOVEMENT_STATE.WALKING,
    });
    entityToMove.LastCommandDirection = DIRECTIONS.LEFT;
    entityToMove.EmitSignal(Entity.SignalName.MovementInputTriggered);
  }
}
