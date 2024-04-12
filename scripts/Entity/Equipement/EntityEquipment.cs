
using Godot;

namespace Entity;

public class EntityEquipment(Entity entity)
{
  public Entity Entity = entity;

  public IEquipment LeftHand { get; set; } = new EmptyEquipment();
  public IEquipment RightHand { get; set; } = new EmptyEquipment();

  public IEquipment Helmet { get; set; } = new EmptyEquipment();
  public IEquipment Armor { get; set; } = new EmptyEquipment();
  public IEquipment Neck { get; set; } = new EmptyEquipment();
  public IEquipment Back { get; set; } = new EmptyEquipment();
  public IEquipment Legs { get; set; } = new EmptyEquipment();
  public IEquipment Boots { get; set; } = new EmptyEquipment();

  public IEquipment AccessoryLeft { get; set; } = new EmptyEquipment();
  public IEquipment AccessoryRight { get; set; } = new EmptyEquipment();
}
