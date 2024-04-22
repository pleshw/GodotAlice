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

  public event Action OnStoppedEvent;
  public void EntityStoppedEvent()
  {
    OnStoppedEvent?.Invoke();
  }

  public event Action<Vector2, Vector2> OnMovedEvent;
  public void EntityMovedEvent(Vector2 from, Vector2 to)
  {
    OnMovedEvent?.Invoke(from, to);
  }

  public event Action<Vector2, Vector2> OnEntityDashedEvent;
  public void EntityDashedEvent(Vector2 from, Vector2 to)
  {
    OnEntityDashedEvent?.Invoke(from, to);
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

  // INPUT EVENTS BELOW

  public event Action OnMouseIn;
  public void MouseIn()
  {
    OnMouseIn?.Invoke();
  }

  public event Action OnMouseOut;
  public void MouseOut()
  {
    OnMouseOut?.Invoke();
  }

  // ANIMATION EVENTS BELOW

  public event Action OnBeforeAttackAnimationEvent;
  public void BeforeAttackAnimationEvent()
  {
    OnBeforeAttackAnimationEvent?.Invoke();
  }

  public event Action<int, int> OnAttackAnimationFrameChangeEvent;
  public void AttackAnimationFrameChangeEvent(int currentFrame, int animationFrameCount)
  {
    OnAttackAnimationFrameChangeEvent?.Invoke(currentFrame, animationFrameCount);
  }

  public event Action OnAfterAttackAnimationEvent;
  public void AfterAttackAnimationEvent()
  {
    OnAfterAttackAnimationEvent?.Invoke();
  }

  // COMBAT EVENTS BELOW

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

  public event Action<EntityGameState, EntityGameState> OnGameStateChangeEvent;
  public void GameStateChangeEvent(EntityGameState prev, EntityGameState actual)
  {
    OnGameStateChangeEvent?.Invoke(prev, actual);
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

  public event Action<Entity, EntityActionInfo> OnStartedCombatEvent;
  public void StartedCombatEvent(Entity target, EntityActionInfo hitInfo)
  {
    OnStartedCombatEvent?.Invoke(target, hitInfo);
  }

  public event Action<Entity, EntityActionInfo> OnNotEnoughRangeEvent;
  public void NotEnoughRangeEvent(Entity target, EntityActionInfo hitInfo)
  {
    OnNotEnoughRangeEvent?.Invoke(target, hitInfo);
  }

  public event Action<EntityActionInfo, int, bool> OnBeforeHealthLossEvent;
  public void BeforeHealthLossEvent(EntityActionInfo actionInfo, int damageTaken, bool wasCritical)
  {
    OnBeforeHealthLossEvent?.Invoke(actionInfo, damageTaken, wasCritical);
  }

  public event Action<EntityActionInfo, int, bool> OnAfterHealthLossEvent;
  public void AfterHealthLossEvent(EntityActionInfo actionInfo, int damageTaken, bool wasCritical)
  {
    OnAfterHealthLossEvent?.Invoke(actionInfo, damageTaken, wasCritical);
  }

  public event Action<EntityActionInfo, int, bool> OnBeforeDeathCheckEvent;
  public void BeforeDeathCheckEvent(EntityActionInfo actionInfo, int damageTaken, bool wasCritical)
  {
    OnBeforeDeathCheckEvent?.Invoke(actionInfo, damageTaken, wasCritical);
  }

  public event Action<EntityActionInfo, int, bool> OnAfterDeathCheckEvent;
  public void AfterDeathCheckEvent(EntityActionInfo actionInfo, int damageTaken, bool wasCritical)
  {
    OnAfterDeathCheckEvent?.Invoke(actionInfo, damageTaken, wasCritical);
  }

  public event Action<EntityActionInfo> OnConfirmDeathEvent;
  public void ConfirmDeathEvent(EntityActionInfo actionInfo)
  {
    OnConfirmDeathEvent?.Invoke(actionInfo);
  }
}