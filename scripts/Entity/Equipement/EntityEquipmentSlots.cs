
using Godot;

namespace Entity;

public class EntityEquipmentSlots(Entity entity)
{
  public Entity Entity = entity;

  public EntityEquipmentBase LeftHand { get; set; } = EntityEquipment.Instantiate(new EmptyEquipment());
  public EntityEquipmentBase RightHand { get; set; } = EntityEquipment.Instantiate(new EmptyEquipment());

  public EntityEquipmentBase Helmet { get; set; } = EntityEquipment.Instantiate(new EmptyEquipment());
  public EntityEquipmentBase Armor { get; set; } = EntityEquipment.Instantiate(new EmptyEquipment());
  public EntityEquipmentBase Neck { get; set; } = EntityEquipment.Instantiate(new EmptyEquipment());
  public EntityEquipmentBase Back { get; set; } = EntityEquipment.Instantiate(new EmptyEquipment());
  public EntityEquipmentBase Legs { get; set; } = EntityEquipment.Instantiate(new EmptyEquipment());
  public EntityEquipmentBase Boots { get; set; } = EntityEquipment.Instantiate(new EmptyEquipment());

  public EntityEquipmentBase AccessoryLeft { get; set; } = EntityEquipment.Instantiate(new EmptyEquipment());
  public EntityEquipmentBase AccessoryRight { get; set; } = EntityEquipment.Instantiate(new EmptyEquipment());
}
