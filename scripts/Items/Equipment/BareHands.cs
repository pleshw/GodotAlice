using System.Collections.Generic;
using Entity;
using Godot;

namespace Items.Equipment;

public partial class BareHands : EntityEquipmentBase
{
  public override EntityEquipmentSlotType SlotType { get; set; } = EntityEquipmentSlotType.RIGHT_HAND | EntityEquipmentSlotType.LEFT_HAND;

  public override int LevelRequired { get; set; } = 0;

  public override void _Ready()
  {
    base._Ready();
    AddToGroup("Items");
    AddToGroup("Equipment");
  }

  public override void ModifyAttributes(Entity.Entity entity)
  {
  }

  public override void ModifyStats(Entity.Entity entity)
  {
  }

  public override void ExecuteEffect(Entity.Entity entity, List<Entity.Entity> targets)
  {
  }

  public override void PassiveEffect(Entity.Entity entity, List<Entity.Entity> targets)
  {
  }
}