
using Godot;
using Items.Equipment;

namespace Entity;

public class EntityEquipmentSlots(Entity entity)
{
  public Entity Entity = entity;

  public EntityEquipmentBase LeftHand { get; set; } = new BareHands();
  public EntityEquipmentBase RightHand { get; set; } = new BareHands();

  public EntityEquipmentBase Helmet { get; set; }
  public EntityEquipmentBase Armor { get; set; }
  public EntityEquipmentBase Neck { get; set; }
  public EntityEquipmentBase Back { get; set; }
  public EntityEquipmentBase Legs { get; set; }
  public EntityEquipmentBase Boots { get; set; }

  public EntityEquipmentBase AccessoryLeft { get; set; }
  public EntityEquipmentBase AccessoryRight { get; set; }
}
