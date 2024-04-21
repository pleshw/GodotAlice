using Godot;

namespace Entity.Commands;


public partial class PlayerCommands
{
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
        ForceStateChange = false,
        GameState = EntityGameState.MOVING,
      });

      entity.directionState.LastCommandDirection = DIRECTIONS.TOP;
      entity.MovementInputEvent();
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
        ForceStateChange = false,
        GameState = EntityGameState.MOVING,
      });

      entity.directionState.LastCommandDirection = DIRECTIONS.RIGHT;
      entity.MovementInputEvent();
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
        ForceStateChange = false,
        GameState = EntityGameState.MOVING,
      });

      entity.directionState.LastCommandDirection = DIRECTIONS.BOTTOM;
      entity.MovementInputEvent();
    }
  }

  public class DashCommand(Entity entityToMove, float cooldownTime = .7f) : EntityMovementCommand(entityToMove)
  {
    private float lastDashTime = -1.0f;
    private readonly float cooldownTime = cooldownTime; // cooldown time in seconds

    public float Cooldown
    {
      get
      {
        if (lastDashTime == -1)
        {
          return 0;
        }

        float currentTime = Time.GetTicksMsec() / 1000.0f;
        float timeSinceLastDash = currentTime - lastDashTime;
        if (timeSinceLastDash >= cooldownTime)
        {
          lastDashTime = -1;
          return 0;
        }

        return cooldownTime - timeSinceLastDash;
      }
    }

    public override void Execute(bool repeating)
    {
      float currentTime = Time.GetTicksMsec() / 1000.0f; // Get current time in seconds
      if (repeating || currentTime - lastDashTime < cooldownTime || entity.GameState.HasFlag(EntityGameState.DASHING))
      {
        return; // Action is still on cooldown or entity is already dashing
      }

      MovementController.DashTo(new EntityMovementInput
      {
        Position = entity.Position + (entity.directionState.FacingDirectionVector.Normalized() * entity.DashDistance),
        IsRunning = false,
        ForceStateChange = true,
        GameState = EntityGameState.DASHING,
      });

      entity.MovementInputEvent();

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
        ForceStateChange = false,
        GameState = EntityGameState.MOVING,
      });
      entity.directionState.LastCommandDirection = DIRECTIONS.LEFT;
      entity.MovementInputEvent();
    }
  }

}

