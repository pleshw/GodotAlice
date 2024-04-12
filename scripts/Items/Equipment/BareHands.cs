using System.Collections.Generic;
using Entity;
using Godot;

namespace Items.Equipment;

public partial class BareHands : EntityEquipmentBase
{
  [Export]
  public override EntityEquipmentSlotType SlotType { get; set; }

  [Export]
  public override int LevelRequired { get; set; }

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