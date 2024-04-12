
using Godot;

namespace Entity;


[GlobalClass]
public partial class EmptyEquipment() : EntityEquipmentBase
{
  public override string ItemId { get; set; } = "EmptyEquipment";
  public override string ItemName { get; set; } = "Empty Equipment";
  public override EntityEquipmentSlotType SlotType { get; set; } = EntityEquipmentSlotType.ANY;

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

