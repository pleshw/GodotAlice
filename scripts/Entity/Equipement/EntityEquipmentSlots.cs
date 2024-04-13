
using System;
using System.Collections.Generic;
using Godot;
using Items.Equipment;

namespace Entity;

public class EntityEquipmentSlots
{
  public Entity Entity;

  public EntityEquipmentSlot LeftHand { get { return SlotsByName["LeftHand"]; } }
  // EquipmentManager.Instance.CreateInstance("bare_hands", "bareHandsLeftHand");
  public EntityEquipmentSlot RightHand { get { return SlotsByName["RightHand"]; } }
  public EntityEquipmentSlot Helmet { get { return SlotsByName["Helmet"]; } }
  public EntityEquipmentSlot Armor { get { return SlotsByName["Armor"]; } }
  public EntityEquipmentSlot Neck { get { return SlotsByName["Neck"]; } }
  public EntityEquipmentSlot Back { get { return SlotsByName["Back"]; } }
  public EntityEquipmentSlot Legs { get { return SlotsByName["Legs"]; } }
  public EntityEquipmentSlot Boots { get { return SlotsByName["Boots"]; } }
  public EntityEquipmentSlot AccessoryLeft { get { return SlotsByName["AccessoryLeft"]; } }
  public EntityEquipmentSlot AccessoryRight { get { return SlotsByName["AccessoryRight"]; } }

  public readonly Dictionary<StringName, EntityEquipmentSlot> SlotsByName = [];

  public EntityEquipmentSlots(Entity entity)
  {
    Entity = entity;
    SlotsByName.Add("LeftHand", new(entity, EntityEquipmentSlotType.LEFT_HAND));
    SlotsByName.Add("RightHand", new(entity, EntityEquipmentSlotType.RIGHT_HAND));
    SlotsByName.Add("Helmet", new(entity, EntityEquipmentSlotType.HELMET));
    SlotsByName.Add("Armor", new(entity, EntityEquipmentSlotType.ARMOR));
    SlotsByName.Add("Neck", new(entity, EntityEquipmentSlotType.NECK));
    SlotsByName.Add("Back", new(entity, EntityEquipmentSlotType.BACK));
    SlotsByName.Add("Legs", new(entity, EntityEquipmentSlotType.LEGS));
    SlotsByName.Add("Boots", new(entity, EntityEquipmentSlotType.BOOTS));
    SlotsByName.Add("AccessoryLeft", new(entity, EntityEquipmentSlotType.ACCESSORY_LEFT));
    SlotsByName.Add("AccessoryRight", new(entity, EntityEquipmentSlotType.ACCESSORY_RIGHT));
  }

  public void ForEveryItemSlot(Action<EntityEquipmentSlot> action)
  {
    if (LeftHand != null)
    {
      action.Invoke(LeftHand);
    }
    if (RightHand != null)
    {
      action.Invoke(RightHand);
    }
    if (Helmet != null)
    {
      action.Invoke(Helmet);
    }
    if (Armor != null)
    {
      action.Invoke(Armor);
    }
    if (Neck != null)
    {
      action.Invoke(Neck);
    }
    if (Back != null)
    {
      action.Invoke(Back);
    }
    if (Legs != null)
    {
      action.Invoke(Legs);
    }
    if (Boots != null)
    {
      action.Invoke(Boots);
    }
    if (AccessoryLeft != null)
    {
      action.Invoke(AccessoryLeft);
    }
    if (AccessoryRight != null)
    {
      action.Invoke(AccessoryRight);
    }
  }
}
