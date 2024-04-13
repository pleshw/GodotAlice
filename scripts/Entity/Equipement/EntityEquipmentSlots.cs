
using System;
using Godot;
using Items.Equipment;

namespace Entity;

public class EntityEquipmentSlots(Entity entity)
{
  public Entity Entity = entity;

  public EntityEquipmentBase LeftHand { get; set; } = EquipmentManager.Instance.CreateInstance("bare_hands", "bareHandsLeftHand");
  public EntityEquipmentBase RightHand { get; set; } = EquipmentManager.Instance.CreateInstance("bare_hands", "bareHandsRightHand");

  public EntityEquipmentBase Helmet { get; set; }
  public EntityEquipmentBase Armor { get; set; }
  public EntityEquipmentBase Neck { get; set; }
  public EntityEquipmentBase Back { get; set; }
  public EntityEquipmentBase Legs { get; set; }
  public EntityEquipmentBase Boots { get; set; }

  public EntityEquipmentBase AccessoryLeft { get; set; }
  public EntityEquipmentBase AccessoryRight { get; set; }

  public void CallActionOnEquippedItems(Action<EntityEquipmentBase> action)
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
