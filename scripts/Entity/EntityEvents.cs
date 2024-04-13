using System;
using Godot;

namespace Entity;

public partial class Entity
{
  public event Action OnStateChangedEvent;
  public void StateHasChangedEvent()
  {
    OnStateChangedEvent?.Invoke();
  }

  public event Action OnEntityStoppedEvent;
  public void EntityStoppedEvent()
  {
    OnEntityStoppedEvent?.Invoke();
  }

  public event Action<Vector2, Vector2> OnEntityMovedEvent;
  public void EntityMovedEvent(Vector2 from, Vector2 to)
  {
    OnEntityMovedEvent?.Invoke(from, to);
  }

  public event Action OnMovementStateUpdatedEvent;
  public void MovementStateUpdatedEvent()
  {
    OnMovementStateUpdatedEvent?.Invoke();
  }

  public event Action OnMovementInputTriggeredEvent;
  public void MovementInputEvent()
  {
    OnMovementInputTriggeredEvent?.Invoke();
  }

  public event Action<EntityEquipmentBase, EntityEquipmentSlotType> OnTryEquipFailEvent;
  public void TryEquipFailEvent(EntityEquipmentBase equipment, EntityEquipmentSlotType positionTriedToEquip)
  {
    OnTryEquipFailEvent?.Invoke(equipment, positionTriedToEquip);
  }

  public event Action<EntityEquipmentBase, EntityEquipmentSlotType> OnTryEquipSuccessEvent;
  public void TryEquipSuccessEvent(EntityEquipmentBase equipment, EntityEquipmentSlotType positionTriedToEquip)
  {
    OnTryEquipSuccessEvent?.Invoke(equipment, positionTriedToEquip);
  }
}