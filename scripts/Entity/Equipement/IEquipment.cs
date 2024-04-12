
using System.Collections.Generic;
using Godot;

namespace Entity;

public interface IEquipment
{

  public void ModifyAttributes(Entity entity);

  public void ModifyStats(Entity entity);

  public void ExecuteEffect(Entity entity, List<Entity> targets);

  public void PassiveEffect(Entity entity, List<Entity> targets);

}
