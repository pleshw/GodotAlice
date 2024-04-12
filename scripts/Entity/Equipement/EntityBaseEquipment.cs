
using Godot;

namespace Entity;


public abstract partial class EntityEquipmentBase : Resource, IEquipment
{
  [Export]
  public SpriteFrames SpriteResource { get; set; }

  /// <summary>
  /// The name of the .tres file at the path $"res://resources/equipment/{resourceToLoad.ItemId}.tres";
  /// </summary>
  public abstract string ItemId { get; set; }

  /// <summary>
  /// The name that will be displayed
  /// </summary>
  public abstract string ItemName { get; set; }

  public abstract EntityEquipmentSlotType SlotType { get; set; }

  public abstract int LevelRequired { get; set; }

  public abstract void ModifyAttributes(Entity entity);

  public abstract void ModifyStats(Entity entity);

  public abstract void ExecuteEffect(Entity entity, Entity target);

  public bool CheckCanEquipAtPosition(EntityEquipmentSlotType position)
  {
    return SlotType == EntityEquipmentSlotType.ANY || SlotType == position;
  }

  public bool CheckCanEquipAtLevel(int entityLevel)
  {
    return LevelRequired <= 0 || LevelRequired <= entityLevel;
  }

  public bool CanEquip(Entity entity)
  {
    return LevelRequired <= entity.Level;
  }

  public bool TryEquip(Entity entity, EntityEquipmentSlotType position)
  {
    if (!CheckCanEquipAtPosition(position) || !CheckCanEquipAtLevel(entity.Level))
    {
      return false;
    }

    if (position == EntityEquipmentSlotType.ANY)
    {
      return false;
    }

    switch (position)
    {
      case EntityEquipmentSlotType.ANY:
        entity.equipment.AccessoryLeft = this;
        entity.TryEquipSuccessEvent(this, position);
        return true;

      case EntityEquipmentSlotType.LEFT_HAND:
        entity.equipment.LeftHand = this;
        entity.TryEquipSuccessEvent(this, position);
        return true;
      case EntityEquipmentSlotType.RIGHT_HAND:
        entity.equipment.RightHand = this;
        entity.TryEquipSuccessEvent(this, position);
        return true;

      case EntityEquipmentSlotType.HELMET:
        entity.equipment.Helmet = this;
        entity.TryEquipSuccessEvent(this, position);
        return true;
      case EntityEquipmentSlotType.ARMOR:
        entity.equipment.Armor = this;
        entity.TryEquipSuccessEvent(this, position);
        return true;
      case EntityEquipmentSlotType.NECK:
        entity.equipment.Neck = this;
        entity.TryEquipSuccessEvent(this, position);
        return true;
      case EntityEquipmentSlotType.BACK:
        entity.equipment.Back = this;
        entity.TryEquipSuccessEvent(this, position);
        return true;
      case EntityEquipmentSlotType.LEGS:
        entity.equipment.Legs = this;
        entity.TryEquipSuccessEvent(this, position);
        return true;
      case EntityEquipmentSlotType.BOOTS:
        entity.equipment.Boots = this;
        entity.TryEquipSuccessEvent(this, position);
        return true;

      case EntityEquipmentSlotType.ACCESSORY_LEFT:
        entity.equipment.AccessoryLeft = this;
        entity.TryEquipSuccessEvent(this, position);
        return true;
      case EntityEquipmentSlotType.ACCESSORY_RIGHT:
        entity.equipment.AccessoryRight = this;
        entity.TryEquipSuccessEvent(this, position);
        return true;

      default: return false;
    }
  }
}
