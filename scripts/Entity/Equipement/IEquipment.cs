
using Godot;

namespace Entity;

public interface IEquipment
{

  public void ModifyAttributes(Entity entity);
  public void ModifyStats(Entity entity);
  public void ExecuteEffect(Entity entity, Entity target);
}
