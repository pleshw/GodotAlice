
namespace Entity.Commands;


public abstract class EntityMovementCommand(Entity entityToMove) : IEntityCommand
{
  public Entity entityToMove = entityToMove;

  public EntityMovementController MovementController = entityToMove.MovementController;

  public abstract void Execute(bool repeating);
}