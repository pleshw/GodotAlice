using System.Collections.Generic;
using Animation;
using Godot;

namespace Entity;

public interface IEntityCombatStats
{
  public int MinDamageMelee { get; set; }
  public int MinDamageRanged { get; set; }
  public int CriticalChance { get; set; }
  public int DodgeChance { get; set; }
}