
using Godot;

namespace Entity;


public abstract class EntityBaseEquipment : IEquipment
{
  public abstract EntityEquipmentPosition Position { get; set; }
  public abstract int LevelRequired { get; set; }

  public virtual void ModifyAttributes(Entity entity)
  {

  }

  public virtual void ModifyStats(Entity entity)
  {

  }

  public virtual void ExecuteEffect(Entity entity, Entity target)
  {

  }

  public bool CheckCanEquipAtPosition(EntityEquipmentPosition position)
  {
    return Position == EntityEquipmentPosition.ANY || Position == position;
  }

  public bool CheckCanEquipAtLevel(int entityLevel)
  {
    return LevelRequired <= 0 || LevelRequired <= entityLevel;
  }

  public bool CanEquip(Entity entity)
  {
    return LevelRequired <= entity.Level;
  }

  public bool TryEquip(Entity entity, EntityEquipmentPosition position)
  {
    if (!CheckCanEquipAtPosition(position) || !CheckCanEquipAtLevel(entity.Level))
    {
      return false;
    }

    if (position == EntityEquipmentPosition.ANY)
    {
      return false;
    }

    switch (position)
    {
      case EntityEquipmentPosition.ANY:
        entity.equipment.AccessoryLeft = this;
        return true;

      case EntityEquipmentPosition.LEFT_HAND:
        entity.equipment.LeftHand = this;
        return true;
      case EntityEquipmentPosition.RIGHT_HAND:
        entity.equipment.RightHand = this;
        return true;

      case EntityEquipmentPosition.HELMET:
        entity.equipment.Helmet = this;
        return true;
      case EntityEquipmentPosition.ARMOR:
        entity.equipment.Armor = this;
        return true;
      case EntityEquipmentPosition.NECK:
        entity.equipment.Neck = this;
        return true;
      case EntityEquipmentPosition.BACK:
        entity.equipment.Back = this;
        return true;
      case EntityEquipmentPosition.LEGS:
        entity.equipment.Legs = this;
        return true;
      case EntityEquipmentPosition.BOOTS:
        entity.equipment.Boots = this;
        return true;

      case EntityEquipmentPosition.ACCESSORY_LEFT:
        entity.equipment.AccessoryLeft = this;
        return true;
      case EntityEquipmentPosition.ACCESSORY_RIGHT:
        entity.equipment.AccessoryRight = this;
        return true;

      default: return false;
    }
  }
}
