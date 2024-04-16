using System;
using System.Collections.Generic;
using Extras;
using Godot;

namespace Entity;


public partial class EntityCombatController(Entity entity, int sightRange = 0, int awarenessRange = 0) : IEntityAction
{
  public Entity entity = entity;


  private readonly Random random = new();

  public int SightRange { get; set; } = sightRange;

  /// <summary>
  /// The range a unit has to be from another to activate awareness from danger 
  /// </summary>
  public int AwarenessRange { get; set; } = awarenessRange;

  /// <summary>
  /// If the unit know it is in danger. Can be used to improve temporarily some units sight range or something like that.
  /// An unaware unit will always take max damage from an attacker.
  /// </summary>
  public bool IsAware { get; set; } = false;

  public bool CancelOnNextIteration { get; set; } = false;
  public bool IsPerformingAction { get; set; } = false;

  public int EnemiesAround { get; } = 1;
  public List<DamageElementalProperty> ElementalWeaknesses { get; set; } = [];

  public bool TryAttack(Entity target, EntityActionInfo actionInfo)
  {
    int distanceFromTarget = entity.DistanceInCells(target.Position, entity.MovementController.CellWidth);
    if (actionInfo.RangeInCells < distanceFromTarget)
    {
      target.CombatController.UpdateAwareness(actionInfo, distanceFromTarget);
      entity.CombatController.NotEnoughRangeEvent(target, actionInfo);
      entity.AttackMissedEvent(target, actionInfo);
      return false;
    }

    target.CombatController.MarkedAsTargetEvent(entity, actionInfo);

    if (target.CombatController.TryDodge(entity))
    {
      target.CombatController.DodgeEvent(entity, actionInfo);
      entity.CombatController.AttackDodgedEvent(target, actionInfo);
      target.DodgeEvent(actionInfo);
      entity.AttackMissedEvent(target, actionInfo);
      return false;
    }

    target.CombatController.TakeHit(actionInfo, out int damageTaken, out bool wasCritical);
    if (actionInfo.WeaponExtraDamage > 0 && damageTaken == 0)
    {
      if (wasCritical)
      {
        target.CombatController.ZeroDamageTakenEvent(entity, actionInfo);
        entity.CombatController.ZeroDamageDealtEvent(target, actionInfo);
        target.TookZeroCriticalDamageEvent(actionInfo);
        entity.CriticalZeroDamageEvent(target, actionInfo);
      }
      else
      {
        target.CombatController.ZeroDamageTakenEvent(entity, actionInfo);
        entity.CombatController.ZeroDamageDealtEvent(target, actionInfo);
        target.TookNoDamageEvent(actionInfo);
        entity.DealtNoDamageEvent(target, actionInfo);
      }

      return true;
    }

    if (wasCritical)
    {
      entity.CriticalAttackEvent(target, actionInfo);
      target.TookCriticalEvent(actionInfo);
    }

    return true;
  }

  private bool TryDodge(Entity attacker)
  {
    float enemiesAroundModifier = 1 - (EnemiesAround - 2) * 0.1f;
    float totalDodgePoints = entity.Stats.DodgePoints * enemiesAroundModifier;

    int totalDodgeChance = Mathf.RoundToInt(100 - (attacker.Stats.HitPoints - totalDodgePoints));

    int randomNumber = random.Next(0, 100);

    return randomNumber < totalDodgeChance;
  }

  private bool CheckCritical()
  {
    int randomNumber = random.Next(0, 100);

    return randomNumber < entity.Stats.CriticalChance;
  }

  private void TakeHit(EntityActionInfo actionInfo, out int damageTaken, out bool wasCritical)
  {
    wasCritical = false;
    float totalDamagePoints = ElementalWeaknesses.Contains(actionInfo.DamageElementalProperty)
      ? actionInfo.DamagePointsElementalWeakness
      : actionInfo.DamagePoints;

    float flatDamageTaken = (totalDamagePoints * entity.Stats.DefensePercentage) - entity.Stats.FlatDefense;

    if (CheckCritical())
    {
      flatDamageTaken *= 2;
      wasCritical = true;
    }

    damageTaken = Mathf.RoundToInt(flatDamageTaken);

    entity.Stats.CurrentHealth -= damageTaken;
    entity.WasHitEvent(actionInfo);
  }

  private void UpdateAwareness(EntityActionInfo actionInfo, int distanceFromPerformer)
  {
    int distanceFromAction = distanceFromPerformer - actionInfo.RangeInCells;
    if (distanceFromAction > AwarenessRange)
    {
      return;
    }

    IsAware = true;
  }
}