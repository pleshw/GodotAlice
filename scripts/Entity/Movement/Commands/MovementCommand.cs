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
    MovementController.MoveTo(new EntityMovementInput
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
    MovementController.MoveTo(new EntityMovementInput
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
    MovementController.MoveTo(new EntityMovementInput
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

public class WalkLeftCommand(Entity entityToMove) : EntityMovementCommand(entityToMove)
{
  public override void Execute()
  {
    MovementController.MoveTo(new EntityMovementInput
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
