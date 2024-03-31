using Godot;

namespace Entity.Commands.Movement;

public abstract class EntityMovementCommand(Entity entityToMove) : IEntityCommand
{
  public Entity entityToMove = entityToMove;

  public abstract void Execute();
}

public class WalkTopCommand(Entity entityToMove) : EntityMovementCommand(entityToMove)
{
  public override void Execute()
  {
    entityToMove.facing &= DIRECTIONS.TOP;

    entityToMove.MoveTo(new PlayerMovementInput
    {
      Position = entityToMove.TargetPosition with
      {
        Y = entityToMove.Position.Y - entityToMove.StepSize
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
    entityToMove.facing &= DIRECTIONS.RIGHT;

    entityToMove.MoveTo(new PlayerMovementInput
    {
      Position = entityToMove.TargetPosition with
      {
        X = entityToMove.Position.X + entityToMove.StepSize
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
    entityToMove.facing &= DIRECTIONS.BOTTOM;

    entityToMove.MoveTo(new PlayerMovementInput
    {
      Position = entityToMove.TargetPosition with
      {
        Y = entityToMove.Position.Y + entityToMove.StepSize
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
    entityToMove.facing &= DIRECTIONS.LEFT;

    entityToMove.MoveTo(new PlayerMovementInput
    {
      Position = entityToMove.TargetPosition with
      {
        X = entityToMove.Position.X - entityToMove.StepSize
      },
      IsRunning = false,
      ForceMovementState = true,
      MovementState = MOVEMENT_STATE.WALKING,
    });
  }
}
