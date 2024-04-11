
namespace Entity.Commands;

public abstract class EntityMovementCommand(Entity entity) : IEntityCommand
{
  public Entity entity = entity;
  public EntityMovementController MovementController = entity.MovementController;


  public abstract void Execute(bool repeating);
}