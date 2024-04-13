
using System.Collections.Generic;
using GameManagers;
using Godot;

namespace Entity;


public abstract partial class EntityEquipmentBase : Node, IEquipment
{
  [Export]
  public EntityEquipmentResource ItemResource { get; set; }

  public abstract EntityEquipmentSlotType SlotType { get; set; }

  public abstract int LevelRequired { get; set; }

  public abstract void ModifyAttributes(Entity entity);

  public abstract void ModifyStats(Entity entity);

  public abstract void ExecuteEffect(Entity entity, List<Entity> targets);

  public abstract void PassiveEffect(Entity entity, List<Entity> targets);

  public bool CheckCanEquipAtPosition(EntityEquipmentSlotType position)
  {
    return SlotType == EntityEquipmentSlotType.ANY || SlotType == position;
  }

  public bool CheckCanEquipAtLevel(int entityLevel)
  {
    return LevelRequired <= 0 || LevelRequired <= entityLevel;
  }

  public bool CanEquip(Entity entity, EntityEquipmentSlotType position)
  {
    if (!CheckCanEquipAtPosition(position) || !CheckCanEquipAtLevel(entity.Level))
    {
      return false;
    }

    if (position == EntityEquipmentSlotType.NONE)
    {
      return false;
    }

    return true;
  }
}
