using Godot;

namespace Entity;


public partial class EntityEquipmentSlot(Entity owner, EntityEquipmentSlotType slotType) : Node
{
  // public EntityEquipmentBase LeftHand { get; set; } = EquipmentManager.Instance.CreateInstance("bare_hands", "bareHandsLeftHand");
  public Entity Entity = owner;
  public readonly EntityEquipmentSlotType SlotType = slotType;

  public EntityEquipmentBase Equipment { get; set; } = null;

  public override void _Ready()
  {
    base._Ready();

    AddToGroup(Entity.Name + "EquipmentSlots");
    AddToGroup("EquipmentSlots");
    AddToGroup("ItemSlots");
  }

  public bool TryEquipItem(EntityEquipmentBase equipment)
  {
    if (!equipment.CanEquip(Entity, SlotType))
    {
      Entity.TryEquipFailEvent(equipment, SlotType);
      return false;
    }

    Equipment = equipment;
    Entity.TryEquipSuccessEvent(equipment, SlotType);
    return true;
  }
}