using System;
using Extras;
using Godot;

namespace Entity;

public class DamageTypeProportion : ProportionDictionary<DamageType>
{
  public DamageTypeProportion(float physicalDamagePercent, float magicalDamagePercent) : base()
  {
    Add(DamageType.PHYSICAL, physicalDamagePercent);
    Add(DamageType.MAGICAL, magicalDamagePercent);
  }
}

public record class EntityActionInfo
{

  public Random random = new();
  public required Entity Attacker;

  public required int WeaponExtraDamage;

  public required int RangeInCells;

  public required bool IsMelee;

  /// <summary>
  /// The percentage of damage that is magical and physical 
  /// </summary>
  public required DamageTypeProportion DamageProportion;

  /// <summary>
  /// The percentage of damage that is magical and physical 
  /// </summary>
  public required DamageElementalProperty DamageElementalProperty;

  public float MinMaxDamageModifier
  {
    get
    {
      float randomModifier = random.NextFloatInRange(1, 1.5f);
      return 1 + randomModifier / 100;
    }
  }

  public float AttackerWeaponMasteryDamageBonus { get; } = 50;

  public int DamageByDistance
  {
    get
    {
      return IsMelee ? Attacker.Stats.MinDamageMelee : Attacker.Stats.MinDamageRanged;
    }
  }

  public int DamagePoints
  {
    get
    {
      float flatDamage = ((DamageByDistance * 2) + (WeaponExtraDamage * 2)) * MinMaxDamageModifier;
      return Mathf.RoundToInt(flatDamage + AttackerWeaponMasteryDamageBonus);
    }
  }

  public int DamagePointsElementalWeakness
  {
    get
    {
      float flatDamage = (DamageByDistance * 2) + WeaponExtraDamage;
      float magicalDamageModifier = 3 * DamageProportion[DamageType.MAGICAL];
      float extraMagicalDamage = WeaponExtraDamage * magicalDamageModifier;
      float finalDamagePoints = (flatDamage + extraMagicalDamage) * MinMaxDamageModifier;
      return Mathf.RoundToInt(finalDamagePoints);
    }
  }
}