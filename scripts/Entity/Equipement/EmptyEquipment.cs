
using Godot;

namespace Entity;

public class EmptyEquipment : EntityBaseEquipment
{
  public override EntityEquipmentPosition Position { get; set; } = EntityEquipmentPosition.ANY;

  public override int LevelRequired { get; set; } = 0;

  public override void ModifyAttributes(Entity entity)
  {

  }

  public override void ModifyStats(Entity entity)
  {

  }

  public override void ExecuteEffect(Entity entity, Entity target)
  {

  }
}
