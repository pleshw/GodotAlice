using System;
using Godot;

namespace Entity;

public partial class EntityStats(Entity entity) : IEntityCombatStats
{
  public Entity Entity = entity;
  public int MinDamageMelee { get; }
  public int MinDamageRanged { get; }
  public int MagicDamage { get; }
  public int CriticalChance { get; }


  public float BaseAttackSpeed
  {
    get
    {
      return 158;
    }
  }


  public float AttackSpeedPointsFromEquip
  {
    get
    {
      return (195 - BaseAttackSpeedPoints) * 0.0f; /// 0.0f indica a soma dos buffs todos os equipamentos dá 0% de velocidade extra 
    }
  }

  public float AttackSpeedBuffs
  {
    get
    {
      return 0.0f; /// 0% de buff no attack speed de poções etc
    }
  }

  public float BaseAttackSpeedPoints
  {
    get
    {
      /// Dont know what this is. 14.31 is sqrt of 205
      float penalty = 1 - (BaseAttackSpeed - 144) / 50;
      float aspdCorrection = (float)((14.3178210633 - Math.Sqrt(AGI)) / 7.15);
      float aspdFromAttributes = (float)Math.Sqrt(AGI * 9.999 + DEX * 0.19212) * penalty;
      float aspdReduction = 200 - BaseAttackSpeed - aspdCorrection + aspdFromAttributes;
      float baseAttackSpeed = 200 - aspdReduction * (1 - AttackSpeedBuffs);
      return baseAttackSpeed;
    }
  }

  public float FinalAttackSpeedPoints
  {
    get
    {
      return BaseAttackSpeedPoints + AttackSpeedPointsFromEquip + 2; /// 2 são os pontos que você ganha de um equipamento que dá attack speed bruta e não em porcentagem
    }
  }

  public float AttacksPerSecond
  {
    get
    {
      return 50 / (200 - FinalAttackSpeedPoints);
    }
  }

  public int DodgeBonus { get; set; } = 0;

  /// <summary>
  /// Dodge chance is 100 - (AttackerHitPoints - EntityDodgePoints)
  /// </summary>
  public int DodgePoints
  {
    get
    {
      return 100 + DodgeBonus + Entity.Level + Entity.Attributes.Agility.Points + (Entity.Attributes.Luck.Points / 5);
    }
  }
  public int HitPoints { get; }

  public int DefensePercentage { get; }
  public int FlatDefense { get; }

  public int FlatMagicResistance { get; }


  public int STR
  {
    get
    {
      return Entity.Attributes.Strength.Points;
    }
  }

  public int AGI
  {
    get
    {
      return Entity.Attributes.Agility.Points;
    }
  }

  public int DEX
  {
    get
    {
      return Entity.Attributes.Dexterity.Points;
    }
  }

  public int VIT
  {
    get
    {
      return Entity.Attributes.Vitality.Points;
    }
  }

  public int LUK
  {
    get
    {
      return Entity.Attributes.Luck.Points;
    }
  }

  public int INT
  {
    get
    {
      return Entity.Attributes.Intelligence.Points;
    }
  }
}