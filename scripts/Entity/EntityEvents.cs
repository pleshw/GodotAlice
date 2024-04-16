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


  public event Action<EntityActionInfo> OnWasHitEvent;
  public void WasHitEvent(EntityActionInfo hitInfo)
  {
    OnWasHitEvent?.Invoke(hitInfo);
  }

  public event Action<EntityActionInfo> OnDodgeEvent;
  public void DodgeEvent(EntityActionInfo hitInfo)
  {
    OnDodgeEvent?.Invoke(hitInfo);
  }

  public event Action<EntityActionInfo> OnTookNoDamageEvent;
  public void TookNoDamageEvent(EntityActionInfo hitInfo)
  {
    OnTookNoDamageEvent?.Invoke(hitInfo);
  }

  public event Action<Entity, EntityActionInfo> OnDealtNoDamageEvent;
  public void DealtNoDamageEvent(Entity target, EntityActionInfo hitInfo)
  {
    OnDealtNoDamageEvent?.Invoke(target, hitInfo);
  }

  public event Action<Entity, EntityActionInfo> OnAttackMissedEvent;
  public void AttackMissedEvent(Entity target, EntityActionInfo hitInfo)
  {
    OnAttackMissedEvent?.Invoke(target, hitInfo);
  }

  public event Action<Entity, EntityActionInfo> OnCriticalAttackEvent;
  public void CriticalAttackEvent(Entity target, EntityActionInfo hitInfo)
  {
    OnCriticalAttackEvent?.Invoke(target, hitInfo);
  }

  public event Action<EntityActionInfo> OnTookCriticalEvent;
  public void TookCriticalEvent(EntityActionInfo hitInfo)
  {
    OnTookCriticalEvent?.Invoke(hitInfo);
  }

  public event Action<Entity, EntityActionInfo> OnCriticalZeroDamageEvent;
  public void CriticalZeroDamageEvent(Entity target, EntityActionInfo hitInfo)
  {
    OnCriticalZeroDamageEvent?.Invoke(target, hitInfo);
  }

  public event Action<EntityActionInfo> OnTookZeroCriticalDamageEvent;
  public void TookZeroCriticalDamageEvent(EntityActionInfo hitInfo)
  {
    OnTookZeroCriticalDamageEvent?.Invoke(hitInfo);
  }

  public event Action<Entity, EntityActionInfo> OnMarkedAsTargetEvent;
  public void MarkedAsTargetEvent(Entity target, EntityActionInfo hitInfo)
  {
    OnMarkedAsTargetEvent?.Invoke(target, hitInfo);
  }


  public event Action<Entity, EntityActionInfo> OnNotEnoughRangeEvent;
  public void NotEnoughRangeEvent(Entity target, EntityActionInfo hitInfo)
  {
    OnNotEnoughRangeEvent?.Invoke(target, hitInfo);
  }
}