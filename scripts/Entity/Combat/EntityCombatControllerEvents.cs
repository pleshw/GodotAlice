using System;
using Godot;

namespace Entity;

public partial class EntityCombatController
{

  public event Action<Entity, EntityActionInfo> OnNotEnoughRangeEvent;
  public void NotEnoughRangeEvent(Entity target, EntityActionInfo actionInfo)
  {
    OnNotEnoughRangeEvent?.Invoke(target, actionInfo);
  }

  public event Action<Entity, EntityActionInfo> OnMarkedAsTargetEvent;
  public void MarkedAsTargetEvent(Entity target, EntityActionInfo actionInfo)
  {
    OnMarkedAsTargetEvent?.Invoke(target, actionInfo);
  }

  public event Action<Entity, EntityActionInfo> OnDodgeEvent;
  public void DodgeEvent(Entity attacker, EntityActionInfo actionInfo)
  {
    OnDodgeEvent?.Invoke(attacker, actionInfo);
  }

  public event Action<Entity, EntityActionInfo> OnAttackDodgedEvent;
  public void AttackDodgedEvent(Entity target, EntityActionInfo actionInfo)
  {
    OnAttackDodgedEvent?.Invoke(target, actionInfo);
  }

  public event Action<Entity, EntityActionInfo> OnFledEvent;
  public void FledEvent(Entity attacker, EntityActionInfo actionInfo)
  {
    OnFledEvent?.Invoke(attacker, actionInfo);
  }

  public event Action<Entity, EntityActionInfo> OnAttackMissedEvent;
  public void AttackMissedEvent(Entity target, EntityActionInfo actionInfo)
  {
    OnAttackMissedEvent?.Invoke(target, actionInfo);
  }

  public event Action<Entity, EntityActionInfo> OnZeroDamageTakenEvent;
  public void ZeroDamageTakenEvent(Entity attacker, EntityActionInfo actionInfo)
  {
    OnZeroDamageTakenEvent?.Invoke(attacker, actionInfo);
  }

  public event Action<Entity, EntityActionInfo> OnZeroDamageDealtEvent;
  public void ZeroDamageDealtEvent(Entity target, EntityActionInfo actionInfo)
  {
    OnZeroDamageDealtEvent?.Invoke(target, actionInfo);
  }
}