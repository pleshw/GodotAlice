
namespace Entity.Commands;


public abstract class EntityBaseCommand(Entity entity) : IEntityCommand
{
  public Entity entity = entity;


  public abstract void Execute(bool repeating);
}