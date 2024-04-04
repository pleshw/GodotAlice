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
    entityToMove.EmitSignal(Entity.SignalName.MovementInputTriggered);

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
  }
}

public class WalkRightCommand(Entity entityToMove) : EntityMovementCommand(entityToMove)
{
  public override void Execute()
  {
    entityToMove.EmitSignal(Entity.SignalName.MovementInputTriggered);

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
  }
}


public class WalkBottomCommand(Entity entityToMove) : EntityMovementCommand(entityToMove)
{
  public override void Execute()
  {
    entityToMove.EmitSignal(Entity.SignalName.MovementInputTriggered);

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
  }
}

public class WalkLeftCommand(Entity entityToMove) : EntityMovementCommand(entityToMove)
{
  public override void Execute()
  {
    entityToMove.EmitSignal(Entity.SignalName.MovementInputTriggered);

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
  }
}
